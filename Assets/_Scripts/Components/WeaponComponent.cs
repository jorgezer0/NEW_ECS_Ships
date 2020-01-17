using Unity.Entities;
using Unity.Mathematics;

namespace ShipsNew.ECS
{
    [GenerateAuthoringComponent]
    public struct Weapon : IComponentData
    {
        public Entity Projectile;
        public float3 ShootPosition;
        public float ShootCooldown;
        public float ProjectileSpeed;
        public float ProjectileLifetime;
    }
}