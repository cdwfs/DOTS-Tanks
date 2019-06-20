using System;
using Unity.Entities;

[Serializable]
public struct TankPlayer : IComponentData
{
    public int Score;
    public int PlayerId;
}
