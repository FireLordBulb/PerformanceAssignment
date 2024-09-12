using Unity.Entities;
using UnityEngine;

public class ProjectileAuthoring : MonoBehaviour {
    public float speed;
    public float radius;

    public class ProjectileBaker : Baker<ProjectileAuthoring> {
        public override void Bake(ProjectileAuthoring authoring){
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Projectile{
                Speed = authoring.speed
            });
            AddComponent(entity, new Collider{
                Radius = authoring.radius
            });
        }
    }
}