
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
            }else
            {
                physicsVelocity.Linear = 0;
            }
            if (math.lengthsq(inputState.Move.x) > 0f)
            {
                physicsVelocity.Angular = new float3(0, inputState.Move.x*tankMovementStats.MoveSpeed, 0);
            }else { physicsVelocity.Angular = 0; }
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
