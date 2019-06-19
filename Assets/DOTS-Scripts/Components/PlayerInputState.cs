using Unity.Entities;
using Unity.Mathematics;

namespace DOTSInputs
{
    public struct PlayerInputState : IComponentData
    {
        public float Firing;
        public float2 Move;
    }
}