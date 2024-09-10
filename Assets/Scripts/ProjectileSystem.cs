using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct ProjectileSystem : ISystem {
    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        foreach (var (projectile, transform) in SystemAPI.Query<Projectile, RefRW<LocalTransform>>()){
            float3 position = transform.ValueRO.Position;
            // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable // Not an impure method. 
            float3 direction = transform.ValueRO.Up();
            position += projectile.Speed*SystemAPI.Time.DeltaTime*direction;
            transform.ValueRW.Position = position;
        }
    }
}
