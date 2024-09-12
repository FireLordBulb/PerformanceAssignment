using Unity.Entities;

public struct EnemySpawner : IComponentData {
    public Entity EnemyPrefab;
    public float TimePerWave;
    public float SpawnRadius;
    public float SpawnArcAngle;
    public int EnemiesPerWave;
    public int MaxEnemies;
}

public struct NextWaveTime : IComponentData {
    public float Value;
}

public struct EnemyCount : IComponentData {
    public int Value;
}