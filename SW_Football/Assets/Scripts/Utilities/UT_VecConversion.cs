/*************************************************************************************
Converts a Vector2 into a vector3, but with the z value as the y value.
It will also do the opposite.

Vec2(5,2) -> Vec3(5, 0, 2)
Vec3(5,0,2) -> Vec2(5,2)
*************************************************************************************/
using UnityEngine;

public static class UT_VecConversion
{
    public static Vector3 ConvertVec2(Vector2 v)
    {
        return new Vector3(
            v.x,
            0f,
            v.y 
        );
    }

    public static Vector2 ConvertVec3(Vector3 v)
    {
        return new Vector2(
            v.x,
            v.z
        );
    }
}
