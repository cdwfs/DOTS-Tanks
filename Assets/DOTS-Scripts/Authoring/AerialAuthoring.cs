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
            dstManager.GetComponentData<Parent>(entity1);
        }
        {
            var p = dstManager.GetComponentData<Parent>(entity2);
            p.Value = entity1;
            dstManager.GetComponentData<Parent>(entity2);
        }
        {
            var p = dstManager.GetComponentData<Parent>(entity3);
            p.Value = entity2;
            dstManager.GetComponentData<Parent>(entity3);
        }

        // Add aerial component
        var data = new Aerial()
        {
            Entity0 = entity0,
            Entity1 = entity1,
            Entity2 = entity2
        };
        dstManager.AddComponentData(entity, data);
    }
}
