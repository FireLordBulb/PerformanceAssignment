using Unity.Entities;

public struct Enemy : IComponentData {
    public float MoveSpeed;
    public float TurnSpeed;
}