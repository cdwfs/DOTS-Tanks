
using Unity.Transforms;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Unity.Physics;
using Unity.Physics.Extensions;

[UpdateBefore(typeof(TransformSystemGroup))]
public class TankMovementSystem : JobComponentSystem
{
    struct MovementJob : IJobForEach<PlayerInputState, TankMovementStats, PhysicsVelocity, PhysicsMass, Rotation>
    {
        public void Execute(ref PlayerInputState inputState, ref TankMovementStats tankMovementStats, ref PhysicsVelocity physicsVelocity, ref PhysicsMass physicsMass, ref Rotation rotation)
        {
            // save the y component as we don't want to mess with gravity.
            // TODO: should this be math.dot against math.up?
            float3 oldLinear = physicsVelocity.Linear;
            float3 oldAngular = physicsVelocity.Angular;

            // damping tank velocities toward zero
            float3 linear = math.lerp(physicsVelocity.Linear, float3.zero, 0.5f);
            float3 angular = math.lerp(physicsVelocity.Angular, float3.zero, 0.5f);

            if (math.lengthsq(inputState.Move.y) > 0f)
            {
                linear = math.forward(rotation.Value)* inputState.Move.y*tankMovementStats.MoveSpeed;
                tankMovementStats.moving = true;
            }
            //if (tankMovementStats.moving && math.lengthsq(inputState.Move.y) == 0f)
            //{
            //    linear = math.lerp(physicsVelocity.Linear, float3.zero, 0.5f);
            //    tankMovementStats.moving = math.lengthsq(linear) > 0.01f;
            //}
            if (math.lengthsq(inputState.Move.x) > 0f)
            {
                angular =  new float3(0, inputState.Move.x*tankMovementStats.TurnSpeed, 0);
                if (inputState.Move.y < 0) angular = -angular;
                tankMovementStats.rotating = true;
            }
            //if (tankMovementStats.rotating && math.lengthsq(inputState.Move.x) == 0f)
            //{
            //    angular = math.lerp(physicsVelocity.Angular, float3.zero, 0.5f);
            //    tankMovementStats.rotating = math.lengthsq(angular) > 0.01f;
            //}

            // reapply linear elements that are not controlled
            linear.y = oldLinear.y;
            physicsVelocity.Linear = linear;

            // reapply angular elements on axes that are not controlled
            angular.x = 0;// oldAngular.x;
            angular.z = 0;// oldAngular.z;
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
