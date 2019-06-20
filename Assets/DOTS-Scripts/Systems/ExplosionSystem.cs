using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine.InputSystem;
using static Unity.Physics.Math;
using Ray = UnityEngine.Ray;

namespace Systems
{
    public class ExplosionSystem : ComponentSystem, TanksControls.IUIActions
    {
        private UnityEngine.Vector2 mousePosition;
        
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
            mousePosition = context.ReadValue<UnityEngine.Vector2>();
        }
        
        public void OnMouseLeftButton(InputAction.CallbackContext context)
        {
            BuildPhysicsWorld stepWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            CollisionWorld collisionWorld = stepWorld.PhysicsWorld.CollisionWorld;
            UnityEngine.Ray ray = UnityEngine.Camera.main.ScreenPointToRay(mousePosition);
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

        void MakeExplosion(float3 explosionPosition)
        {
            //
        }

    }
}