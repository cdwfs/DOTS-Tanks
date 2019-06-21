
using Unity.Transforms;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Physics.Systems;

[UpdateBefore(typeof(BuildPhysicsWorld))]
public class TankMovementSystem : JobComponentSystem
{
    struct MovementJob : IJobForEach<PlayerInputState, TankMovementStats, PhysicsVelocity, PhysicsMass, Rotation>
    {
        public void Execute(ref PlayerInputState inputState, ref TankMovementStats tankMovementStats, ref PhysicsVelocity physicsVelocity, ref PhysicsMass physicsMass, ref Rotation rotation)
        {
            // damping tank velocities toward zero
            float3 linear = math.lerp(physicsVelocity.Linear, float3.zero, 0.05f);
            float3 angular = math.lerp(physicsVelocity.Angular, float3.zero, 0.1f);

            if (math.lengthsq(inputState.Move.y) > 0f)
            {
                linear = math.forward(rotation.Value)* inputState.Move.y*tankMovementStats.MoveSpeed;

            }
            if (math.lengthsq(inputState.Move.x) > 0f)
            {
                angular =  new float3(0, inputState.Move.x*tankMovementStats.TurnSpeed, 0);
                if (inputState.Move.y < 0) angular = -angular;
            }

            // reapply linear elements that are not controlled
            physicsVelocity.Linear = linear;

            // reapply angular elements on axes that are not controlled

            physicsVelocity.SetAngularVelocity(physicsMass, rotation, angular);
        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var movementJob = new MovementJob
        { 
        }.Schedule(this, inputDeps);
        return movementJob;
    }

}
