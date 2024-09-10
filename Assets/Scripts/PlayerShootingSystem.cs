using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Transforms;

[StructLayout(LayoutKind.Auto)]
public partial struct PlayerShootingSystem : ISystem {
	private float lastShotTime;
	
	public void OnCreate(ref SystemState state){
		lastShotTime = float.NegativeInfinity;
		state.RequireForUpdate<PlayerShooting>();
	}
	public void OnUpdate(ref SystemState state){
		EntityCommandBuffer commandBuffer = new(Unity.Collections.Allocator.TempJob);
		foreach (var (playerShooting, playerShootInput, playerTransform) in SystemAPI.Query<PlayerShooting, PlayerShootInput, LocalTransform>()){
			if (!playerShootInput.Value){
				continue;
			}
			SystemAPI.SetSingleton(new PlayerShootInput{Value = false});
			if (SystemAPI.Time.ElapsedTime < lastShotTime+playerShooting.SecondsPerShot){
				continue;
			}
			lastShotTime = (float)SystemAPI.Time.ElapsedTime;
			LocalTransform projectileTransform = playerTransform;
			projectileTransform.Scale =	SystemAPI.GetComponent<LocalTransform>(playerShooting.ProjectilePrefab).Scale;
			Entity newProjectile = commandBuffer.Instantiate(playerShooting.ProjectilePrefab);
			commandBuffer.SetComponent(newProjectile, projectileTransform);
			commandBuffer.AddComponent(newProjectile, new Projectile{
				Speed = playerShooting.ProjectileSpeed,
				LifetimeEnd = playerShooting.ProjectileLifetime+(float)SystemAPI.Time.ElapsedTime
			});
		}
		commandBuffer.Playback(state.EntityManager);
		commandBuffer.Dispose();
	}
}
