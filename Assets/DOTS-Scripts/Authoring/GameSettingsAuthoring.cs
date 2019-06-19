using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GameSettingsAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public int ScoreToWin;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new GameSettings()
        {
            ScoreToWin = ScoreToWin,
        });
    }
}
