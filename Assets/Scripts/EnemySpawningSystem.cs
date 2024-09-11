using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;

public partial struct EnemySpawningSystem : ISystem {
    private const float RightAngle = math.PI/2;
    [BurstCompile]
    public void OnCreate(ref SystemState state){
        state.RequireForUpdate<EnemySpawner>();
        state.RequireForUpdate<PlayerMovement>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        LocalTransform playerTransform = SystemAPI.GetComponent<LocalTransform>(SystemAPI.GetSingletonEntity<PlayerMovement>());
        EntityCommandBuffer commandBuffer = new(Unity.Collections.Allocator.TempJob);
        foreach (var (spawner, nextWaveTime) in SystemAPI.Query<EnemySpawner, RefRW<NextWaveTime>>()){
            if (SystemAPI.Time.ElapsedTime < nextWaveTime.ValueRO.Value){
                continue;
            }
            nextWaveTime.ValueRW.Value = spawner.TimePerWave+(float)SystemAPI.Time.ElapsedTime;
            const float radius = 3.5f;
            for (int i = 0; i < spawner.EnemiesPerWave; i++){
                LocalTransform enemyTransform = playerTransform;
                float randomAngle = Random.value*math.TAU;
                enemyTransform.Position += radius * new float3(math.cos(randomAngle), math.sin(randomAngle), 0);
                // The enemy sprite's "forward" when having 0 rotation is up (y+ direction), but sin and cos with 0 rotation
                // is to the right (x+ direction), adding a right angle to the angle will make the enemy look toward the player.
                enemyTransform.Rotation = quaternion.AxisAngle(GlobalVariables.ScreenNormal, randomAngle+RightAngle);
                Entity newEnemy = commandBuffer.Instantiate(spawner.EnemyPrefab);
                commandBuffer.SetComponent(newEnemy, enemyTransform);
            }
        }
        commandBuffer.Playback(state.EntityManager);
        commandBuffer.Dispose();
    }
}
