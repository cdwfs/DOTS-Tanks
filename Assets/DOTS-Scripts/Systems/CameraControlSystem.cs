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

    protected override void OnUpdate()
    {
        if (camera == null)
            GetSceneCamera();

        if (player1Entity == Entity.Null || player2Entity == Entity.Null)
            GetPlayersEntities();

        if (camera == null || player1Entity == Entity.Null || player2Entity == Entity.Null)
            return;

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
    private void GetSceneCamera()
    {
        camera = Camera.main;
        Entities.ForEach((Entity e, ref CameraControlComponent control) =>
        {
            cameraControl = control;
        });
    }
    private void GetPlayersEntities()
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
        }
    }
    private void Move(float3 averagePosition)
    {
        var camerapos = (float3) camera.transform.position;
        camerapos = math.lerp(camerapos, averagePosition, cameraControl.DampTime);
        camera.transform.position = (Vector3)camerapos;
    }

    private void Zoom(float cameraSize)
    {
        cameraControl.CameraSize = math.lerp(cameraControl.CameraSize, cameraSize, cameraControl.ZoomSpeed);
        camera.orthographicSize = cameraControl.CameraSize;
    }
}
