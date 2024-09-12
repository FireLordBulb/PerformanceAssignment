using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct CollisionSystem : ISystem {
    [BurstCompile]
    public void OnCreate(ref SystemState state){
        state.RequireForUpdate<EnemyCount>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        EnemyCount enemyCount = SystemAPI.GetSingleton<EnemyCount>();
        EntityCommandBuffer commandBuffer = new(Unity.Collections.Allocator.TempJob);
        // This is O(n*m), where n and m are the number of projectiles and enemies respectively.
        // The number of bullets is at most 4 or 5 because bullets move fast and get destroyed when they leave the screen.
        // So this is essentially linear time, which is the best possible since all enemies HAVE to be checked.
        foreach (var (projectileCollider, projectileTransform, projectileEntity) in SystemAPI.Query<Collider, LocalTransform>().WithAll<Projectile>().WithEntityAccess()){
            foreach (var (enemyCollider, enemyTransform, enemyEntity) in SystemAPI.Query<Collider, LocalTransform>().WithAll<Enemy>().WithEntityAccess()){
                if (math.square(projectileCollider.Radius+enemyCollider.Radius) < math.distancesq(projectileTransform.Position, enemyTransform.Position)){
                    continue;
                }
                commandBuffer.DestroyEntity(projectileEntity);
                commandBuffer.DestroyEntity(enemyEntity);
                enemyCount.Value--;
                SystemAPI.SetSingleton(enemyCount);
                break;
            }
        }
        commandBuffer.Playback(state.EntityManager);
        commandBuffer.Dispose();
    }
}