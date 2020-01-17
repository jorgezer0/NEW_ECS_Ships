using ShipsNew.ECS;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using Unity.Physics;
using UnityEngine;
using Camera = ShipsNew.ECS;

namespace Ships.ECS
{
    
    [UpdateAfter(typeof(FollowSystem))]
    public class CameraFollowSystem : JobComponentSystem
    {
        private EntityQuery ships;
        
        protected override void OnCreate()
        {
            ships = GetEntityQuery(typeof(TeamA), ComponentType.ReadOnly<Translation>(),
                ComponentType.ReadOnly<Rotation>());
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var shipPos = ships.ToComponentDataArray<Translation>(Allocator.TempJob);
            var shipRot = ships.ToComponentDataArray<Rotation>(Allocator.TempJob);

            var deltaTime = Time.DeltaTime;
            
            JobHandle jobHandle = Entities.ForEach((Entity Entity, ref CameraComponent camera, ref Translation translation, ref Rotation rotation) =>
                {
                    translation.Value = new float3(
                        shipPos[0].Value.x ,
                        shipPos[0].Value.y + 1.35f,
                        shipPos[0].Value.z                       
                        );
                    //rotation.Value = Quaternion.Slerp(rotation.Value, shipRot[0].Value, deltaTime * 5);
                    rotation.Value = shipRot[0].Value;
                }).Schedule(inputDeps);

            jobHandle.Complete();
            
            shipPos.Dispose();
            shipRot.Dispose();
            
            return default;
        }
    }
}