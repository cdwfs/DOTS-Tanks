using System;
using Unity.Entities;

[Serializable]
public struct ShellStats : IComponentData
{
    public float MaxDamage;
    public float ExplosionForce;
    public float ExplosionRadius;
}
