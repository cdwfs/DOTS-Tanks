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
    struct SpawnShellsJob : IJobForEachWithEntity<PlayerInputState, LocalToWorld, Rotation, TankAttackStats>
    {
        public EntityCommandBuffer.Concurrent CommandBuffer;
        public Entity ShellPrefab;
        public float DeltaTime;

        void FireShell(int jobIndex, [ReadOnly] ref LocalToWorld tankLocalToWorld, [ReadOnly] ref Rotation tankRotation, [ReadOnly] ref TankAttackStats attackStats)
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
        }

        public void Execute(Entity tankEntity, int jobIndex, [ReadOnly] ref PlayerInputState inputState,
            [ReadOnly] ref LocalToWorld tankLocalToWorld, [ReadOnly] ref Rotation tankRotation, ref TankAttackStats attackStats)
        {
            // Track the current state of the fire button and make decisions based on the current launch force.
            if (attackStats.CurrentLaunchForce >= attackStats.MaxLaunchForce && attackStats.IsCharging != 0) {
                // at max charge, not fired -- force the player to fire at max force.
                attackStats.CurrentLaunchForce = attackStats.MaxLaunchForce;
                FireShell(jobIndex, ref tankLocalToWorld, ref tankRotation, ref attackStats);
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
                FireShell(jobIndex, ref tankLocalToWorld, ref tankRotation, ref attackStats);
                attackStats.CurrentLaunchForce = attackStats.MinLaunchForce;
            }
        }
    }

    private BeginInitializationEntityCommandBufferSystem beginInitEcbSystem;
    private Entity ShellPrefab = Entity.Null;
    protected override void OnCreate()
    {
        beginInitEcbSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        // TODO: This block should be moved to OnCreate() once I figure out how to ensure it runs after the GameObject conversion system.
        if (ShellPrefab == Entity.Null)
        {
            var shellPrefabQuery = EntityManager.CreateEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<ShellStats>()},
                Options = EntityQueryOptions.IncludePrefab,
            });
            int count = shellPrefabQuery.CalculateLength();
            if (count == 0)
            {
                shellPrefabQuery.Dispose();
                return inputDeps; // in case no match was found for the first few frames
            }
            if (count == 1)
            {
                var prefabEntities = shellPrefabQuery.ToEntityArray(Allocator.TempJob);
                ShellPrefab = prefabEntities[0];
                prefabEntities.Dispose();
            }
            shellPrefabQuery.Dispose();
        }

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
    