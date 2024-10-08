using Unity.Entities;

public struct PlayerMovement : IComponentData {
	public float MoveSpeed;
	public float ReverseSpeed;
	public float TurnSpeed;
}

public struct PlayerShooting : IComponentData {
	public Entity ProjectilePrefab;
	public float SecondsPerShot;
}

public struct EarliestNextShotTime : IComponentData {
	public float Value;
}
