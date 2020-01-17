using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace ShipsNew.ECS
{
    [GenerateAuthoringComponent]
    public struct ShipsSpawnerData : IComponentData
    {
        public Entity ShipEntity;
        public Entity ProjectileEntity;
        public float Horizontal;
        public float Vertical;
        public int Count;
    }

    public class ShipsSpawnerConverter : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public GameObject ShipPrefab;
        public GameObject ProjectilePrefab;
        public float Horizontal;
        public float Vertical;
        public int Count;

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(ShipPrefab);
            referencedPrefabs.Add(ProjectilePrefab);
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var shipPrefabEntity = conversionSystem.GetPrimaryEntity((ShipPrefab));
            var projectilePrefabentity = conversionSystem.GetPrimaryEntity(ProjectilePrefab);

            var shipSpawnerData = new ShipsSpawnerData
            {
                ShipEntity = shipPrefabEntity,
                ProjectileEntity = projectilePrefabentity,
                Horizontal = Horizontal,
                Vertical = Vertical,
                Count = Count
            };

            dstManager.AddComponentData(entity, shipSpawnerData);
        }
    }
    
}