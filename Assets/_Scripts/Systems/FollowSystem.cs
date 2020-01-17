using ShipsNew.ECS;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using Unity.Physics;
using UnityEngine;

namespace Ships.ECS
{
    
    [UpdateAfter(typeof(TargetingSystem))]
    public class FollowSystem : JobComponentSystem
    {
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var deltaTime = Time.DeltaTime;

            JobHandle jobHandle = Entities.ForEach((Entity entity, ref Translation translation, ref Rotation rotation,
                ref PhysicsVelocity physicsVelocity, in MovableData movableData, in Target target, in LocalToWorld localToWorld) =>
            {
                var pos = translation.Value;
                var rot = rotation.Value;
                
                var dir = math.normalize(target.Value - pos);
                var look = Quaternion.LookRotation(dir);
                var newRot = Quaternion.Slerp(rot, look, deltaTime);

                rotation.Value = newRot;
                physicsVelocity.Linear = localToWorld.Forward * movableData.Speed;

            }).Schedule(inputDeps);

            return jobHandle;
        }
    }
}