using Unity.Entities;
using UnityEngine;

public class TankSpawner : MonoBehaviour
{
    public GameObject Player1Prefab;
    public GameObject Player2Prefab;

    public void Start()
    {
        Instantiate(Player1Prefab);
        Instantiate(Player2Prefab);
    }
}
