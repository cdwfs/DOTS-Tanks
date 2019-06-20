using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Transforms;

[AlwaysUpdateSystem] // TEMP
[UpdateInGroup(typeof(SimulationSystemGroup)), UpdateAfter(typeof(StepPhysicsWorld))]
public class FollowSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        // Copy transforms from entity world to game object world
        Entities.WithAll<FollowEntity>().ForEach((UnityEngine.Transform t, ref LocalToWorld l) =>
        {
            t.position = l.Value.c3.xyz;
        });
    }
}
