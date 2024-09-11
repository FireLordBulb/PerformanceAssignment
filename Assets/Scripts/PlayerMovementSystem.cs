using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class PlayerMovementSystem : SystemBase {
	private static readonly float3 ScreenNormal = new(0, 0, 1);
	private float3 velocity;
	
	protected override void OnCreate(){
		RequireForUpdate<PlayerMovement>();
	}
	protected override void OnUpdate(){
		foreach (var (playerMovement, playerMoveInput, playerTurnInput, physicsMovement, transform) in SystemAPI.Query<PlayerMovement, PlayerMoveInput, PlayerTurnInput, RefRW<PhysicsMovement>, RefRW<LocalTransform>>()){
			// ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable // Up() just gets a value. 
			float3 direction = transform.ValueRO.Up();
			float moveSpeed = 0 < playerMoveInput.Value ? playerMovement.MoveSpeed : playerMovement.ReverseSpeed;
			physicsMovement.ValueRW.Velocity += playerMoveInput.Value*moveSpeed*SystemAPI.Time.DeltaTime*direction;
			
			quaternion rotation = transform.ValueRO.Rotation;
			rotation = math.mul(rotation, quaternion.AxisAngle(ScreenNormal, playerTurnInput.Value*playerMovement.TurnSpeed*SystemAPI.Time.DeltaTime));
			transform.ValueRW.Rotation = rotation;
		}
	}
}
