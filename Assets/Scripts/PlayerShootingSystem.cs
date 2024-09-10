using Unity.Entities;
using Unity.Transforms;

public partial class PlayerShootingSystem : SystemBase {

	private double lastShotTime = float.NegativeInfinity;
	
	protected override void OnCreate(){
		RequireForUpdate<PlayerShooting>();
	}
	protected override void OnUpdate(){
		EntityCommandBuffer commandBuffer = new(Unity.Collections.Allocator.TempJob);
		foreach (var (playerShooting, playerShootInput, playerTransform) in SystemAPI.Query<PlayerShooting, PlayerShootInput, LocalTransform>()){
			if (!playerShootInput.Value){
				continue;
			}
			SystemAPI.SetSingleton(new PlayerShootInput{Value = false});
			if (SystemAPI.Time.ElapsedTime < lastShotTime+playerShooting.SecondsPerShot){
				continue;
			}
			lastShotTime = SystemAPI.Time.ElapsedTime;
			LocalTransform projectileTransform = playerTransform;
			projectileTransform.Scale =	SystemAPI.GetComponent<LocalTransform>(playerShooting.ProjectilePrefab).Scale;
			Entity newProjectile = commandBuffer.Instantiate(playerShooting.ProjectilePrefab);
			commandBuffer.SetComponent(newProjectile, projectileTransform);
			commandBuffer.AddComponent(newProjectile, new Projectile{Speed = playerShooting.ProjectileSpeed});
		}
		commandBuffer.Playback(EntityManager);
		commandBuffer.Dispose();
	}
}
