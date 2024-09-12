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
        foreach (var (spawner, nextWaveTime, enemyCount) in SystemAPI.Query<EnemySpawner, RefRW<NextWaveTime>, RefRW<EnemyCount>>()){
            if (SystemAPI.Time.ElapsedTime < nextWaveTime.ValueRO.Value){
                continue;
            }
            // For the first wave the min angle is set such that the spawn arc is directly in front of the player.
            // For all other waves, the min angle is fully random.
            float spawnArcMin = nextWaveTime.ValueRO.Value <= 0 ? RightAngle-spawner.SpawnArcAngle/2 : Random.value*math.TAU;
            float spawnArcMax = spawnArcMin+spawner.SpawnArcAngle;
            nextWaveTime.ValueRW.Value = spawner.TimePerWave+(float)SystemAPI.Time.ElapsedTime;
            for (int i = 0; i < spawner.EnemiesPerWave && enemyCount.ValueRO.Value < spawner.MaxEnemies; i++){
                LocalTransform enemyTransform = playerTransform;
                float spawnAngle = Random.Range(spawnArcMin, spawnArcMax);
                enemyTransform.Position += spawner.SpawnRadius * new float3(math.cos(spawnAngle), math.sin(spawnAngle), 0);
                // The enemy sprite's "forward" when having 0 rotation is up (y+ direction), but sin and cos with 0 rotation
                // is to the right (x+ direction), adding a right angle to the angle will make the enemy look toward the player.
                enemyTransform.Rotation = quaternion.AxisAngle(GlobalVariables.ScreenNormal, spawnAngle+RightAngle);
                Entity newEnemy = commandBuffer.Instantiate(spawner.EnemyPrefab);
                commandBuffer.SetComponent(newEnemy, enemyTransform);
                enemyCount.ValueRW.Value++;
            }
        }
        commandBuffer.Playback(state.EntityManager);
        commandBuffer.Dispose();
    }
}
