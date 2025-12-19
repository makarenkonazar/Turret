using UnityEngine;

public static class Utility
{
    public static Vector3 MakeHorizontal(Vector3 vec)
    {
        return new Vector3(vec.x, 0, vec.z);
    }

    public static Vector3 NormalizeByMax(Vector3 v)
    {
        float m = Mathf.Max(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        if (m == 0f) return Vector3.zero;
        return v / m;
    }

    public static Vector3 RotateVector(Vector3 v, Vector3 axis, float degrees)
    {
        return Quaternion.AngleAxis(degrees, axis) * v;
    }

    public static T AutoAssign<T>(ref T field, Component owner) where T : Component
    {
        if (field == null)
            field = owner.GetComponent<T>();
        return field;
    }
}