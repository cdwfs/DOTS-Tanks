using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Plugins.PlayerInput;

[AlwaysUpdateSystem]
public class InputGatheringSystem : ComponentSystem, TanksControls.IInGameActions
{
    // TODO - TankControls should be stored in its own singleton somewhere, and just used by systems, rather than owned here.
    public TanksControls TanksControls
    {
        get { return tankControls;  }
    }
    
    private TanksControls tankControls;
 
    private EntityQuery playersQuery;
    
    private Entity player1Entity;
    private Entity player2Entity;

    protected override void OnCreate()
    {
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
            if (players.Length == 0)
            {
                players.Dispose();
                return; // players haven't been spawned yet, nothing to do
            }
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
        
        
//        PlayerInputState playerInputState = EntityManager.GetComponentData<PlayerInputState>(player1Entity);
//        if (playerInputState.Firing > 0f)
//        {
//            Debug.Log("Player Firing");
//        }
//
//        if (math.lengthsq(playerInputState.Move) > 0f)
//        {
//            Debug.Log("Player moving");
//        }
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
