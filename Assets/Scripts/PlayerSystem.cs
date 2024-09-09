using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class PlayerSystem : SystemBase {
	protected override void OnCreate(){
		RequireForUpdate<PlayerInput>();
		RequireForUpdate<Player>();
	}
	protected override void OnUpdate(){
		foreach (var (player, playerMoveInput, playerTurnInput, playerShootInput, transform) in SystemAPI.Query<Player, PlayerMoveInput, PlayerTurnInput, PlayerShootInput, RefRW<LocalTransform>>()){
			float3 position = transform.ValueRO.Position;
			quaternion rotation = transform.ValueRO.Rotation;
			transform.ValueRW.Position = position;
		}
	}
}
