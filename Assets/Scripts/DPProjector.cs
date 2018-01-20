using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPProjector : MonoBehaviour
{
    public float NearClipPlane
    {
        get { return m_NearClipPlane; }
        set
        {
            float v = !m_Orthographic ? Mathf.Max(0.01f, value) : value;
            if (m_NearClipPlane != v)
            {
                m_NearClipPlane = v;
                m_MatrixChanged = true;
                m_MeshChanged = true;
            }
        }
    }

    public float FarClipPlane
    {
        get { return m_FarClipPlane; }
        set
        {
            float v = !m_Orthographic ? Mathf.Max(m_NearClipPlane, value) : value;
            if (m_FarClipPlane != v)
            {
                m_FarClipPlane = v;
                m_MatrixChanged = true;
                m_MeshChanged = true;
            }
        }
    }

    public float FieldOfView
    {
        get { return m_FieldOfView; }
        set
        {
            float v = Mathf.Clamp(value, 1, 180);
            if (m_FieldOfView != v)
            {
                m_FieldOfView = v;
                if (!m_Orthographic)//非正交状态下fov的改变才会引起矩阵和mesh的重建
                {
                    m_MatrixChanged = true;
                    m_MeshChanged = true;
                }
            }
        }
    }

    public float AspectRatio
    {
        get { return m_AspectRatio; }
        set
        {
            if (m_AspectRatio != value)
            {
                m_AspectRatio = value;
                m_MatrixChanged = true;
            }
        }
    }

    public bool Orthographic
    {
        get { return m_Orthographic; }
        set
        {
            if (m_Orthographic != value)
            {
                m_Orthographic = value;
                m_MatrixChanged = true;
                m_MeshChanged = true;
            }
        }
    }

    public float OrthographicSize
    {
        get { return m_OrthographicSize; }
        set
        {
            if (m_OrthographicSize != value)
            {
                m_OrthographicSize = value;
                if (m_Orthographic)//正交状态下改变OrthographicSize才会引起矩阵和Mesh的重建
                {
                    m_MatrixChanged = true;
                    m_MeshChanged = true;
                }
            }
        }
    }

    [SerializeField] private float m_NearClipPlane = 0.1f;
    [SerializeField] private float m_FarClipPlane = 100;
    [SerializeField] private float m_FieldOfView = 60;
    [SerializeField] private float m_AspectRatio = 1;
    [SerializeField] private bool m_Orthographic = false;
    [SerializeField] private float m_OrthographicSize = 10;
    public Material material;

    private DPProjectorMesh m_Mesh;

    private Matrix4x4 m_WorldToProjector;
    private Matrix4x4 m_Projector;

    private Matrix4x4 m_WorldToLocal;

    private bool m_MatrixChanged;//标记矩阵是否发生变化，将矩阵的重计算延迟到下一帧，避免当前帧做了多个改变矩阵的操作而引起一帧内多次重新计算矩阵
    private bool m_MeshChanged;//标记mesh是否需要重构，将Mesh的重新计算延迟到下一帧

    void Awake()
    {
        RecalculateMatrix();
        
        
        m_Mesh = new DPProjectorMesh(gameObject);
        m_Mesh.RebuildMesh(m_Projector);
    }

    void Update()
    {
        if (m_WorldToLocal != transform.worldToLocalMatrix)
            m_MatrixChanged = true;
        if (m_MatrixChanged)
        {
            m_MatrixChanged = false;
            RecalculateMatrix();
        }
        if (m_MeshChanged)
        {
            m_MeshChanged = false;
            m_Mesh.RebuildMesh(m_Projector);
        }
        m_Mesh.DrawMesh(material, transform.localToWorldMatrix, gameObject.layer);
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

        m_WorldToLocal = transform.worldToLocalMatrix;

        if (material != null)
            material.SetMatrix("internal_WorldToProjector", m_WorldToProjector);
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
