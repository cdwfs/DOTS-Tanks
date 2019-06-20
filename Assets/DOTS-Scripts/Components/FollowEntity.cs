using System;
using Unity.Entities;

[Serializable]
public struct FollowEntity : IComponentData
{
    public bool Position;
    public bool Rotation;

}
