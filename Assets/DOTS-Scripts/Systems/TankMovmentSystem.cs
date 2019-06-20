
using Unity.Transforms;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Unity.Physics;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class TankMovmentSystem : JobComponentSystem
{
    struct MovementJob : IJobForEachWithEntity<PlayerInputState, TankMovementStats, PhysicsVelocity , Rotation>
    {
        public void Execute(Entity entity, int index, ref PlayerInputState inputState, ref TankMovementStats tankMovementStats, ref PhysicsVelocity physicsVelocity, ref Rotation rotation)
        {
            if (math.lengthsq(inputState.Move.y) > 0f)
            {
                physicsVelocity.Linear = math.forward(rotation.Value)* inputState.Move.y*tankMovementStats.MoveSpeed;
                tankMovementStats.moving = true;
            }
            if (tankMovementStats.moving && math.lengthsq(inputState.Move.y) == 0f)
            {
                tankMovementStats.moving = false;
                physicsVelocity.Linear = 0;

            }
            if (math.lengthsq(inputState.Move.x) > 0f)
            {
                physicsVelocity.Angular =  new float3(0, inputState.Move.x*tankMovementStats.MoveSpeed, 0);
                tankMovementStats.rotating = true;
            }
            if (tankMovementStats.rotating && math.lengthsq(inputState.Move.x) == 0f)
            {
                physicsVelocity.Angular = 0;
                tankMovementStats.rotating = false;
            }
        }
    }
    private PlayerInputState playerInputState;
    protected override void OnCreate()
    {
        var inputSystem = World.GetOrCreateSystem<InputGatheringSystem>();
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var movementJob = new MovementJob
        { 
        }.Schedule(this, inputDeps);
        return movementJob;
    }

}
