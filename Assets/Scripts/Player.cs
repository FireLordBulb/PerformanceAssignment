using Unity.Entities;

public struct PlayerMovement : IComponentData {
	public float MoveSpeed;
	public float ReverseSpeed;
	public float TurnSpeed;
	// Why is there drag in space? Shouldn't running out of fuel and relativity be the only limits on speed?
	public float LinearDrag;
}

public struct PlayerShooting : IComponentData {
	public Entity ProjectilePrefab;
	public float SecondsPerShot;
	public float ProjectileSpeed;
	public float ProjectileLifetime;
}

public struct LastShotTime : IComponentData {
	public float Value;
}
