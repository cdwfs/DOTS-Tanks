using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CameraControlSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Move();
        Zoom();
    }

    private void Move()
    {
        Entities.WithAll<Transform, CameraControlComponent, Translation>().ForEach((Entity e, Transform t, ref CameraControlComponent control,ref Translation translation) => 
        {
            translation.Value = math.lerp(translation.Value, FindAveragePosition(translation.Value), control.DampTime);
            t.position = (Vector3)translation.Value;
        });
    }

    private void Zoom()
    {
        Entities.WithAll<Camera,Translation, CameraControlComponent >().ForEach((Entity e, Camera cam, ref Translation translation, ref CameraControlComponent control) =>
        {
            control.CameraSize = math.lerp(control.CameraSize, FindRequiredSize(translation.Value), control.ZoomSpeed);
            cam.orthographicSize = control.CameraSize;
        });
    }
    private float3 FindAveragePosition(float3 CameraPosition)
    {
        float3 averagePos = new float3();
        int numTargets = 0;

        Entities.WithAll<CameraTarget, Translation>().ForEach((Entity e, ref CameraTarget target, ref Translation translation) =>
        {
            averagePos += translation.Value;
            numTargets++;
        });
     
        if (numTargets > 0)
            averagePos /= numTargets;
        averagePos.y = CameraPosition.y;
        return averagePos;
    }

    private float FindRequiredSize(float3 CameraPosition)
    {
        float size = 0f;
        CameraControlComponent cameraControl = new CameraControlComponent();
        Entities.ForEach((Entity e, ref CameraControlComponent control) =>
        {
            cameraControl = control;
        });
        Entities.WithAll<CameraTarget, Translation>().ForEach((Entity e, ref CameraTarget target, ref Translation targetTranslation) =>
        {
            size = math.max(size, math.abs(targetTranslation.Value.y));
            size = math.max(size, math.abs(targetTranslation.Value.x)/cameraControl.CameraAspect);

        });
        size += cameraControl.ScreenEdgeBuffer;
        size += math.max(size,cameraControl.MinSize);

        return size;
    }
}
