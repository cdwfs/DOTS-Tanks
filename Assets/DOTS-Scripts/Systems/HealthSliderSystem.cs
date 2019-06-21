using System;
using Unity.Transforms;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;

[UpdateBefore(typeof(TransformSystemGroup))]

public class HealthSliderSystem : ComponentSystem
{
    private Entity player1Entity;
    private Entity player2Entity;
    private SpriteMask player1HealthMask;
    private SpriteMask player2HealthMask;
    private EntityQuery playersQuery;

    protected override void OnCreate()
    {
        playersQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new[] { ComponentType.ReadOnly<TankPlayer>() }
        });
    }

    protected override void OnStartRunning()
    {
        // Retrieve tank player entities
        if (playersQuery.CalculateLength() == 0)
        {
            throw new InvalidOperationException("no player tanks detected");
        }
        var players = playersQuery.ToEntityArray(Allocator.TempJob);
        ComponentDataFromEntity<TankPlayer> tankPlayers = GetComponentDataFromEntity<TankPlayer>();
        for (int i = 0; i < players.Length; i++)
        {
            TankPlayer tankPlayer = tankPlayers[players[i]];
            if (tankPlayer.PlayerId == 0)
            {
                player1Entity = players[i];
            }
            else if(tankPlayer.PlayerId == 1)
            {
                player2Entity = players[i];
            }
        }
        players.Dispose();
        
        // Retrieve health masks
        Entities.WithAll<SpriteMask, HealthSlider>().ForEach((Entity e, SpriteMask spriteMask, ref HealthSlider healthSlider) => {
            if (healthSlider.PlayerId == 0)
                player1HealthMask = spriteMask;
            else
                player2HealthMask = spriteMask;
        });
    }

    protected override void OnUpdate()
    {
        player1HealthMask.alphaCutoff = GetPlayerHealthValue(EntityManager.GetComponentData<PlayerHealth>(player1Entity).Health);
        player2HealthMask.alphaCutoff = GetPlayerHealthValue(EntityManager.GetComponentData<PlayerHealth>(player2Entity).Health);
    }
    private float GetPlayerHealthValue(float playerHealth)
    {
        return (100 - playerHealth) / 10;
    }
}
