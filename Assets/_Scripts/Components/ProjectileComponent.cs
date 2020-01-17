using Unity.Entities;

namespace ShipsNew.ECS
{
    [GenerateAuthoringComponent]
    public struct Projectile : IComponentData
    {
        public float Lifetime;
    }
}