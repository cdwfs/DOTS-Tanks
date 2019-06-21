using Unity.Entities;
using UnityEngine;

public class FollowEntityAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public bool FollowPosition = true;
    public bool FollowRotation = true;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        if (FollowPosition) dstManager.AddComponent(entity, typeof(FollowEntityTranslation));
        if (FollowRotation) dstManager.AddComponent(entity, typeof(FollowEntityRotation));
    }
}
