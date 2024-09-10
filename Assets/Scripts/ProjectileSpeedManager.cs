using UnityEngine;

public class ProjectileSpeedManager : MonoBehaviour {
    public static float Speed;
    [SerializeField] private float speed;
    private void Awake(){
        Speed = speed;
    }
}