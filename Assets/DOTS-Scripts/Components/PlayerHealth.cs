using System;
using Unity.Entities;

[Serializable]
public struct PlayerHealth : IComponentData
{
    public float Health;
}
