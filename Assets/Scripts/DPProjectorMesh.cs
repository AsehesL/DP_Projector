using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DPProjectorMesh
{
    private Mesh m_Mesh;

    private List<Vector3> m_VertexList;
    private List<int> m_IndexeList;

    public DPProjectorMesh()
    {
        m_VertexList = new List<Vector3>();
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
            m_Mesh.MarkDynamic();
            SetFaces();
        }
        m_Mesh.Clear();

        SetVertex(p1, 0);
        SetVertex(p2, 1);
        SetVertex(p3, 2);
        SetVertex(p4, 3);
        SetVertex(p5, 4);
        SetVertex(p6, 5);
        SetVertex(p7, 6);
        SetVertex(p8, 7);

        m_Mesh.SetVertices(m_VertexList);
        m_Mesh.SetTriangles(m_IndexeList, 0);
    }

    public void DrawMesh(Material material, Matrix4x4 matrix, int layer)
    {
        if (material == null)
            return;
        Graphics.DrawMesh(m_Mesh, matrix, material, layer);
    }

    public void Release()
    {
        if (m_Mesh)
            Object.Destroy(m_Mesh);
        m_Mesh = null;
        m_VertexList = null;
        m_IndexeList = null;
    }

    private void SetVertex(Vector3 position, int index)
    {
        if (m_VertexList == null) return;
        if (index >= m_VertexList.Count)
        {
            m_VertexList.Add(position);
        }
        else
        {
            m_VertexList[index] = position;
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
