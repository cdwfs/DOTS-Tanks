using System;
using Unity.Entities;

[Serializable]
public struct TankMovementStats : IComponentData
{
    public float MoveSpeed;
    public float TurnSpeed;
}
