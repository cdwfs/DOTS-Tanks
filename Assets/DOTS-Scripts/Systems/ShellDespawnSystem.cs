using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(TransformSystemGroup))]
public class ShellDespawnSystem : JobComponentSystem
{
    //[BurstCompile] -- uncomment when burstable ECBs are enabled
    struct DespawnShellsJob : IJobForEachWithEntity<ShellDuration>
    {
        public EntityCommandBuffer.Concurrent CommandBuffer;
        public float dt;
        public void Execute(Entity shellEntity, int jobIndex, ref ShellDuration duration)
        {
            duration.SecondsToLive -= dt;
            if (duration.SecondsToLive <= 0)
            {
                CommandBuffer.DestroyEntity(jobIndex, shellEntity);
            }
        }
    }

    private EndSimulationEntityCommandBufferSystem endSimEcbSystem;
    protected override void OnCreate()
    {
        endSimEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var despawnShellsJob = new DespawnShellsJob
        {
            CommandBuffer = endSimEcbSystem.CreateCommandBuffer().ToConcurrent(),
            dt = Time.deltaTime,
        }.Schedule(this, inputDeps);
        endSimEcbSystem.AddJobHandleForProducer(despawnShellsJob);
        return despawnShellsJob;
    }
}
    