using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[AlwaysUpdateSystem] // TODO: the query here assumes players already exist, but this is always the system that spawns the players.
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class GameManagerSystem : ComponentSystem
{
    private EntityQuery tankPlayerQuery;
    private ArchetypeChunkComponentType<TankPlayer> tankPlayerComponentType;
    private Entity TankPrefab = Entity.Null;
    protected override void OnCreate()
    {
        tankPlayerQuery = GetEntityQuery(ComponentType.ReadWrite<TankPlayer>());
        tankPlayerComponentType = GetArchetypeChunkComponentType<TankPlayer>();
        
        // Create GameProgress singleton entity
        var progressEntity = EntityManager.CreateEntity();
        EntityManager.AddComponentData(progressEntity, new GameProgress
        {
            CurrentRound = 1,
        });
    }

    protected override void OnUpdate()
    {
        // TODO: his block should be moved to OnCreate() once I figure out how to ensure it runs after the GameObject conversion system.
        if (TankPrefab == Entity.Null)
        {
            var tankPrefabQuery = EntityManager.CreateEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<TankMovementStats>()},
                Options = EntityQueryOptions.IncludePrefab,
            });
            int count = tankPrefabQuery.CalculateLength();
            if (count == 0)
            {
                tankPrefabQuery.Dispose();
                return; // in case no match was found for the first few frames
            }
            if (count == 1)
            {
                var prefabEntities = tankPrefabQuery.ToEntityArray(Allocator.TempJob);
                TankPrefab = prefabEntities[0];
                prefabEntities.Dispose();
            }
            tankPrefabQuery.Dispose();
            
            // Now that we have the prefab, spawn the initial set of tanks
            CreateTanks();
        }

        bool newGame = false;
        bool newRound = false;
        var tankPlayerChunks = tankPlayerQuery.CreateArchetypeChunkArray(Allocator.TempJob);
        if (tankPlayerChunks.Length == 1)
        {
            if (tankPlayerChunks[0].Count == 1)
            {
                // Exactly one tank left alive. Its owner wins this round.
                var chunkPlayers = tankPlayerChunks[0].GetNativeArray(tankPlayerComponentType);
                var winningPlayer = chunkPlayers[0];
                winningPlayer.Score += 1;
                
                var gameSettings = GetSingleton<GameSettings>();
                if (winningPlayer.Score == gameSettings.ScoreToWin)
                {
                    // game over, this player wins
                    newGame = true;
                }
                else
                {
                    // round over
                    newRound = true;
                }
            }
        }
        else if (tankPlayerChunks.Length == 0)
        {
            // No tanks left alive. This round is a tie.
            newRound = true;
        }
        tankPlayerChunks.Dispose();

        if (newRound)
        {
            var gameProgress = GetSingleton<GameProgress>();
            gameProgress.CurrentRound += 1;
            SetSingleton(gameProgress);
        }
        else if (newGame)
        {
            var gameProgress = GetSingleton<GameProgress>();
            gameProgress.CurrentRound = 1;
            SetSingleton(gameProgress);

            CreateTanks();
        }
    }

    void CreateTanks()
    {
        // TODO - this archetype should be coming from a prefab
        Entity player1Entity = EntityManager.Instantiate(TankPrefab);
        EntityManager.SetComponentData(player1Entity, new TankPlayer { PlayerId = 0 });
        
        Entity player2Entity = EntityManager.Instantiate(TankPrefab);
        EntityManager.SetComponentData(player2Entity, new TankPlayer { PlayerId = 1 });


    }
}
