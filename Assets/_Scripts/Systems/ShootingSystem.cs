using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShipsNew.ECS
{
    public class ShootingSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var deltaTime = Time.DeltaTime;
            
            Entities.WithStructuralChanges().ForEach((Entity entity, ref Weapon weapon, ref Translation translation, ref Rotation rotation,
                ref LocalToWorld localToWorld, ref Target target) =>
            {
                if (weapon.ShootCooldown > 0)
                {
                    weapon.ShootCooldown -= deltaTime;
                    return;
                }
                
                var newProjectile = EntityManager.Instantiate(weapon.Projectile);
                EntityManager.SetComponentData(newProjectile, new Translation { Value = translation.Value + weapon.ShootPosition });
                EntityManager.SetComponentData(newProjectile, new Rotation { Value = rotation.Value });
                EntityManager.SetComponentData(newProjectile, new PhysicsVelocity { Linear = localToWorld.Forward * weapon.ProjectileSpeed});
                EntityManager.SetComponentData(newProjectile, new Projectile {Lifetime = weapon.ProjectileLifetime});
                weapon.ShootCooldown = Random.Range(4f, 5f);

            }).Run();

            return default;
        }
    }
}