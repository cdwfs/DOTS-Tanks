﻿using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct TankAttackStats : IComponentData
{
    public float3 ShellSpawnPositionOffset;
    public quaternion ShellSpawnRotationOffset;
    public float MuzzleVelocity;
}
