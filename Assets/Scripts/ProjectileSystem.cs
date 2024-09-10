using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct ProjectileSystem : ISystem {
    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        EntityCommandBuffer commandBuffer = new(Unity.Collections.Allocator.TempJob);
        foreach (var (projectile, transform, entity) in SystemAPI.Query<Projectile, RefRW<LocalTransform>>().WithEntityAccess()){
            float3 position = transform.ValueRO.Position;
            // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable // Not an impure method. 
            float3 direction = transform.ValueRO.Up();
            position += projectile.Speed*SystemAPI.Time.DeltaTime*direction;
            transform.ValueRW.Position = position;
            if (projectile.LifetimeEnd < state.WorldUnmanaged.Time.ElapsedTime){
                commandBuffer.DestroyEntity(entity);
            }
        }
        commandBuffer.Playback(state.EntityManager);
        commandBuffer.Dispose();
    }
}
