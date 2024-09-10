using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class PlayerMovementSystem : SystemBase {
	private static readonly float3 ScreenNormal = new(0, 0, 1);
	private float3 velocity;
	
	protected override void OnCreate(){
		RequireForUpdate<PlayerMovement>();
	}
	protected override void OnUpdate(){
		Camera mainCamera = Camera.main;
		// Don't update the player if the main camera doesn't exist.
		if (!mainCamera){
			return;
		}
		FrustumPlanes cameraFrustumPlanes = mainCamera.projectionMatrix.decomposeProjection;
		Vector3 cameraPosition = mainCamera.transform.position;
		foreach (var (playerMovement, playerMoveInput, playerTurnInput, transform) in SystemAPI.Query<PlayerMovement, PlayerMoveInput, PlayerTurnInput, RefRW<LocalTransform>>()){
			float3 position = transform.ValueRO.Position;
			// ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable // Not an impure method. 
			float3 direction = transform.ValueRO.Up();
			velocity *= 1-playerMovement.LinearDrag*SystemAPI.Time.DeltaTime;
			velocity += playerMoveInput.Value*(0 < playerMoveInput.Value ? playerMovement.MoveSpeed : playerMovement.ReverseSpeed)*SystemAPI.Time.DeltaTime*direction;
			position += velocity*SystemAPI.Time.DeltaTime;
			// Asteroids-style screen looping (torus topology)
			if (position.x < cameraFrustumPlanes.left+cameraPosition.x){
				position.x += cameraFrustumPlanes.right-cameraFrustumPlanes.left;
			} else if (cameraFrustumPlanes.right+cameraPosition.x < position.x){
				position.x -= cameraFrustumPlanes.right-cameraFrustumPlanes.left;
			} else if (position.y < cameraFrustumPlanes.bottom+cameraPosition.y){
				position.y += cameraFrustumPlanes.top-cameraFrustumPlanes.bottom;
			} else if (cameraFrustumPlanes.top+cameraPosition.y < position.y){
				position.y -= cameraFrustumPlanes.top-cameraFrustumPlanes.bottom;
			}
			transform.ValueRW.Position = position;
			
			quaternion rotation = transform.ValueRO.Rotation;
			rotation = math.mul(rotation, quaternion.AxisAngle(ScreenNormal, playerTurnInput.Value*playerMovement.TurnSpeed*SystemAPI.Time.DeltaTime));
			transform.ValueRW.Rotation = rotation;
		}
	}
}
