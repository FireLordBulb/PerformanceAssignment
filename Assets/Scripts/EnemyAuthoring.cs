using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour {
    public float speed;
    public float linearDrag;
    public class EnemyBaker : Baker<EnemyAuthoring> {
        public override void Bake(EnemyAuthoring authoring){
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Enemy{
                Speed = authoring.speed
            });
            AddComponent(entity, new PhysicsMovement{
                LinearDrag = authoring.linearDrag
            });
        }
    }
}