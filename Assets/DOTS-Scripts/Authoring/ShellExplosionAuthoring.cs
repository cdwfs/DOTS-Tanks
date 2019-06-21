using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ShellExplosionAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent(entity, typeof(ShellExplosionTag));
    }
}

public struct ShellExplosionTag : IComponentData { };