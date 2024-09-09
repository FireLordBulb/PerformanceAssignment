using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class PlayerSystem : SystemBase {
	private static readonly float3 ScreenNormal = new(0, 0, 1);
	private float3 velocity;
	
	protected override void OnCreate(){
		RequireForUpdate<PlayerInput>();
		RequireForUpdate<Player>();
	}
	protected override void OnUpdate(){
		// Don't update the player if the main camera doesn't exist.
		if (!Camera.main){
			return;
		}
		FrustumPlanes cameraFrustrumPlanes =  Camera.main.projectionMatrix.decomposeProjection;
		Vector3 cameraPosition = Camera.main.transform.position;
		foreach (var (player, playerMoveInput, playerTurnInput, playerShootInput, transform) in SystemAPI.Query<Player, PlayerMoveInput, PlayerTurnInput, PlayerShootInput, RefRW<LocalTransform>>()){
			float3 position = transform.ValueRO.Position;
			// ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable // Not an impure method. 
			float3 direction = transform.ValueRO.Up();
			velocity *= 1-player.LinearDrag*SystemAPI.Time.DeltaTime;
			velocity += direction*playerMoveInput.Value*(0 < playerMoveInput.Value ? player.MoveSpeed : player.ReverseSpeed)*SystemAPI.Time.DeltaTime;
			position += velocity*SystemAPI.Time.DeltaTime;
			// Asteroids-style screen looping (torus topology)
			if (position.x < cameraFrustrumPlanes.left+cameraPosition.x){
				position.x += cameraFrustrumPlanes.right-cameraFrustrumPlanes.left;
			} else if (cameraFrustrumPlanes.right+cameraPosition.x < position.x){
				position.x -= cameraFrustrumPlanes.right-cameraFrustrumPlanes.left;
			} else if (position.y < cameraFrustrumPlanes.bottom+cameraPosition.y){
				position.y += cameraFrustrumPlanes.top-cameraFrustrumPlanes.bottom;
			} else if (cameraFrustrumPlanes.top+cameraPosition.y < position.y){
				position.y -= cameraFrustrumPlanes.top-cameraFrustrumPlanes.bottom;
			}
			transform.ValueRW.Position = position;
			
			quaternion rotation = transform.ValueRO.Rotation;
			rotation = math.mul(rotation, quaternion.AxisAngle(ScreenNormal, playerTurnInput.Value*player.TurnSpeed*SystemAPI.Time.DeltaTime));
			transform.ValueRW.Rotation = rotation;
		}
	}
}
