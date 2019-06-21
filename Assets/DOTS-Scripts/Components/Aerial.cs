using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct Aerial : IComponentData
{
    public Entity Entity0;

    public Entity Entity1;
    public quaternion ReferenceRotation1;

    public Entity Entity2;
    public quaternion ReferenceRotation2;

    public float BendAngle;
    public float BendSpeed;
}
