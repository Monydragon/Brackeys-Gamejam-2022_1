using System.Collections;
using UnityEngine;

public static class Common
{
    // a map function in difrence data type
    public static Vector2 map(Vector2 x, float in_min, float in_max, float out_min, float out_max)
    {
        return new Vector2(map(x.x, in_min, in_max, out_min, out_max), map(x.y, in_min, in_max, out_min, out_max));
    }
    public static float map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
