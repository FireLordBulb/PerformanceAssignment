using Unity.Entities;
using Unity.Mathematics;

public struct PhysicsMovement : IComponentData {
    // Why is there drag in space? Shouldn't running out of fuel and relativity be the only limits on speed?
    public float LinearDrag;
    public float3 Velocity;
}