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
        private float3 explosionPosition;
        private float explosionRadius = 10f;
        private float explosionForce = 10;
        private float explosionForceY = 0.5f;

        public float ExplosionRadius => explosionRadius;
        public float ExplosionForce => explosionForce;
        public float ExplosionForceY => explosionForceY;


        private bool fuseLit = false;
        public void Bang(float3 worldPosition)
        {
            fuseLit = true;
            explosionPosition = worldPosition;
        }

        protected override void OnCreate()
        {
            InputGatheringSystem inputSystem = World.GetOrCreateSystem<InputGatheringSystem>();
            inputSystem.TanksControls.UI.SetCallbacks(this);
            inputSystem.TanksControls.UI.Enable();
        }

        protected override void OnUpdate()
        {
            //
            if(fuseLit)
            {
                var translationGroup = GetComponentDataFromEntity<Translation>(true);
                var rotationGroup = GetComponentDataFromEntity<Rotation>(true);
                var physicsVelocityGroup = GetComponentDataFromEntity<PhysicsVelocity>();
                var physicsMassGroup = GetComponentDataFromEntity<PhysicsMass>(true);
                var healthGroup = GetComponentDataFromEntity<PlayerHealth>(true);

                MakeExplosion(ref World.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld,
                    translationGroup, rotationGroup,
                    physicsVelocityGroup, physicsMassGroup, 
                    healthGroup,
                    explosionPosition, explosionRadius, explosionForce, 0);
                fuseLit = false;
            }
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
                End = ray.origin + ray.direction * 1000f,
                Filter = CollisionFilter.Default,
            };
            RaycastHit closestHit = new RaycastHit();
            if (collisionWorld.CastRay(raycastInput, out closestHit))
            {
                Bang(closestHit.Position);
            }
        }
        
        public static void MakeExplosion(ref PhysicsWorld physicsWorld,
            ComponentDataFromEntity<Translation> translationGroup, ComponentDataFromEntity<Rotation> rotationGroup,
            ComponentDataFromEntity<PhysicsVelocity> physicsVelocityGroup, ComponentDataFromEntity<PhysicsMass> physicsMassGroup,
            ComponentDataFromEntity<PlayerHealth> healthGroup,
            float3 origin, float explosionRadius, float explosionForce, float explosionMaxDamage = 0f)
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

            NativeList<int> rigidBodyIndexs = new NativeList<int>(256, Allocator.Temp); // indexes into world.Bodies[]
                
            if (physicsWorld.CollisionWorld.OverlapAabb(input, ref rigidBodyIndexs))
            {
                for (int i = 0; i < rigidBodyIndexs.Length; i++)
                {
                    RigidBody body = physicsWorld.Bodies[rigidBodyIndexs[i]];
                        
                    if (physicsVelocityGroup.Exists(body.Entity) && physicsMassGroup.Exists(body.Entity))
                    {
                        float distance = math.distance(body.WorldFromBody.pos, origin);
                        if (distance < explosionRadius)
                        {
                            float gain = math.clamp((explosionRadius - distance) / explosionRadius, 0, 1);
                            float force = gain * explosionForce;

                            float3 toBody = body.WorldFromBody.pos - origin;
                            float3 impulse = math.normalize(toBody) * force;
                            //impulse.y = explosionForceY;

                            var velocity = physicsVelocityGroup[body.Entity];
                            var mass = physicsMassGroup[body.Entity];
                            var translation = translationGroup[body.Entity];
                            var rotation = rotationGroup[body.Entity];

                            velocity.ApplyImpulse(mass, translation, rotation, impulse, origin);

                            physicsVelocityGroup[body.Entity] = velocity;

                            if (healthGroup.Exists(body.Entity))
                            {
                                var health = healthGroup[body.Entity];
                                health.Health -= explosionMaxDamage * gain;
                                healthGroup[body.Entity] = health;
                            }
                        }
                    }
                }
            }

            rigidBodyIndexs.Dispose();
        }

    }
}