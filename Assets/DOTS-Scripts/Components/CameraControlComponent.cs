using System;
using Unity.Mathematics;
using Unity.Entities;

[Serializable]
public struct CameraControlComponent : IComponentData
{
    public float DampTime;
    public float ScreenEdgeBuffer;
    public float MinSize;
    public float ZoomSpeed;
    public float CameraAspect;
    public float CameraSize;
}
