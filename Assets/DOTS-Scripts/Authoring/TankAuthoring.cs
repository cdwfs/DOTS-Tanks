using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class TankAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public float MoveSpeed = 12f;
    public float TurnSpeed = 180f;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new TankPlayer
        {
            PlayerId = -1,
            Score = 0,
        });
        dstManager.AddComponentData(entity, new TankMovementStats
        {
            MoveSpeed = MoveSpeed,
            TurnSpeed = TurnSpeed,
        });
        dstManager.AddComponentData(entity, new PlayerInputState());
        dstManager.AddComponent(entity, typeof(CameraTarget));
        dstManager.AddComponent(entity, typeof(Prefab));
    }
}
