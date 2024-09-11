using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct EnemySystem : ISystem {
    [BurstCompile]
    public void OnCreate(ref SystemState state){
        state.RequireForUpdate<PlayerMovement>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        LocalTransform playerTransform = SystemAPI.GetComponent<LocalTransform>(SystemAPI.GetSingletonEntity<PlayerMovement>());
        foreach (var (enemy, physicsMovement, transform) in SystemAPI.Query<Enemy, RefRW<PhysicsMovement>, RefRW<LocalTransform>>()){
            // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable // Up() just gets a value. 
            float3 direction = transform.ValueRO.Up();
            physicsMovement.ValueRW.Velocity += enemy.MoveSpeed*SystemAPI.Time.DeltaTime*direction;

            quaternion currentRotation = transform.ValueRO.Rotation;
            quaternion targetRotation = quaternion.LookRotation(GlobalVariables.ScreenNormal, math.normalizesafe(playerTransform.Position-transform.ValueRO.Position));
            // Changes the transform's rotation with a constant angular velocity toward targetRotation without overshooting.
            // Unreal Engine has a "RotateTowards" built-in method for this, but alas. Unity!!!!.
            float angle = math.angle(currentRotation, targetRotation);
            float interpolationAlpha = enemy.TurnSpeed*SystemAPI.Time.DeltaTime/angle;
            transform.ValueRW.Rotation = math.nlerp(currentRotation, targetRotation, math.min(interpolationAlpha, 1));
        }
    }
}
