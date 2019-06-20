using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[RequiresEntityConversion]
[RequireComponent(typeof(Camera))]
public class CameraControlAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public float m_DampTime = 0.2f;
    public float m_ScreenEdgeBuffer = 4f;
    public float m_MinSize = 6.5f;
    public float m_ZoomSpeed = 0.2f;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var camera = GetComponent<Camera>();
        var data = new CameraControlComponent { DampTime = m_DampTime, MinSize = m_MinSize, ScreenEdgeBuffer = m_ScreenEdgeBuffer, CameraAspect = camera.aspect, CameraSize = camera.orthographicSize, ZoomSpeed = m_ZoomSpeed };
        dstManager.AddComponentData(entity, data);
    }
}
