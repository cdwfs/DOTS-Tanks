using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine.UIElements;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class TankFiringSystem : JobComponentSystem
{
    //[BurstCompile] -- uncomment when burstable ECBs are enabled
    struct SpawnShellsJob : IJobForEachWithEntity<PlayerInputState, Translation>
    {
        public EntityCommandBuffer.Concurrent CommandBuffer;
        public Entity ShellPrefab;
        public void Execute(Entity tankEntity, int jobIndex, [ReadOnly] ref PlayerInputState inputState, [ReadOnly] ref Translation tankPosition)
        {
            if (inputState.Firing != 0)
            {
                var shellEntity = CommandBuffer.Instantiate(jobIndex, ShellPrefab);
                CommandBuffer.SetComponent(jobIndex, shellEntity, tankPosition);
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
    