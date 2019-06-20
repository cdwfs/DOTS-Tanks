﻿using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))]
public class TankFiringSystem : JobComponentSystem
{
    //[BurstCompile] -- uncomment when burstable ECBs are enabled
    struct SpawnShellsJob : IJobForEachWithEntity<PlayerInputState, LocalToWorld, Rotation, TankAttackStats>
    {
        public EntityCommandBuffer.Concurrent CommandBuffer;
        public Entity ShellPrefab;
        public void Execute(Entity tankEntity, int jobIndex, ref PlayerInputState inputState,
            [ReadOnly] ref LocalToWorld tankLocalToWorld, [ReadOnly] ref Rotation tankRotation, [ReadOnly] ref TankAttackStats attackStats)
        {
            if (inputState.Firing != 0)
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
                    Linear = math.mul(attackStats.ShellSpawnRotationOffset, tankLocalToWorld.Forward) * attackStats.MuzzleVelocity,
                });
                inputState.Firing = 0;
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
            ShellPrefab = ShellPrefab,
        }.Schedule(this, inputDeps);
        beginInitEcbSystem.AddJobHandleForProducer(spawnShellsJob);
        return spawnShellsJob;
    }
}
    