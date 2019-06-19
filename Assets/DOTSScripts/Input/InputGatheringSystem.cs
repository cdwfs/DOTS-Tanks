using System.Collections;
using System.Collections.Generic;
using DOTSInputs;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Plugins.PlayerInput;

public class InputGatheringSystem : ComponentSystem, TanksControls.IInGameActions
{
    InputAction player1Shoot;
    InputAction player1Move;

    private TanksControls tankControls;
    
    private EntityArchetype inputEntityArchetype;
    // TODO - use the player array from the GameManagerSystem
    private Entity playerEntityA;
    private Entity playerEntityB;

    protected override void OnCreate()
    {
        base.OnCreate();
        
        // Create objects to hold the input
        // TODO - this is temp - should use the player array from GameManagerSystem
        inputEntityArchetype = World.EntityManager.CreateArchetype(typeof(PlayerInputState));
        playerEntityA = EntityManager.CreateEntity(inputEntityArchetype);
        playerEntityB = EntityManager.CreateEntity(inputEntityArchetype);

        // Create input
        tankControls = new TanksControls();
        tankControls.InGame.SetCallbacks(this);
        tankControls.InGame.Enable();
    }
    
    protected override void OnUpdate()
    {
        PlayerInputState playerInputState = EntityManager.GetComponentData<PlayerInputState>(playerEntityB);
        if (playerInputState.Firing > 0f)
        {
            Debug.Log("Player 1 Firing");
        }

        if (math.lengthsq(playerInputState.Move) > 0f)
        {
            Debug.Log("Player 1 moving");
        }
    }

    public void OnPlayer1Move(InputAction.CallbackContext context)
    {        
        Entity player = playerEntityA;
        PlayerInputState playerInputState = EntityManager.GetComponentData<PlayerInputState>(player);
        playerInputState.Move = context.ReadValue<Vector2>();
        EntityManager.SetComponentData(player, playerInputState);        
    }

    public void OnPlayer2Move(InputAction.CallbackContext context)
    {
        Entity player = playerEntityB;
        PlayerInputState playerInputState = EntityManager.GetComponentData<PlayerInputState>(player);
        playerInputState.Move = context.ReadValue<Vector2>();
        EntityManager.SetComponentData(player, playerInputState); 
    }

    public void OnPlayer1Shoot(InputAction.CallbackContext context)
    {
        Entity player = playerEntityA;
        PlayerInputState playerInputState = EntityManager.GetComponentData<PlayerInputState>(player);
        playerInputState.Firing = context.ReadValue<float>();
        EntityManager.SetComponentData(player, playerInputState);
    }

    public void OnPlayer2Shoot(InputAction.CallbackContext context)
    {
        Entity player = playerEntityB;
        PlayerInputState playerInputState = EntityManager.GetComponentData<PlayerInputState>(player);
        playerInputState.Firing = context.ReadValue<float>();
        EntityManager.SetComponentData(player, playerInputState);
    }

    public void OnNewaction(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}
