using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[AlwaysUpdateSystem] // TEMP
[UpdateInGroup(typeof(SimulationSystemGroup)), UpdateAfter(typeof(StepPhysicsWorld))]
public class FollowSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        // Copy transforms from entity world to game object world
        Entities.WithAll<FollowEntity>().ForEach((Transform t, ref LocalToWorld l) =>
        {
            t.position = l.Value.c3.xyz;
            
            // Must be a better way than this..
            var q = new Quaternion();
            q.w = Mathf.Sqrt(Mathf.Max(0, 1 + l.Value.c0.x + l.Value.c1.y + l.Value.c2.z)) / 2;
            q.x = Mathf.Sqrt(Mathf.Max(0, 1 + l.Value.c0.x - l.Value.c1.y - l.Value.c2.z)) / 2;
            q.y = Mathf.Sqrt(Mathf.Max(0, 1 - l.Value.c0.x + l.Value.c1.y - l.Value.c2.z)) / 2;
            q.z = Mathf.Sqrt(Mathf.Max(0, 1 - l.Value.c0.x - l.Value.c1.y + l.Value.c2.z)) / 2;
            q.x *= Mathf.Sign(q.x * (l.Value.c1.z - l.Value.c2.y));
            q.y *= Mathf.Sign(q.y * (l.Value.c2.x - l.Value.c0.z));
            q.z *= Mathf.Sign(q.z * (l.Value.c0.y - l.Value.c0.x));
            t.rotation = q;
        });
    }
}
