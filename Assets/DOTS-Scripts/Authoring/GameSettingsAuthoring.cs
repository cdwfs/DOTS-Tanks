using System;
using Unity.Entities;
using UnityEngine;

public class GameSettingsAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public int scoreToWin;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new GameSettings
        {
            ScoreToWin = scoreToWin,
        });
    }
}
