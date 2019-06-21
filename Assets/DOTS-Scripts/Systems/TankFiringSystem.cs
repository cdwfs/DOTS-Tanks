using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(TransformSystemGroup))]
public class TankFiringSystem : JobComponentSystem
{
    //[BurstCompile] -- uncomment when burstable ECBs are enabled
    struct SpawnShellsJob : IJobForEachWithEntity<PlayerInputState, LocalToWorld, Rotation, TankAttackStats, Aerial>
    {
        public EntityCommandBuffer.Concurrent CommandBuffer;
        public Entity ShellPrefab;
        public float DeltaTime;

        void FireShell(int jobIndex, [ReadOnly] ref LocalToWorld tankLocalToWorld, [ReadOnly] ref Rotation tankRotation, [ReadOnly] ref TankAttackStats attackStats, ref Aerial aerial)
        {
            var shellEntity = CommandBuffer.Instantiate(jobIndex, ShellPrefab);
            CommandBuffer.SetComponent(jobIndex, shellEntity, new Translation
            {
                Value = math.transform(tankLocalToWorld.Value, attackStats.ShellSpawnPositionOffset),
            });
            CommandBuffer.SetComponent(jobIndex, shellEntity, new Rotation
            {
                Value = math.mul(tankRotation.Value, attackStats.ShellSpawnRotationOffset),
            });
            CommandBuffer.SetComponent(jobIndex, shellEntity, new PhysicsVelocity
            {
                // TODO: add tank's current velocity
                Linear = math.mul(attackStats.ShellSpawnRotationOffset, tankLocalToWorld.Forward) * attackStats.CurrentLaunchForce,
            });
            attackStats.IsCharging = 0;

            // Start aerial wobble animation
            aerial.WobbleMagnitude = 1.0f;
            aerial.WobbleTime = 0.0f;
        }

        public void Execute(Entity tankEntity, int jobIndex, [ReadOnly] ref PlayerInputState inputState,
            [ReadOnly] ref LocalToWorld tankLocalToWorld, [ReadOnly] ref Rotation tankRotation, ref TankAttackStats attackStats, ref Aerial aerial)
        {
            // Track the current state of the fire button and make decisions based on the current launch force.
            if (attackStats.CurrentLaunchForce >= attackStats.MaxLaunchForce && attackStats.IsCharging != 0) {
                // at max charge, not fired -- force the player to fire at max force.
                attackStats.CurrentLaunchForce = attackStats.MaxLaunchForce;
                FireShell(jobIndex, ref tankLocalToWorld, ref tankRotation, ref attackStats, ref aerial);
            } else if (inputState.Firing != 0 && attackStats.CurrentLaunchForce == attackStats.MinLaunchForce && attackStats.IsCharging == 0) {
                // not charging, launch force at min, just pressed fire -- reset launch force and start charging
                attackStats.IsCharging = 1;
            } else if (attackStats.IsCharging == 0 && inputState.Firing == 0 && attackStats.CurrentLaunchForce > attackStats.MinLaunchForce) {
                // not charging, launch force > minimum, fire button released -- button finally released after an auto max-force shot.
                // Reset launch force for next shot.
                attackStats.CurrentLaunchForce = attackStats.MinLaunchForce;
            } else if (inputState.Firing != 0 && attackStats.IsCharging != 0) {
                // charging, fire button still down -- keep increasing the launch force
                attackStats.CurrentLaunchForce += attackStats.ChargeSpeed * DeltaTime;
            } else if (inputState.Firing == 0 && attackStats.IsCharging != 0) {
                // charging + fire button released -- fire and reset launch force for next shot
                FireShell(jobIndex, ref tankLocalToWorld, ref tankRotation, ref attackStats, ref aerial);
                attackStats.CurrentLaunchForce = attackStats.MinLaunchForce;
            }
        }
    }

    private BeginInitializationEntityCommandBufferSystem beginInitEcbSystem;
    private Entity ShellPrefab = Entity.Null;
    protected override void OnCreate()
    {
        beginInitEcbSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        // We explicitly create this query to force the system to only start running if there are player entities.
        // Otherwise, the system will always update once before the implicit Query from the IJobForEach is generated,
        // which causes the Shell prefab lookup to fail in Scenes where the prefab doesn't exist.
        this.GetEntityQueryForIJobForEach(typeof(SpawnShellsJob));
    }

    protected override void OnStartRunning()
    {
        var shellPrefabQuery = EntityManager.CreateEntityQuery(new EntityQueryDesc
        {
            All = new[] {ComponentType.ReadWrite<ShellStats>()},
            Options = EntityQueryOptions.IncludePrefab,
        });
        if (shellPrefabQuery.CalculateLength() != 1)
        {
            throw new InvalidOperationException("No Shell prefab detected?");
        }
        var prefabEntities = shellPrefabQuery.ToEntityArray(Allocator.TempJob);
        ShellPrefab = prefabEntities[0];
        prefabEntities.Dispose();
        shellPrefabQuery.Dispose();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var spawnShellsJob = new SpawnShellsJob
        {
            CommandBuffer = beginInitEcbSystem.CreateCommandBuffer().ToConcurrent(),
            DeltaTime = Time.deltaTime,
            ShellPrefab = ShellPrefab,
        }.Schedule(this, inputDeps);
        beginInitEcbSystem.AddJobHandleForProducer(spawnShellsJob);
        return spawnShellsJob;
    }
}
    