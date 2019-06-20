using Unity.Entities;
using UnityEngine;

public class FollowEntityAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public bool FollowPosition = true;
    public bool FollowRotation = true;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new FollowEntity()
        {
            Position = FollowPosition,
            Rotation = FollowRotation
        });
    }
}
