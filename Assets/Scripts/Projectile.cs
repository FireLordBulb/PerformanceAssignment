using System;
using Unity.Entities;
using UnityEngine;

public struct Projectile : IComponentData {}

public class ProjectileSpeedManager : MonoBehaviour {
    public static float Speed;
    [SerializeField] private float speed;
    private void Awake(){
        Speed = speed;
    }
}
