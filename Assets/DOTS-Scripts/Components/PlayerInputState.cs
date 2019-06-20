using Unity.Entities;
using Unity.Mathematics;

public struct PlayerInputState : IComponentData
{
    public float Firing;
    public float2 Move;
}
