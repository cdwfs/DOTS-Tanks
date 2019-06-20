using DOTSInputs;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class GameManagerSystem : ComponentSystem
{
    private EntityQuery tankPlayerQuery;
    private ArchetypeChunkComponentType<TankPlayer> tankPlayerComponentType;
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
        
        CreateTanks();
    }

    protected override void OnUpdate()
    {
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
        EntityArchetype tankArchetype = World.EntityManager.CreateArchetype(typeof(TankPlayer), typeof(Translation), typeof(Rotation), typeof(PlayerInputState));
        
        Entity player1Entity = EntityManager.CreateEntity(tankArchetype);
        EntityManager.SetComponentData(player1Entity, new TankPlayer { PlayerId = 0 });
        
        Entity player2Entity = EntityManager.CreateEntity(tankArchetype);
        EntityManager.SetComponentData(player2Entity, new TankPlayer { PlayerId = 1 });


    }
}
