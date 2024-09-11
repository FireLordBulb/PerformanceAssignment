using Unity.Entities;

public struct EnemySpawner : IComponentData {
    public Entity EnemyPrefab;
    public float TimePerWave;
    public int EnemiesPerWave;
}

public struct NextWaveTime : IComponentData {
    public float Value;
}
