using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Random = UnityEngine.Random;

namespace ShipsNew.ECS
{
    [AlwaysSynchronizeSystem]
    public class SpawnShipsSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach(
                (Entity spawner, ref ShipsSpawnerData shipsSpawnerData, ref Translation translation, ref Rotation rotation) =>
                {
                    //var spawner = m_Group.Spawner[0];
                    //var sourceEntity = spawnShipsDataComponent.prefab;
                    //var center = transform.position;

                    for (int i = 0; i < shipsSpawnerData.Count; i++)
                    {
                        var newShip = EntityManager.Instantiate(shipsSpawnerData.ShipEntity);

                        var newPos = new float3(
                            translation.Value.x +Random.Range(-shipsSpawnerData.Horizontal / 2, shipsSpawnerData.Horizontal / 2),
                            translation.Value.y +Random.Range(-shipsSpawnerData.Vertical / 2, shipsSpawnerData.Vertical / 2),
                            translation.Value.z);
                        
                        //EntityManager.AddComponentData(newShip, new Target { Value = float3.zero });
                        EntityManager.SetComponentData(newShip, new Translation { Value = newPos });
                        EntityManager.SetComponentData(newShip, new MovableData { StartPosition = newPos, Speed = Random.Range(40, 50)});
                        EntityManager.SetComponentData(newShip, new Rotation {Value = rotation.Value});
                        EntityManager.SetComponentData(newShip, new PhysicsVelocity { Linear = float3.zero});
                        EntityManager.SetComponentData(newShip, new Weapon { Projectile = shipsSpawnerData.ProjectileEntity, ProjectileSpeed = 70, ProjectileLifetime = 4});
                    }

                    EntityManager.RemoveComponent<ShipsSpawnerData>(spawner);
                });
        }
    }
}
