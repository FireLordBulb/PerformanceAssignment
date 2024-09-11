using Unity.Entities;
using UnityEngine;

public class ScreenBoundsAuthoringAuthoring : MonoBehaviour {
    private class ScreenBoundsAuthoringBaker : Baker<ScreenBoundsAuthoringAuthoring> {
        public override void Bake(ScreenBoundsAuthoringAuthoring authoring){
            AddComponent(GetEntity(TransformUsageFlags.None), new ScreenBounds());
        }
    }
}
