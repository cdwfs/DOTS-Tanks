using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
[RequiresEntityConversion]
public class HealthSliderAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public int m_PlayerId;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var data = new HealthSlider {PlayerId = m_PlayerId};

        dstManager.AddComponentData(entity, data);

    }
}
