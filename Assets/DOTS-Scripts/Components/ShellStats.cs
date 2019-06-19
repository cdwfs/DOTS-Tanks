using Unity.Entities;

public struct ShellStats : IComponentData
{
    public float MaxDamage;
    public float ExplosionForce;
    public float ExplosionRadius;
}
