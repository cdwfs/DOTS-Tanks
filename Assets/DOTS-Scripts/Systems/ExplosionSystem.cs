using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Physics.Extensions;
using Unity.Transforms;
using static Unity.Physics.Math;
using Collider = Unity.Physics.Collider;
using Ray = UnityEngine.Ray;
using RaycastHit = Unity.Physics.RaycastHit;
using SphereCollider = Unity.Physics.SphereCollider;

namespace Systems
{
    public class ExplosionSystem : ComponentSystem, TanksControls.IUIActions
    {
        private Vector2 mousePosition;
        private float explosionRadius = 10f;
        private float explosionForce = 10;
        private float explosionForceY = 0.5f;
        
        protected override void OnCreate()
        {
            InputGatheringSystem inputSystem = World.GetOrCreateSystem<InputGatheringSystem>();
            inputSystem.TanksControls.UI.SetCallbacks(this);
            inputSystem.TanksControls.UI.Enable();
        }

        protected override void OnUpdate()
        {
            //
        }
        
        public void OnMousePosition(InputAction.CallbackContext context)
        {
            mousePosition = context.ReadValue<Vector2>();
        }
        
        public void OnMouseLeftButton(InputAction.CallbackContext context)
        {
            BuildPhysicsWorld stepWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            CollisionWorld collisionWorld = stepWorld.PhysicsWorld.CollisionWorld;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastInput raycastInput = new RaycastInput
            {
                Start = ray.origin,
                End = ray.origin + ray.direction * 100000f,
                Filter = CollisionFilter.Default,
            };
            RaycastHit closestHit = new RaycastHit();
            if (collisionWorld.CastRay(raycastInput, out closestHit))
            {
                MakeExplosion(closestHit.Position);
            }
        }
        
        void MakeExplosion(float3 origin)
        {
            float3 min = new float3(origin.x - explosionRadius, origin.y - explosionRadius, origin.z - explosionRadius);
            float3 max = new float3(origin.x + explosionRadius, origin.y + explosionRadius, origin.z + explosionRadius);
            Aabb explosionBounds = new Aabb
            {
                Min = min,
                Max = max,
            };

            OverlapAabbInput input = new OverlapAabbInput
            {
                Aabb = explosionBounds,
                Filter = CollisionFilter.Default,
            };
                
            NativeList<int> rigidBodyIndexs = new NativeList<int>(256, Allocator.TempJob); // indexes into world.Bodies[]
                
            BuildPhysicsWorld world = World.GetOrCreateSystem<BuildPhysicsWorld>();
            if (world.PhysicsWorld.CollisionWorld.OverlapAabb(input, ref rigidBodyIndexs))
            {
                for (int i = 0; i < rigidBodyIndexs.Length; i++)
                {
                    RigidBody body = world.PhysicsWorld.Bodies[rigidBodyIndexs[i]];
                        
                    float distance = math.distance(body.Entity.GetPosition(), origin);
                    float force = (explosionRadius - distance)/explosionRadius * explosionForce;
                        
                    float3 toTank = body.Entity.GetPosition() - origin;
                    float3 impulse = math.normalize(toTank) * force;
                    impulse.y = explosionForceY;
                        
                    body.Entity.ApplyImpulse(impulse, origin);
                }
            }

            rigidBodyIndexs.Dispose();
        }

    }
}