using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DPProjectorMesh
{
    private Mesh m_Mesh;

    private List<Vector3> m_VertexList;
    private List<Color> m_ColorList;
    private List<int> m_IndexeList;

    private MeshFilter m_MeshFilter;
    private MeshRenderer m_MeshRenderer;

    private GameObject m_Parent;

    public DPProjectorMesh(GameObject parent)
    {
        m_Parent = parent;
        m_VertexList = new List<Vector3>();
        m_ColorList = new List<Color>();
        m_IndexeList = new List<int>();
    }

    public void RebuildMesh(Matrix4x4 projector)
    {
        Matrix4x4 mat = projector.inverse;
        Vector3 p1 = mat.MultiplyPoint(new Vector3(-1, -1, -1));
        Vector3 p2 = mat.MultiplyPoint(new Vector3(1, -1, -1));
        Vector3 p3 = mat.MultiplyPoint(new Vector3(1, -1, 1));
        Vector3 p4 = mat.MultiplyPoint(new Vector3(-1, -1, 1));
        Vector3 p5 = mat.MultiplyPoint(new Vector3(-1, 1, -1));
        Vector3 p6 = mat.MultiplyPoint(new Vector3(1, 1, -1));
        Vector3 p7 = mat.MultiplyPoint(new Vector3(1, 1, 1));
        Vector3 p8 = mat.MultiplyPoint(new Vector3(-1, 1, 1));

        p1.z *= -1;
        p2.z *= -1;
        p3.z *= -1;
        p4.z *= -1;
        p5.z *= -1;
        p6.z *= -1;
        p7.z *= -1;
        p8.z *= -1;

        if (m_Mesh == null)
        {
            m_Mesh = new Mesh();
            SetFaces();
        }
        m_Mesh.Clear();
        if (m_MeshFilter == null)
        {
            m_MeshFilter = new GameObject("[PjMesh]").AddComponent<MeshFilter>();
            m_MeshFilter.transform.SetParent(m_Parent.transform, false);
            m_MeshFilter.sharedMesh = m_Mesh;
        }
        if (m_MeshRenderer == null)
        {
            m_MeshRenderer = m_MeshFilter.gameObject.AddComponent<MeshRenderer>();
        }

        SetVertex(p1, Color.white, 0);
        SetVertex(p2, Color.white, 1);
        SetVertex(p3, Color.white, 2);
        SetVertex(p4, Color.white, 3);
        SetVertex(p5, Color.white, 4);
        SetVertex(p6, Color.white, 5);
        SetVertex(p7, Color.white, 6);
        SetVertex(p8, Color.white, 7);

        m_Mesh.SetVertices(m_VertexList);
        m_Mesh.SetColors(m_ColorList);
        m_Mesh.SetTriangles(m_IndexeList, 0);
    }

    public void RefreshMaterial(Material material)
    {
        if (material == null)
            m_MeshRenderer.enabled = false;
        else
            m_MeshRenderer.enabled = true;
        m_MeshRenderer.sharedMaterial = material;
    }

    public void Release()
    {
        if (m_Mesh)
            Object.Destroy(m_Mesh);
        m_Mesh = null;
        m_VertexList = null;
        m_ColorList = null;
        m_IndexeList = null;
    }

    private void SetVertex(Vector3 position, Color color, int index)
    {
        if (m_VertexList == null) return;
        if (index >= m_VertexList.Count)
        {
            m_VertexList.Add(position);
            m_ColorList.Add(color);
        }
        else
        {
            m_VertexList[index] = position;
            m_ColorList[index] = color;
        }
    }

    private void SetFaces()
    {
        SetFace(0, 4, 5, 1);
        SetFace(2, 6, 7, 3);
        SetFace(4, 7, 6, 5);
        SetFace(5, 6, 2, 1);
        SetFace(1, 2, 3, 0);
        SetFace(0, 3, 7, 4);
    }

    private void SetFace(int id0, int id1, int id2, int id3)
    {
        m_IndexeList.Add(id0);
        m_IndexeList.Add(id1);
        m_IndexeList.Add(id2);

        m_IndexeList.Add(id0);
        m_IndexeList.Add(id2);
        m_IndexeList.Add(id3);
    }

    private void SetIndex(int index, int value)
    {
        if (m_IndexeList == null) return;
        if (index >= m_IndexeList.Count)
            m_IndexeList.Add(value);
        else
            m_IndexeList[index] = value;
    }
}
