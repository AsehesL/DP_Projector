using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixDebug : MonoBehaviour
{
    private Camera m_Camera;

	void Start ()
	{
	    m_Camera = gameObject.GetComponent<Camera>();
	}

    void OnGUI()
    {
        MatrixLabel(m_Camera.worldToCameraMatrix);

        GUILayout.Space(20);

        MatrixLabel(transform.worldToLocalMatrix);
    }

    void MatrixLabel(Matrix4x4 matrix)
    {
        GUILayout.Label(matrix.m00.ToString("f3") + "," + matrix.m01.ToString("f3") + "," + matrix.m02.ToString("f3") +
                        "," + matrix.m03.ToString("f3"));
        GUILayout.Label(matrix.m10.ToString("f3") + "," + matrix.m11.ToString("f3") + "," + matrix.m12.ToString("f3") +
                        "," + matrix.m13.ToString("f3"));
        GUILayout.Label(matrix.m20.ToString("f3") + "," + matrix.m21.ToString("f3") + "," + matrix.m22.ToString("f3") +
                        "," + matrix.m23.ToString("f3"));
        GUILayout.Label(matrix.m30.ToString("f3") + "," + matrix.m31.ToString("f3") + "," + matrix.m32.ToString("f3") +
                        "," + matrix.m33.ToString("f3"));
    }
}
