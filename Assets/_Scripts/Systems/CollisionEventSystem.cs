using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

namespace ShipsNew.ECS
{
    [UpdateAfter(typeof(EndFramePhysicsSystem))]
    public class CollisionEventSystem : JobComponentSystem
    {
        BuildPhysicsWorld m_BuildPhysicsWorldSystem;
        StepPhysicsWorld m_StepPhysicsWorldSystem;

        protected override void OnCreate()
        {
            m_BuildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
            m_StepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
        }

        [BurstCompile]
        struct CollisionEventJob : ITriggerEventsJob
        {
            //public NativeArray<Translation> TranslationGroup;
            public ComponentDataFromEntity<Translation> TranslationGroup;
            [ReadOnly] public ComponentDataFromEntity<MovableData> MovableDataGroup;
            [ReadOnly] public ComponentDataFromEntity<TeamA> TeamAGroup;
            [ReadOnly] public ComponentDataFromEntity<TeamB> TeamBGroup;
            [ReadOnly] public ComponentDataFromEntity<Projectile> ProjectileGroup;

            public void Execute(TriggerEvent collisionEvent)
            {
                Entity entityA = collisionEvent.Entities.EntityA;
                Entity entityB = collisionEvent.Entities.EntityB;

                bool isAShip = TeamAGroup.Exists(entityA) || TeamBGroup.Exists(entityA);
                bool isBShip = TeamAGroup.Exists(entityB) || TeamBGroup.Exists(entityB);

                bool isAProjectile = ProjectileGroup.Exists(entityA);
                bool isBProjectile = ProjectileGroup.Exists(entityB);

                if (isAShip && isBProjectile)
                {
                    var pos = TranslationGroup[entityA];
                    pos.Value = MovableDataGroup[entityA].StartPosition;
                    TranslationGroup[entityA] = pos;
                }
                else if (isBShip && isAProjectile)
                {
                    var pos = TranslationGroup[entityB];
                    pos.Value = MovableDataGroup[entityB].StartPosition;
                    TranslationGroup[entityB] = pos;
                } else if (isAShip && isBShip)
                {
                    var posA = TranslationGroup[entityA];
                    posA.Value = MovableDataGroup[entityA].StartPosition;
                    TranslationGroup[entityA] = posA;
                    var posB = TranslationGroup[entityB];
                    posB.Value = MovableDataGroup[entityB].StartPosition;
                    TranslationGroup[entityB] = posB;
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            JobHandle jobHandle = new CollisionEventJob
            {
                TranslationGroup = GetComponentDataFromEntity<Translation>(),
                MovableDataGroup = GetComponentDataFromEntity<MovableData>(),
                TeamAGroup = GetComponentDataFromEntity<TeamA>(),
                TeamBGroup = GetComponentDataFromEntity<TeamB>(),
                ProjectileGroup = GetComponentDataFromEntity<Projectile>()
            }.Schedule(m_StepPhysicsWorldSystem.Simulation, ref m_BuildPhysicsWorldSystem.PhysicsWorld, inputDeps);

            return jobHandle;
        }
    }
}
