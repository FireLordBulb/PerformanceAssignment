using Unity.Burst;
using Unity.Entities;

public partial struct ProjectileSystem : ISystem {
    [BurstCompile]
    public void OnCreate(ref SystemState state){}

    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state){}
}
