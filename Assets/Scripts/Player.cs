using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

public struct Player : IComponentData {
	public Entity ProjectilePrefab;
	public float MoveSpeed;
	public float ReverseSpeed;
	public float TurnSpeed;
	public float SecondsPerShot;
}

public class PlayerAuthoring : MonoBehaviour {
	public GameObject projectilePrefab;
	public float moveSpeed; 
	public float reverseSpeed;
	public float turnSpeed;
	public float secondsPerShot;

	public class PlayerBaker : Baker<PlayerAuthoring> {
		public override void Bake(PlayerAuthoring authoring){
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent(entity, new Player {
				ProjectilePrefab = GetEntity(authoring.projectilePrefab, TransformUsageFlags.Dynamic),
				MoveSpeed = authoring.moveSpeed,
				ReverseSpeed = authoring.reverseSpeed,
				TurnSpeed = authoring.turnSpeed,
				SecondsPerShot = authoring.secondsPerShot
			});
		}
	}
}
