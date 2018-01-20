using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPProjector : MonoBehaviour
{

    [SerializeField] private float m_NearClipPlane = 0.1f;
    [SerializeField] private float m_FarClipPlane = 100;
    [SerializeField] private float m_FieldOfView = 60;
    [SerializeField] private float m_AspectRatio = 1;
    [SerializeField] private bool m_Orthographic = false;
    [SerializeField] private float m_OrthographicSize = 10;
    [SerializeField] private Material m_Material;

    private DPProjectorMesh m_Mesh;

    private Matrix4x4 m_WorldToProjector;
    private Matrix4x4 m_Projector;


    void Awake()
    {
        RecalculateMatrix();
        m_Mesh = new DPProjectorMesh(gameObject);
        m_Mesh.RebuildMesh(m_Projector);
        m_Mesh.RefreshMaterial(m_Material);
    }

    void OnDestroy()
    {
        if (m_Mesh != null)
        {
            m_Mesh.Release();
            m_Mesh = null;
        }
    }

    private void RecalculateMatrix()
    {
        Matrix4x4 toLocal = transform.worldToLocalMatrix;
        toLocal.m20 *= -1;
        toLocal.m21 *= -1;
        toLocal.m22 *= -1;
        toLocal.m23 *= -1;

        m_Projector = default(Matrix4x4);
        if (m_Orthographic)
            m_Projector = Matrix4x4.Ortho(-m_AspectRatio * m_OrthographicSize, m_AspectRatio * m_OrthographicSize,
                -m_OrthographicSize, m_OrthographicSize, m_NearClipPlane, m_FarClipPlane);
        else
            m_Projector = Matrix4x4.Perspective(m_FieldOfView, m_AspectRatio, m_NearClipPlane, m_FarClipPlane);
        m_WorldToProjector = m_Projector * toLocal;
    }

    void OnDrawGizmosSelected()
    {
        Matrix4x4 proj = default(Matrix4x4);
        if (m_Orthographic)
            proj = Matrix4x4.Ortho(-m_AspectRatio*m_OrthographicSize, m_AspectRatio*m_OrthographicSize,
                -m_OrthographicSize, m_OrthographicSize, m_NearClipPlane, m_FarClipPlane);
        else
            proj = Matrix4x4.Perspective(m_FieldOfView, m_AspectRatio, m_NearClipPlane, m_FarClipPlane);
        GizmosEx.DrawViewFrustum(proj, transform.localToWorldMatrix, new Color(0.65f,0.65f,0.65f,1));
    }
}
