using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class TankAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public int playerId;
    public float moveSpeed = 12f;
    public float turnSpeed = 180f;
    public Transform fireTransform;
    public float minLaunchForce = 15.0f;
    public float maxLaunchForce = 30.0f;
    public float maxChargeTime = 0.75f;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new TankPlayer
        {
            PlayerId = playerId,
            Score = 0,
        });
        dstManager.AddComponentData(entity, new TankMovementStats
        {
            MoveSpeed = moveSpeed,
            TurnSpeed = turnSpeed,
        });
        dstManager.AddComponentData(entity, new TankAttackStats
        {
            MinLaunchForce = minLaunchForce,
            MaxLaunchForce = maxLaunchForce,
            ChargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime,
            CurrentLaunchForce = minLaunchForce,
            IsCharging = 0,

            ShellSpawnPositionOffset = fireTransform.localPosition,
            ShellSpawnRotationOffset = fireTransform.localRotation,
        });
        dstManager.AddComponentData(entity, new PlayerInputState());
        dstManager.AddComponent(entity, typeof(CameraTarget));
        dstManager.AddComponent(entity, typeof(Prefab));
    }
}
