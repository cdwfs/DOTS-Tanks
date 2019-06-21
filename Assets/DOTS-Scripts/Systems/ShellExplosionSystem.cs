using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Burst;
using Unity.Transforms;

// This system applies an impulse to any dynamic that collides with a Repulsor.
// A Repulsor is defined by a PhysicsShape with the `Raise Collision Events` flag ticked and a
// CollisionEventImpulse behaviour added.
[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class ShellExplosionSystem : JobComponentSystem
{
    BuildPhysicsWorld m_BuildPhysicsWorldSystem;
    StepPhysicsWorld m_StepPhysicsWorldSystem;
    EndSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    EntityQuery ImpulseGroup;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        m_BuildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
        m_StepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
        
        ImpulseGroup = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(ShellStats), }
        });
    }

    //[BurstCompile]
    struct ShellExplosionJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<ShellStats> ShellStatsGroup;
        public ComponentDataFromEntity<PhysicsVelocity> PhysicsVelocityGroup;
        [ReadOnly] public ComponentDataFromEntity<PhysicsMass> PhysicsMassGroup;
        [ReadOnly] public ComponentDataFromEntity<Translation> TranslationGroup;
        [ReadOnly] public ComponentDataFromEntity<Rotation> RotationGroup;
        public ComponentDataFromEntity<PlayerHealth> HealthGroup;

        public PhysicsWorld PhysicsWorld;
        public EntityCommandBuffer CommandBuffer;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.Entities.EntityA;
            Entity entityB = collisionEvent.Entities.EntityB;

            bool isBodyAShell = ShellStatsGroup.Exists(entityA);
            bool isBodyBShell = ShellStatsGroup.Exists(entityB);
           
            if (isBodyAShell)
            {
                var shellStats = ShellStatsGroup[entityA];
                Systems.ExplosionSystem.MakeExplosion(ref PhysicsWorld,
                    TranslationGroup, RotationGroup,
                    PhysicsVelocityGroup, PhysicsMassGroup,
                    HealthGroup,
                    TranslationGroup[entityA].Value, shellStats.ExplosionRadius, shellStats.ExplosionForce, shellStats.MaxDamage);

                CommandBuffer.DestroyEntity(entityA);

            }
            if (isBodyBShell)
            {
                var shellStats = ShellStatsGroup[entityB];
                Systems.ExplosionSystem.MakeExplosion(ref PhysicsWorld,
                    TranslationGroup, RotationGroup, 
                    PhysicsVelocityGroup, PhysicsMassGroup,
                    HealthGroup,
                    TranslationGroup[entityB].Value, shellStats.ExplosionRadius, shellStats.ExplosionForce, shellStats.MaxDamage);

                CommandBuffer.DestroyEntity(entityB);

            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld;

        JobHandle jobHandle = new ShellExplosionJob
        {
            ShellStatsGroup = GetComponentDataFromEntity<ShellStats>(true),
            TranslationGroup = GetComponentDataFromEntity<Translation>(true),
            RotationGroup = GetComponentDataFromEntity<Rotation>(true),
            PhysicsVelocityGroup = GetComponentDataFromEntity<PhysicsVelocity>(),
            PhysicsMassGroup = GetComponentDataFromEntity<PhysicsMass>(true),
            HealthGroup = GetComponentDataFromEntity<PlayerHealth>(),
            PhysicsWorld = physicsWorld,
            CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer(),
        }.Schedule(m_StepPhysicsWorldSystem.Simulation,
                    ref m_BuildPhysicsWorldSystem.PhysicsWorld, inputDeps);
        m_EntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}