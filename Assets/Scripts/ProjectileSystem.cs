using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct ProjectileSystem : ISystem {
    [BurstCompile]
    public void OnCreate(ref SystemState state){
        state.RequireForUpdate<ScreenBounds>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        ScreenBounds screenBounds = SystemAPI.GetSingleton<ScreenBounds>();
        EntityCommandBuffer commandBuffer = new(Unity.Collections.Allocator.TempJob);
        foreach (var (projectile, transform, entity) in SystemAPI.Query<Projectile, RefRW<LocalTransform>>().WithEntityAccess()){
            float3 position = transform.ValueRO.Position;
            // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable // Not an impure method. 
            float3 direction = transform.ValueRO.Up();
            position += projectile.Speed*SystemAPI.Time.DeltaTime*direction;
            // Projectiles don't loop around the edge of the screen like the player and enemies, they get destroyed when reaching the edge of the screen.
            if (position.x < screenBounds.Left || screenBounds.Right < position.x || position.y < screenBounds.Bottom || screenBounds.Top < position.y){
                commandBuffer.DestroyEntity(entity);
            } else {
                transform.ValueRW.Position = position;
            }
        }
        commandBuffer.Playback(state.EntityManager);
        commandBuffer.Dispose();
    }
}
