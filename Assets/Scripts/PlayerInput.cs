using Unity.Entities;

public struct PlayerInput : IComponentData {}

public struct PlayerMoveInput : IComponentData {
	public float Value;
}

public struct PlayerTurnInput : IComponentData {
	public float Value;
}

public struct PlayerShootInput : IComponentData {
	public bool Value;
}
