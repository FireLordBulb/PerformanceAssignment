using Unity.Entities;
using UnityEngine.Serialization;

public struct EnemySpawner : IComponentData {
    public Entity EnemyPrefab;
    public float TimePerWave;
    public int EnemiesPerWave;
}