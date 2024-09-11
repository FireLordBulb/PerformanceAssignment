using Unity.Mathematics;

public static class GlobalVariables {
    // Vector3 has global right, forward and up constants. Why does float3 not?!
    public static readonly float3 ScreenNormal = new(0, 0, 1);
}