using UnityEngine;
using System.Collections;

public static class GizmosEx
{

    public static void DrawViewFrustum(Matrix4x4 projection, Matrix4x4 toWorld, Color color)
    {
        toWorld.m02 *= -1;
        toWorld.m12 *= -1;
        toWorld.m22 *= -1;
        Matrix4x4 mat = toWorld*projection.inverse;
        Vector3 p1 = mat.MultiplyPoint(new Vector3(-1, -1, -1));
        Vector3 p2 = mat.MultiplyPoint(new Vector3(1, -1, -1));
        Vector3 p3 = mat.MultiplyPoint(new Vector3(1, -1, 1));
        Vector3 p4 = mat.MultiplyPoint(new Vector3(-1, -1, 1));
        Vector3 p5 = mat.MultiplyPoint(new Vector3(-1, 1, -1));
        Vector3 p6 = mat.MultiplyPoint(new Vector3(1, 1, -1));
        Vector3 p7 = mat.MultiplyPoint(new Vector3(1, 1, 1));
        Vector3 p8 = mat.MultiplyPoint(new Vector3(-1, 1, 1));
        

        Gizmos.color = color;

        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);

        Gizmos.DrawLine(p5, p6);
        Gizmos.DrawLine(p6, p7);
        Gizmos.DrawLine(p7, p8);
        Gizmos.DrawLine(p8, p5);

        Gizmos.DrawLine(p1, p5);
        Gizmos.DrawLine(p2, p6);
        Gizmos.DrawLine(p3, p7);
        Gizmos.DrawLine(p4, p8);
    }
}
