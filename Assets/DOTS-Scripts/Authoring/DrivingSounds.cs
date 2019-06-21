using Components;
using Unity.Entities;
using UnityEngine;

namespace Unity.Physics.Authoring
{
    public class DrivingSounds : MonoBehaviour, IConvertGameObjectToEntity
    {
        public AudioClip IdleClip;
        public AudioClip DrivingClip;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
//            dstManager.AddSharedComponentData(entity, new EngineSounds{ Idle = IdleClip, Driving = DrivingClip});
            
            // TODO - find the ID of the player's tank entity
//            int playerId = GetComponentInParent<TankAuthoring>().playerId;
//            dstManager.AddComponentData(entity, new PlayerInputState { PlayerId = playerId });
        }
    }
}