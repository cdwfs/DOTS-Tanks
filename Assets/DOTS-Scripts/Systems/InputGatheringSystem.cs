using System.Collections;
using System.Collections.Generic;
using DOTSInputs;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Plugins.PlayerInput;

[AlwaysUpdateSystem]
public class InputGatheringSystem : ComponentSystem, TanksControls.IInGameActions
{
    private TanksControls tankControls;
 
    private EntityQuery playersQuery;

    
    
    private EntityArchetype inputEntityArchetype;
    // TODO - use the player array from the GameManagerSystem
    private Entity player1Entity;
    private Entity player2Entity;

    protected override void OnCreate()
    {
        // Create objects to hold the input
        // TODO - this is temp - should use the player array from GameManagerSystem
//        inputEntityArchetype = World.EntityManager.CreateArchetype(typeof(PlayerInputState));
//        player1Entity = EntityManager.CreateEntity(inputEntityArchetype);
//        player2Entity = EntityManager.CreateEntity(inputEntityArchetype);

        // Create input
        tankControls = new TanksControls();
        tankControls.InGame.SetCallbacks(this);
        
        // Query
        World.GetOrCreateSystem<GameManagerSystem>();
        EntityQueryDesc desc = new EntityQueryDesc
        {
            All = new ComponentType[]
            {
                ComponentType.ReadOnly<TankPlayer>()
            }
        };

        playersQuery = GetEntityQuery(desc);
    }
    
    protected override void OnUpdate()
    {
        if (player1Entity == Entity.Null || player2Entity == Entity.Null)
        {
            NativeArray<Entity> players = playersQuery.ToEntityArray(Allocator.TempJob);
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
        
        
        PlayerInputState playerInputState = EntityManager.GetComponentData<PlayerInputState>(player1Entity);
        if (playerInputState.Firing > 0f)
        {
            Debug.Log("Player Firing");
        }

        if (math.lengthsq(playerInputState.Move) > 0f)
        {
            Debug.Log("Player moving");
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

    public void OnNewaction(InputAction.CallbackContext context)
    {
        //
    }
}
