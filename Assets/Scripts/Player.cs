using Unity.Entities;

public struct Player : IComponentData {
	public Entity ProjectilePrefab;
	public float MoveSpeed;
	public float ReverseSpeed;
	public float TurnSpeed;
	// Why is there drag in space? Shouldn't running out of fuel and relativity be the only limits on speed?
	public float LinearDrag;
	public float SecondsPerShot;
}