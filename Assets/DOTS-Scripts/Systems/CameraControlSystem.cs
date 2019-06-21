using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CameraControlSystem : ComponentSystem
{
    private Camera camera;
    private CameraControlComponent cameraControl;
    private Entity player1Entity;
    private Entity player2Entity;
    private EntityQuery playersQuery;

    protected override void OnCreate()
    {
        playersQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new[] { ComponentType.ReadOnly<TankPlayer>() },
        });
    }

    protected override void OnStartRunning()
    {
        // Retrieve Scene camera & associated Entity
        camera = Camera.main;
        Entities.ForEach((Entity e, ref CameraControlComponent control) =>
        {
            cameraControl = control;
        });
        
        // Retrieve & cache player Tank entities
        if (playersQuery.CalculateLength() == 0)
        {
            throw new InvalidOperationException("No tank player entities detected");
        }
        var players = playersQuery.ToEntityArray(Allocator.TempJob);
        {
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
    }

    protected override void OnUpdate()
    {
        float3 player1pos = EntityManager.GetComponentData<Translation>(player1Entity).Value;
        float3 player2pos = EntityManager.GetComponentData<Translation>(player2Entity).Value;

        var averagePos = (player1pos + player2pos) / 2;
        averagePos.y = camera.transform.position.y;
        Move(averagePos);

        var size = math.length(player2pos - player1pos) / 2;
        size += cameraControl.ScreenEdgeBuffer;
        size = math.max(size, cameraControl.MinSize);
        Zoom(size);
    }

    private void Move(float3 averagePosition)
    {
        var cameraPos = (float3) camera.transform.position;
        cameraPos = math.lerp(cameraPos, averagePosition, cameraControl.DampTime);
        camera.transform.position = (Vector3)cameraPos;
    }

    private void Zoom(float cameraSize)
    {
        cameraControl.CameraSize = math.lerp(cameraControl.CameraSize, cameraSize, cameraControl.ZoomSpeed);
        camera.orthographicSize = cameraControl.CameraSize;
    }
}
