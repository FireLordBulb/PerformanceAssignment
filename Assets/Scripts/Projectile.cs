using Unity.Entities;

public struct Projectile : IComponentData {
    public float Speed;
    public float LifetimeEnd;
}