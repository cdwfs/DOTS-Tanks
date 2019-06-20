using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ShellAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public float MaxDamage;
    public float ExplosionForce;
    public float MaxLifeTime;
    public float ExplosionRadius;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new ShellDuration
        {
            SecondsToLive = MaxLifeTime,
        });
        dstManager.AddComponentData(entity, new ShellStats
        {
            ExplosionForce = ExplosionForce,
            ExplosionRadius = ExplosionRadius,
            MaxDamage = MaxDamage,
        });
        dstManager.AddComponent(entity, typeof(Prefab));
    }
}
