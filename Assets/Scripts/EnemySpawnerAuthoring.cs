using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpawnerAuthoring : MonoBehaviour {
    public GameObject enemyPrefab;
    public float timePerWave;
    public float spawnRadius;
    public float spawnArcAngle;
    public int enemiesPerWave;
    public int maxEnemies;

    public class EnemySpawnerBaker : Baker<EnemySpawnerAuthoring> {
        public override void Bake(EnemySpawnerAuthoring authoring){
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemySpawner{
                EnemyPrefab = GetEntity(authoring.enemyPrefab, TransformUsageFlags.Dynamic),
                TimePerWave = authoring.timePerWave,
                SpawnRadius = authoring.spawnRadius,
                SpawnArcAngle = authoring.spawnArcAngle,
                EnemiesPerWave = authoring.enemiesPerWave,
                MaxEnemies = authoring.maxEnemies
            });
            AddComponent(entity, new EnemyCount());
            AddComponent(entity, new NextWaveTime());
        }
    }
}