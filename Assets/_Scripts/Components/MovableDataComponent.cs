using Unity.Entities;
using Unity.Mathematics;

namespace ShipsNew.ECS
{
    [GenerateAuthoringComponent]
    public struct MovableData : IComponentData
    {
        public float3 StartPosition;
        public int Speed;
    }
}