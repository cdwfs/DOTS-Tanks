using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Plugins.PlayerInput;

[AlwaysUpdateSystem] // TEMP
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class InputGatheringSystem : ComponentSystem, TanksControls.IInGameActions
{
    // TODO - TankControls should be stored in its own singleton somewhere, and just used by systems, rather than owned here.
    public TanksControls TanksControls
    {
        get { return tankControls;  }
    }
    
    private TanksControls tankControls;
 
    private Entity player1Entity;
    private Entity player2Entity;

    public EntityQuery playersQuery;

    protected override void OnCreate()
    {
        // Create input
        tankControls = new TanksControls();
        tankControls.InGame.SetCallbacks(this);
        
        // Query
        World.GetOrCreateSystem<GameManagerSystem>();
        playersQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new[] { ComponentType.ReadOnly<TankPlayer>() }
        });
    }
    
    protected override void OnUpdate()
    {
        if (player1Entity == Entity.Null || player2Entity == Entity.Null)
        {
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
                    else if(tankPlayer.PlayerId == 1)
                    {
                        player2Entity = players[i];
                    }
                }
                players.Dispose();
                tankControls.InGame.Enable();
            }

            if (playerCount == 0)
            {
                return; // no players spawned yet
            }
        }
    }

    public void OnPlayer1Move(InputAction.CallbackContext context)
    {        
        Entity player = player1Entity;
        PlayerInputState playerInputState = EntityManager.GetComponentData<PlayerInputState>(player);
        playerInputState.Move = context.ReadValue<Vector2>();
        EntityManager.SetComponentData(player, playerInputState);        
    }

    public void OnPlayer2Move(InputAction.CallbackContext context)
    {
        Entity player = player2Entity;
        PlayerInputState playerInputState = EntityManager.GetComponentData<PlayerInputState>(player);
        playerInputState.Move = context.ReadValue<Vector2>();
        EntityManager.SetComponentData(player, playerInputState); 
    }

    public void OnPlayer1Shoot(InputAction.CallbackContext context)
    {
        Entity player = player1Entity;
        PlayerInputState playerInputState = EntityManager.GetComponentData<PlayerInputState>(player);
        playerInputState.Firing = context.ReadValue<float>();
        EntityManager.SetComponentData(player, playerInputState);
    }

    public void OnPlayer2Shoot(InputAction.CallbackContext context)
    {
        Entity player = player2Entity;
        PlayerInputState playerInputState = EntityManager.GetComponentData<PlayerInputState>(player);
        playerInputState.Firing = context.ReadValue<float>();
        EntityManager.SetComponentData(player, playerInputState);
    }
}
