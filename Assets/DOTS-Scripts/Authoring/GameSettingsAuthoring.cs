using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class GameSettingsAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    [Serializable]
    public struct PlayerInfo
    {
        public GameObject tankPrefab;
    }
    
    public int scoreToWin;
    public PlayerInfo[] players;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new GameSettings()
        {
            ScoreToWin = scoreToWin,
        });
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        foreach (var player in players)
        {
            referencedPrefabs.Add(player.tankPrefab);
        }
    }
}
