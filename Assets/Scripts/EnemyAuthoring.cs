using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAuthoring : MonoBehaviour {
    public float moveSpeed;
    public float turnSpeed;
    public float linearDrag;
    public class EnemyBaker : Baker<EnemyAuthoring> {
        public override void Bake(EnemyAuthoring authoring){
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Enemy{
                MoveSpeed = authoring.moveSpeed,
                TurnSpeed = authoring.turnSpeed
            });
            AddComponent(entity, new PhysicsMovement{
                LinearDrag = authoring.linearDrag
            });
        }
    }
}