using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(EndFramePhysicsSystem))]
[UpdateBefore(typeof(TransformSystemGroup))]
public class AerialAnimationSystem : JobComponentSystem
{
    protected struct AnimateAerialsJob : IJobForEach<Aerial>
    {
        [NativeDisableParallelForRestriction]
        public ComponentDataFromEntity<Rotation> RotationFromEntity;
        
        public float DeltaTime;
        
        [BurstCompile]
        public void Execute(ref Aerial aerial)
        {
            var data2 = RotationFromEntity[aerial.Entity2];
            var angle = aerial.WobbleMagnitude * math.cos(5 * aerial.WobbleTime);
            var delta = quaternion.EulerXYZ(0, 0, angle);
            data2.Value = math.mul(aerial.ReferenceRotation2, delta);
            RotationFromEntity[aerial.Entity2] = data2;

            aerial.WobbleMagnitude *= 0.99f;
            aerial.WobbleTime += DeltaTime;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new AnimateAerialsJob
        {
            RotationFromEntity = GetComponentDataFromEntity<Rotation>(),
        }.Schedule(this, inputDeps);
        return job;
    }
}