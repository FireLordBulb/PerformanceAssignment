using Unity.Entities;
using UnityEngine;

public class EnemySpawnerAuthoring : MonoBehaviour {
    public GameObject enemyPrefab;
    public float timePerWave;
    public int enemiesPerWave;

    public class EnemySpawnerBaker : Baker<EnemySpawnerAuthoring> {
        public override void Bake(EnemySpawnerAuthoring authoring){
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemySpawner{
                EnemyPrefab = GetEntity(authoring.enemyPrefab, TransformUsageFlags.Dynamic),
                TimePerWave = authoring.timePerWave,
                EnemiesPerWave = authoring.enemiesPerWave
            });
            AddComponent(entity, new NextWaveTime());
        }
    }
}