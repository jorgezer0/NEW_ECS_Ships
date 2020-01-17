using System.Collections;
using System.Collections.Generic;
using Ships.ECS;
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
    [UpdateBefore(typeof(FollowSystem))]
    public class TargetingSystem : JobComponentSystem
    {
        private EntityQuery teamAShips;
        private NativeArray<Entity> teamAEnities;
        private EntityQuery teamBShips;
        private NativeArray<Entity> teamBEnities;
        
        protected override void OnCreate()
        {
            teamAShips = GetEntityQuery(typeof(TeamA), ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<PhysicsVelocity>());
            teamBShips = GetEntityQuery(typeof(TeamB), ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<PhysicsVelocity>());
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var enemyBPosArray = teamBShips.ToComponentDataArray<Translation>(Allocator.TempJob);
            JobHandle TeamAJob = Entities.ForEach((Entity entity, ref Target target, in Translation translation, in Rotation rotation,  in TeamA teamA, in LocalToWorld localToWorld, in PhysicsVelocity physicsVelocity, in Weapon weapon) =>
            {
                var lastDot = 0f;
                for (int j = 0; j < enemyBPosArray.Length; j++)
                {
                    if (lastDot > 0.999) break;

                    var tempTarget = enemyBPosArray[j].Value;
                    var dot = math.dot(localToWorld.Forward, math.normalize(tempTarget - translation.Value));
                    
                    if (dot > lastDot)
                    {
                        target.Value = tempTarget;
                        lastDot = dot;
                    }
                }

                /*var dir = target.Value - translation.Value;

                var targetRelativeSpeed = targetSpeed - physicsVelocity.Linear;

                var a = math.dot(targetRelativeSpeed, targetRelativeSpeed) - (weapon.ProjectileSpeed * weapon.ProjectileSpeed);
                var b = 2 * math.dot(targetRelativeSpeed, dir);
                var c = math.dot(dir, dir);

                var D = math.sqrt(math.abs((b * b) - 4 * a * c));

                var t = -(b + D) / (2 * a);

                var hit = target.Value + targetRelativeSpeed * t;

                target.Value = hit;*/

            }).Schedule(inputDeps);

            var enemyAPosArray = teamAShips.ToComponentDataArray<Translation>(Allocator.TempJob);
            JobHandle TeamBJob = Entities.ForEach((Entity entity, ref Target target, in Translation translation, in Rotation rotation,  in TeamB teamB, in LocalToWorld localToWorld) =>
            {
                var lastDot = 0f;
                for (int j = 0; j < enemyAPosArray.Length; j++)
                {
                    if (lastDot > 0.999) break;

                    var tempTarget = enemyAPosArray[j].Value;
                    var dot = math.dot(localToWorld.Forward, math.normalize(tempTarget - translation.Value));
                    
                    if (dot > lastDot)
                    {
                        target.Value = tempTarget;
                        lastDot = dot;
                    }
                }
            }).Schedule(TeamAJob);
            
            TeamBJob.Complete();
            
            enemyAPosArray.Dispose();
            enemyBPosArray.Dispose();

            return TeamBJob;
        }
    }

}