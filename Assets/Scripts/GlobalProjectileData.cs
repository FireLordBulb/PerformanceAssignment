using UnityEngine;

public class GlobalProjectileData : MonoBehaviour {
    public static float Speed;
    [SerializeField] private float speed;
    private void Awake(){
        Speed = speed;
    }
}