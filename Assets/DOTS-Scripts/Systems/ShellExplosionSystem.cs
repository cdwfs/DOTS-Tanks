using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Burst;
using Unity.Transforms;
using UnityEngine;

// This system applies an impulse to any dynamic that collides with a Repulsor.
// A Repulsor is defined by a PhysicsShape with the `Raise Collision Events` flag ticked and a
// CollisionEventImpulse behaviour added.
[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class ShellExplosionSystem : JobComponentSystem
{
    BuildPhysicsWorld m_BuildPhysicsWorldSystem;
    StepPhysicsWorld m_StepPhysicsWorldSystem;

    EntityQuery ImpulseGroup;

    public NativeList<float3> ExplodedShellPositions;
    public JobHandle ExplodedShellJobHandle;


    private Entity ShellExplosionPrefab = Entity.Null;

    protected override void OnCreate()
    {
        m_BuildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
        m_StepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
        
        ImpulseGroup = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(ShellStats), }
        });

        ExplodedShellPositions = new NativeList<float3>(8, Allocator.Persistent);
        ExplodedShellJobHandle = new JobHandle();
    }

    protected override void OnDestroy()
    {
        ExplodedShellPositions.Dispose();

        base.OnDestroy();
    }

    //[BurstCompile] -- something in here isn't burst-compatible
    struct ShellExplosionJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<ShellStats> ShellStatsGroup;
        public ComponentDataFromEntity<ShellDuration> ShellDurationGroup;
        public ComponentDataFromEntity<PhysicsVelocity> PhysicsVelocityGroup;
        [ReadOnly] public ComponentDataFromEntity<PhysicsMass> PhysicsMassGroup;
        [ReadOnly] public ComponentDataFromEntity<Translation> TranslationGroup;
        [ReadOnly] public ComponentDataFromEntity<Rotation> RotationGroup;
        public ComponentDataFromEntity<PlayerHealth> HealthGroup;

        public NativeList<float3> ExplodedShellPosition;

        public PhysicsWorld PhysicsWorld;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.Entities.EntityA;
            Entity entityB = collisionEvent.Entities.EntityB;

            bool isBodyAShell = ShellStatsGroup.Exists(entityA);
            bool isBodyBShell = ShellStatsGroup.Exists(entityB);
           
            if (isBodyAShell)
            {
                var shellPosition = TranslationGroup[entityA].Value;
                var shellStats = ShellStatsGroup[entityA];
                Systems.ExplosionSystem.MakeExplosion(ref PhysicsWorld,
                    TranslationGroup, RotationGroup,
                    PhysicsVelocityGroup, PhysicsMassGroup,
                    HealthGroup,
                    TranslationGroup[entityA].Value, shellStats.ExplosionRadius, shellStats.ExplosionForce, shellStats.MaxDamage);

                var shellDuration = ShellDurationGroup[entityA];
                shellDuration.SecondsToLive = 0;
                ShellDurationGroup[entityA] = shellDuration;

                ExplodedShellPosition.Add(shellPosition);
            }
            if (isBodyBShell)
            {
                var shellPosition = TranslationGroup[entityB].Value;
                var shellStats = ShellStatsGroup[entityB];
                Systems.ExplosionSystem.MakeExplosion(ref PhysicsWorld,
                    TranslationGroup, RotationGroup, 
                    PhysicsVelocityGroup, PhysicsMassGroup,
                    HealthGroup,
                    TranslationGroup[entityB].Value, shellStats.ExplosionRadius, shellStats.ExplosionForce, shellStats.MaxDamage);

                var shellDuration = ShellDurationGroup[entityB];
                shellDuration.SecondsToLive = 0;
                ShellDurationGroup[entityB] = shellDuration;

                ExplodedShellPosition.Add(shellPosition);
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        ExplodedShellJobHandle = new ShellExplosionJob
        {
            ShellStatsGroup = GetComponentDataFromEntity<ShellStats>(true),
            ShellDurationGroup = GetComponentDataFromEntity<ShellDuration>(),
            TranslationGroup = GetComponentDataFromEntity<Translation>(true),
            RotationGroup = GetComponentDataFromEntity<Rotation>(true),
            PhysicsVelocityGroup = GetComponentDataFromEntity<PhysicsVelocity>(),
            PhysicsMassGroup = GetComponentDataFromEntity<PhysicsMass>(true),
            HealthGroup = GetComponentDataFromEntity<PlayerHealth>(),
            PhysicsWorld = m_BuildPhysicsWorldSystem.PhysicsWorld,
            ExplodedShellPosition = ExplodedShellPositions,
        }.Schedule(m_StepPhysicsWorldSystem.Simulation,
                    ref m_BuildPhysicsWorldSystem.PhysicsWorld, inputDeps);

        return ExplodedShellJobHandle;
    }
}

[UpdateBefore(typeof(ShellExplosionSystem))]
public class SpawnShellExplosionSystem : ComponentSystem
{
    ShellExplosionSystem m_ShellExplosionSystem;
    private GameObject ShellExplosionPrefab = null;
    private ParticleSystem ShellExplosionParticleSystem = null;

    protected override void OnCreate()
    {
        base.OnCreate();
        m_ShellExplosionSystem = World.GetOrCreateSystem<ShellExplosionSystem>();
    }

    protected override void OnStartRunning()
    {
        base.OnStartRunning();

        ShellExplosionPrefab = GameObject.Find("ShellExplosion");
        if (ShellExplosionPrefab != null)
        {
            ShellExplosionParticleSystem = ShellExplosionPrefab.GetComponentsInChildren<ParticleSystem>()[0];
        }
    }

    protected override void OnUpdate()
    {
        if (ShellExplosionPrefab)
        {
            m_ShellExplosionSystem.ExplodedShellJobHandle.Complete();
            for (int i = 0; i < m_ShellExplosionSystem.ExplodedShellPositions.Length; i++)
            {
                var position = m_ShellExplosionSystem.ExplodedShellPositions[i];

                var newGO = GameObject.Instantiate(ShellExplosionPrefab, position, quaternion.Euler(-90,0,0));
                newGO.GetComponent<ParticleSystem>().Play();
            }
            m_ShellExplosionSystem.ExplodedShellPositions.Clear();
        }
    }
}