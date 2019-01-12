using UnityEngine;

public static class Extentions
{
    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static Vector2 Sub(this Vector2 v2, Vector3 v3)
    {
        return new Vector2(v2.x - v3.x, v2.y - v3.y);
    }
    public static Vector2 Add(this Vector2 v2, Vector3 v3)
    {
        return new Vector2(v2.x + v3.x, v2.y + v3.y);
    }
}
