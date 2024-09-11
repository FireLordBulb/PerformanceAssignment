using Unity.Entities;
using UnityEngine;

public struct ScreenBounds : IComponentData{
    public readonly float Left, Right, Bottom, Top;
    public readonly float Width, Height;

    public ScreenBounds(Camera camera){
        FrustumPlanes frustumPlanes = camera.projectionMatrix.decomposeProjection;
        Vector3 cameraPosition = camera.transform.position;
        Left = frustumPlanes.left+cameraPosition.x;
        Right = frustumPlanes.right+cameraPosition.x;
        Bottom = frustumPlanes.bottom+cameraPosition.y;
        Top = frustumPlanes.top+cameraPosition.y;
        Width = Right-Left;
        Height = Top-Bottom;
    }
}
