using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class PlayerSystem : SystemBase {
	private static readonly float3 ScreenNormal = new(0, 0, 1);
	protected override void OnCreate(){
		RequireForUpdate<PlayerInput>();
		RequireForUpdate<Player>();
	}
	protected override void OnUpdate(){
		foreach (var (player, playerMoveInput, playerTurnInput, playerShootInput, transform) in SystemAPI.Query<Player, PlayerMoveInput, PlayerTurnInput, PlayerShootInput, RefRW<LocalTransform>>()){
			float3 position = transform.ValueRO.Position;
			// ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable // Not an impure method. 
			float3 direction = transform.ValueRO.Up();
			transform.ValueRW.Position = position + direction*playerMoveInput.Value*(0 < playerMoveInput.Value ? player.MoveSpeed : player.ReverseSpeed)*SystemAPI.Time.DeltaTime;
			
			quaternion rotation = transform.ValueRO.Rotation;
			rotation = math.mul(rotation, quaternion.AxisAngle(ScreenNormal, playerTurnInput.Value*player.TurnSpeed*SystemAPI.Time.DeltaTime));
			transform.ValueRW.Rotation = rotation;
		}
	}
}
