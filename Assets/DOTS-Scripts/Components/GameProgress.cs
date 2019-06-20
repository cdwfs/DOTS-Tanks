using System;
using Unity.Entities;

[Serializable]
public struct GameProgress : IComponentData
{
    public int CurrentRound;
}
