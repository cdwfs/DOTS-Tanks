using System;
using Unity.Entities;

[Serializable]
public struct GameSettings : IComponentData
{
    public int ScoreToWin;
}
