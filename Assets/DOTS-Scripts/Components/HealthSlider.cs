using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
[Serializable]
public struct HealthSlider : IComponentData
{
    public int PlayerId;
}
