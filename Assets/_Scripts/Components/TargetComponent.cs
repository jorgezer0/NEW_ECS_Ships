using System;
using Unity.Entities;
using Unity.Mathematics;

namespace ShipsNew.ECS
{
    [GenerateAuthoringComponent]
    public struct Target : IComponentData
    {
        public float3 Value;
        public bool isTarget;
        public bool isHit;
    }
}