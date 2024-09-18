using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct PlayerMovementSystem : ISystem {
	[BurstCompile]
	public void OnCreate(ref SystemState state){
		state.RequireForUpdate<PlayerMovement>();
	}
	[BurstCompile]
	public void OnUpdate(ref SystemState state){
		foreach (var (playerMovement, playerMoveInput, playerTurnInput, physicsMovement, transform) in SystemAPI.Query<PlayerMovement, PlayerMoveInput, PlayerTurnInput, RefRW<PhysicsMovement>, RefRW<LocalTransform>>()){
			// ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable // Up() just gets a value. 
			float3 direction = transform.ValueRO.Up();
			float moveSpeed = 0 < playerMoveInput.Value ? playerMovement.MoveSpeed : playerMovement.ReverseSpeed;
			physicsMovement.ValueRW.Velocity += playerMoveInput.Value*moveSpeed*SystemAPI.Time.DeltaTime*direction;
			
			quaternion rotation = transform.ValueRO.Rotation;
			rotation = math.mul(rotation, quaternion.AxisAngle(GlobalVariables.ScreenNormal, playerTurnInput.Value*playerMovement.TurnSpeed*SystemAPI.Time.DeltaTime));
			transform.ValueRW.Rotation = rotation;
		}
	}
}
