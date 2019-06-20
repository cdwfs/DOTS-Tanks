using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Authoring;
using UnityEngine;
using Unity.Mathematics;

// IConvertGameObjectToEntity pipeline is called *before* the Physics Body & Shape Conversion Systems
// This means that there would be no PhysicsMass component to tweak when Convert is called.
// Instead Convert is called from the PhysicsSamplesConversionSystem instead.
public class SetInertiaInverseBehaviour : MonoBehaviour/*, IConvertGameObjectToEntity*/
{
    public bool LockX = false;
    public bool LockY = false;
    public bool LockZ = false;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        if (dstManager.HasComponent<PhysicsMass>(entity))
        {
            var mass = dstManager.GetComponentData<PhysicsMass>(entity);
            var axisLock = new float3(LockX ? 1 : 0, LockY ? 1 : 0, LockZ ? 1 : 0);
            //axisLock = math.rotate(math.inverse(mass.InertiaOrientation), axisLock);
            mass.InverseInertia[0] = axisLock[0] == 1 ? 0 : mass.InverseInertia[0];
            mass.InverseInertia[1] = axisLock[1] == 1 ? 0 : mass.InverseInertia[1];
            mass.InverseInertia[2] = axisLock[2] == 1 ? 0 : mass.InverseInertia[2];
            dstManager.SetComponentData<PhysicsMass>(entity, mass);
        }
    }
}

