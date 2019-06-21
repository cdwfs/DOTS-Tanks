using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class AerialAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public GameObject Node0;
    public GameObject Node1;
    public GameObject Node2;
    public GameObject Node3;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var entity0 = conversionSystem.GetPrimaryEntity(Node0);
        var entity1 = conversionSystem.GetPrimaryEntity(Node1);
        var entity2 = conversionSystem.GetPrimaryEntity(Node2);
        var entity3 = conversionSystem.GetPrimaryEntity(Node3);

        // Reparent the entities into a hierarchy
        {
            var p = dstManager.GetComponentData<Parent>(entity1);
            p.Value = entity0;
            dstManager.SetComponentData(entity1, p);
            dstManager.SetComponentData(entity1, new Translation { Value = Node0.transform.InverseTransformPoint(Node1.transform.position) });
            dstManager.SetComponentData(entity1, new Rotation { Value = Quaternion.Inverse(Node0.transform.rotation) * Node1.transform.rotation });
        }
        {
            var p = dstManager.GetComponentData<Parent>(entity2);
            p.Value = entity1;
            dstManager.SetComponentData(entity2, p);
            dstManager.SetComponentData(entity2, new Translation { Value = Node1.transform.InverseTransformPoint(Node2.transform.position) });
            dstManager.SetComponentData(entity2, new Rotation { Value = Quaternion.Inverse(Node1.transform.rotation) * Node2.transform.rotation });
        }
        {
            var p = dstManager.GetComponentData<Parent>(entity3);
            p.Value = entity2;
            dstManager.SetComponentData(entity3, p);
            dstManager.SetComponentData(entity3, new Translation { Value = Node2.transform.InverseTransformPoint(Node3.transform.position) });
            dstManager.SetComponentData(entity3, new Rotation { Value = Quaternion.Inverse(Node2.transform.rotation) * Node3.transform.rotation });
        }

        // Add aerial component
        var data = new Aerial()
        {
            Entity0 = entity0,
            Entity1 = entity1,
            ReferenceRotation1 = dstManager.GetComponentData<Rotation>(entity1).Value,
            Entity2 = entity2,
            ReferenceRotation2 = dstManager.GetComponentData<Rotation>(entity2).Value
        };
        dstManager.AddComponentData(entity, data);
    }
}
