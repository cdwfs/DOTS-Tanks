using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct PlayerInputState : IComponentData
{
    public float Firing;
    public float2 Move;
}
