using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct PlayerShootingSystem : ISystem {
	[BurstCompile]
	public void OnCreate(ref SystemState state){
		state.RequireForUpdate<PlayerShooting>();
	}
	[BurstCompile]
	public void OnUpdate(ref SystemState state){
		EntityCommandBuffer commandBuffer = new(Unity.Collections.Allocator.TempJob);
		foreach (var (playerShooting, earliestNextShotTime, playerShootInput, playerTransform) in SystemAPI.Query<PlayerShooting, RefRW<EarliestNextShotTime>, PlayerShootInput, LocalTransform>()){
			if (!playerShootInput.Value){
				continue;
			}
			SystemAPI.SetSingleton(new PlayerShootInput{Value = false});
			if (SystemAPI.Time.ElapsedTime < earliestNextShotTime.ValueRO.Value){
				continue;
			}
			earliestNextShotTime.ValueRW.Value = playerShooting.SecondsPerShot+(float)SystemAPI.Time.ElapsedTime;
			LocalTransform projectileTransform = playerTransform;
			projectileTransform.Scale =	SystemAPI.GetComponent<LocalTransform>(playerShooting.ProjectilePrefab).Scale;
			Entity newProjectile = commandBuffer.Instantiate(playerShooting.ProjectilePrefab);
			commandBuffer.SetComponent(newProjectile, projectileTransform);
			commandBuffer.AddComponent(newProjectile, new Projectile{
				Speed = playerShooting.ProjectileSpeed,
			});
		}
		commandBuffer.Playback(state.EntityManager);
		commandBuffer.Dispose();
	}
}
