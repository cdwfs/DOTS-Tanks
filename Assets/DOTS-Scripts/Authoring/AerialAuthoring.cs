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
        var rotations = conversionSystem.GetComponentDataFromEntity<Rotation>();

        var data = new Aerial()
        {
            Entity0 = conversionSystem.GetPrimaryEntity(Node0),
            ReferenceRotation0 = Node0.transform.rotation,
            Entity1 = conversionSystem.GetPrimaryEntity(Node1),
            ReferenceRotation1 = Node1.transform.rotation,
            Entity2 = conversionSystem.GetPrimaryEntity(Node2),
            ReferenceRotation2 = Node2.transform.rotation,
            Entity3 = conversionSystem.GetPrimaryEntity(Node3),
            ReferenceRotation3 = Node3.transform.rotation,
            WobbleMagnitude = 0,
            WobbleTime = 0
        };

        dstManager.AddComponentData(entity, data);
    }
}
