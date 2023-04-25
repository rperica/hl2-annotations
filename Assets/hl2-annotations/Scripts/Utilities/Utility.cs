using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility 
{
    public static Vector3 ConvertToVector3(Vec3 vector)
    {
        return new Vector3((float)vector.x, (float)vector.y, (float)vector.z);
    }

    public static Vec3 ConvertToVec3(Vector3 vector)
    {
        return new Vec3((double)vector.x, (double)vector.y, (double)vector.z);
    }
}
