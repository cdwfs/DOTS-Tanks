using System;
using Unity.Entities;

[Serializable]
public struct TankMovementStats : IComponentData
{
    public float MoveSpeed;
    public float TurnSpeed;
    public bool moving;
    public bool rotating;
}
