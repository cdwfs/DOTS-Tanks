using System;
using Unity.Entities;

[Serializable]
public struct ShellDuration : IComponentData
{
    public float SecondsToLive;
}
