using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct PhysicsMovementSystem : ISystem {
    [BurstCompile]
    public void OnCreate(ref SystemState state){
        state.RequireForUpdate<ScreenBounds>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        ScreenBounds screenBounds = SystemAPI.GetSingleton<ScreenBounds>();
        foreach (var (physicsMovement, transform) in SystemAPI.Query<RefRW<PhysicsMovement>, RefRW<LocalTransform>>()){
            float3 position = transform.ValueRO.Position;
            position += physicsMovement.ValueRO.Velocity*SystemAPI.Time.DeltaTime;
            // Asteroids-style screen looping (torus topology)
            if (position.x < screenBounds.Left){
                position.x += screenBounds.Width;
            } else if (screenBounds.Right < position.x){
                position.x -= screenBounds.Width;
            } else if (position.y < screenBounds.Bottom){
                position.y += screenBounds.Height;
            } else if (screenBounds.Top < position.y){
                position.y -= screenBounds.Height;
            }
            transform.ValueRW.Position = position;
            
            physicsMovement.ValueRW.Velocity *= 1-physicsMovement.ValueRO.LinearDrag*SystemAPI.Time.DeltaTime;
        }
    }
}