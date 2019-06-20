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

    protected override void OnUpdate()
    {
        if (player1Entity == Entity.Null || player2Entity == Entity.Null)
        {
            var playersQuery = EntityManager.CreateEntityQuery(new EntityQueryDesc
            {
                All = new[] { ComponentType.ReadOnly<TankPlayer>() },
            });
            int playerCount = playersQuery.CalculateLength();
            if (playerCount > 0)
            {
                var players = playersQuery.ToEntityArray(Allocator.TempJob);
                ComponentDataFromEntity<TankPlayer> tankPlayers = GetComponentDataFromEntity<TankPlayer>();
                for (int i = 0; i < players.Length; i++)
                {
                    TankPlayer tankPlayer = tankPlayers[players[i]];
                    if (tankPlayer.PlayerId == 0)
                    {
                        player1Entity = players[i];
                    }
                    else if (tankPlayer.PlayerId == 1)
                    {
                        player2Entity = players[i];
                    }
                }
                players.Dispose();
            }
            playersQuery.Dispose();
            if (playerCount == 0)
            {
                return; // no players spawned yet
            }
        }
        Entities.WithAll<SpriteMask, HealthSlider>().ForEach((Entity e, SpriteMask spriteMask, ref HealthSlider healthSlider) => {
            float playerHealth;
            if (healthSlider.PlayerId == 0)
                playerHealth = EntityManager.GetComponentData<PlayerHealth>(player1Entity).Health;
            else
                playerHealth = EntityManager.GetComponentData<PlayerHealth>(player2Entity).Health;
            spriteMask.alphaCutoff = (float)(100 - playerHealth) / 10;
        });
    }
}
