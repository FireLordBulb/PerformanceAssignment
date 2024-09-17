using System.Diagnostics;
using Unity.Entities;
using UnityEngine;

// This system exists solely to pass needed values of the camera from the managed to the unmanaged world.
public partial class ScreenBoundsSystem : SystemBase {
    protected override void OnStartRunning(){
        // If there's no main camera in this game with Asteroids-style screen looping, all hope is lost.
        Trace.Assert(Camera.main != null, "CRITICAL ERROR: Can't find main camera!");
        EntityManager.AddComponent<ScreenBounds>(EntityManager.CreateEntity());
        SystemAPI.SetSingleton(new ScreenBounds(Camera.main));
    }
    protected override void OnUpdate(){}
}
