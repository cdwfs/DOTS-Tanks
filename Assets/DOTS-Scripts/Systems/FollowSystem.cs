using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(TransformSystemGroup))]
public class FollowSystem : JobComponentSystem
{
    EntityQuery m_FollowTranslationGroup;
    EntityQuery m_FollowRotationGroup;
    EntityQuery m_FollowTranslationAndRotationGroup;

    protected override void OnCreate()
    {
        m_FollowTranslationGroup = GetEntityQuery(new EntityQueryDesc {
            All = new[]
            {
                typeof(Transform),
                ComponentType.ReadOnly<FollowEntityTranslation>(),
                ComponentType.ReadOnly<LocalToWorld>(),
            },
            None = new[] { ComponentType.ReadOnly<FollowEntityRotation>() }
        });
        m_FollowRotationGroup = GetEntityQuery(new EntityQueryDesc {
            All = new[]
            {
                typeof(Transform),
                ComponentType.ReadOnly<FollowEntityRotation>(),
                ComponentType.ReadOnly<LocalToWorld>()
            },
            None = new [] { ComponentType.ReadOnly<FollowEntityTranslation>() }
        });
        m_FollowTranslationAndRotationGroup = GetEntityQuery(new EntityQueryDesc {
            All = new[]
            {
                typeof(Transform),
                ComponentType.ReadOnly<FollowEntityTranslation>(),
                ComponentType.ReadOnly<FollowEntityRotation>(),
                ComponentType.ReadOnly<LocalToWorld>()
            }
        });
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var transformT = m_FollowTranslationGroup.GetTransformAccessArray();
        var ltwT = m_FollowTranslationGroup.ToComponentDataArray<LocalToWorld>(Allocator.TempJob, out inputDeps);
        var followT = new FollowTranslationJob { LocalToWorlds = ltwT };
        var handleT = followT.Schedule(transformT, inputDeps);

        var transformR = m_FollowRotationGroup.GetTransformAccessArray();
        var ltwR = m_FollowRotationGroup.ToComponentDataArray<LocalToWorld>(Allocator.TempJob, out inputDeps);
        var followR = new FollowRotationJob { LocalToWorlds = ltwR };
        var handleR = followR.Schedule(transformR, inputDeps);

        var transformTR = m_FollowTranslationAndRotationGroup.GetTransformAccessArray();
        var ltwTR = m_FollowTranslationAndRotationGroup.ToComponentDataArray<LocalToWorld>(Allocator.TempJob, out inputDeps);
        var followTR = new FollowTranslationAndRotationJob { LocalToWorlds = ltwTR };
        var handleTR = followTR.Schedule(transformTR, inputDeps);

        return JobHandle.CombineDependencies(JobHandle.CombineDependencies(handleT, handleR), handleTR);
    }

    [BurstCompile]
    struct FollowTranslationJob : IJobParallelForTransform
    {
        [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<LocalToWorld> LocalToWorlds;
        public void Execute(int index, TransformAccess transform) =>
            transform.position = LocalToWorlds[index].Value.c3.xyz;
    }

    [BurstCompile]
    struct FollowRotationJob : IJobParallelForTransform
    {
        [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<LocalToWorld> LocalToWorlds;
        public void Execute(int index, TransformAccess transform) =>
            transform.rotation = new quaternion(LocalToWorlds[index].Value);
    }

    [BurstCompile]
    struct FollowTranslationAndRotationJob : IJobParallelForTransform
    {
        [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<LocalToWorld> LocalToWorlds;
        public void Execute(int index, TransformAccess transform)
        {
            var ltw = LocalToWorlds[index].Value;
            transform.position = ltw.c3.xyz;
            transform.rotation = new quaternion(ltw);
        }
    }
}
