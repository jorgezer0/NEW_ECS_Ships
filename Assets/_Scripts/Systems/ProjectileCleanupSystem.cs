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
    public class ProjectileCleanupSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var deltaTime = Time.DeltaTime;
            
            Entities.WithStructuralChanges().ForEach((Entity entity, ref Projectile projectile) =>
            {
                if (projectile.Lifetime > 0)
                {
                    projectile.Lifetime -= deltaTime;
                    return;
                }
                
                EntityManager.DestroyEntity(entity);
            }).Run();

            return default;
        }
    }
}