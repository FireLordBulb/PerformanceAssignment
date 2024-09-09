using Unity.Entities;

public struct Player : IComponentData {
	public Entity ProjectilePrefab;
	public float MoveSpeed;
	public float ReverseSpeed;
	public float TurnSpeed;
	public float SecondsPerShot;
}