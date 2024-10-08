using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour {
	public float moveSpeed; 
	public float reverseSpeed;
	public float turnSpeed;
	public float linearDrag;
	public GameObject projectilePrefab;
	public float secondsPerShot;

	public class PlayerBaker : Baker<PlayerAuthoring> {
		public override void Bake(PlayerAuthoring authoring){
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent(entity, new PlayerMovement {
				MoveSpeed = authoring.moveSpeed,
				ReverseSpeed = authoring.reverseSpeed,
				TurnSpeed = authoring.turnSpeed,
			});
			AddComponent(entity, new PhysicsMovement{
				LinearDrag = authoring.linearDrag,
			});
			AddComponent(entity, new PlayerShooting{
				ProjectilePrefab = GetEntity(authoring.projectilePrefab, TransformUsageFlags.Dynamic),
				SecondsPerShot = authoring.secondsPerShot,
			});
			AddComponent(entity, new EarliestNextShotTime());
			AddComponent(entity, new PlayerInput());
			AddComponent(entity, new PlayerMoveInput());
			AddComponent(entity, new PlayerTurnInput());
			AddComponent(entity, new PlayerShootInput());
		}
	}
}