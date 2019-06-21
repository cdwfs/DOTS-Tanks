using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using Unity.Mathematics;

[AlwaysUpdateSystem]
[UpdateInGroup(typeof(SimulationSystemGroup)), UpdateAfter(typeof(EndFramePhysicsSystem))]
public class AerialAnimationSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        var em = World.Active.EntityManager;
        Entities.ForEach((ref Aerial aerial) =>
        {
            //var data1 = em.GetComponentData<Rotation>(aerial.Entity1);
            //data1.Value = math.mul(aerial.ReferenceRotation1, delta);
            //em.SetComponentData(aerial.Entity1, data1);

            var data2 = em.GetComponentData<Rotation>(aerial.Entity2);
            var angle = aerial.WobbleMagnitude * math.cos(5 * aerial.WobbleTime);
            var delta = quaternion.EulerXYZ(0, 0, angle);
            data2.Value = math.mul(aerial.ReferenceRotation2, delta);
            em.SetComponentData(aerial.Entity2, data2);

            //var data3 = em.GetComponentData<Rotation>(aerial.Entity3);
            //data3.Value = math.mul(aerial.ReferenceRotation3, delta);
            //em.SetComponentData(aerial.Entity2, data3);

            aerial.WobbleMagnitude *= 0.99f;
            aerial.WobbleTime += UnityEngine.Time.deltaTime;
        });
    }
}
