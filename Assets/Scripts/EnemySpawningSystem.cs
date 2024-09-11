using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct EnemySpawningSystem : ISystem {
    private Entity player;
    public void OnCreate(ref SystemState state){
        state.RequireForUpdate<EnemySpawner>();
        state.RequireForUpdate<PlayerMovement>();
        player = SystemAPI.GetSingletonEntity<PlayerMovement>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        LocalTransform playerTransform = SystemAPI.GetComponent<LocalTransform>(player);
        EntityCommandBuffer commandBuffer = new(Unity.Collections.Allocator.TempJob);
        foreach (EnemySpawner spawner in SystemAPI.Query<EnemySpawner>()){
            
        }
        commandBuffer.Playback(state.EntityManager);
        commandBuffer.Dispose();
    }
}
