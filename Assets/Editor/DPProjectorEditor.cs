using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DPProjector))]
public class DPProjectorEditor : Editor
{
    private DPProjector m_Target;

    void OnEnable()
    {
        m_Target = (DPProjector) target;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        Undo.RecordObject(target, "Undo DPProjector Inspector");
        m_Target.NearClipPlane = EditorGUILayout.FloatField("Near Clip Plane", m_Target.NearClipPlane);

        m_Target.FarClipPlane = EditorGUILayout.FloatField("Far Clip Plane", m_Target.FarClipPlane);

        m_Target.FieldOfView = EditorGUILayout.FloatField("Field Of View", m_Target.FieldOfView);

        m_Target.AspectRatio = EditorGUILayout.FloatField("Aspect Ratio", m_Target.AspectRatio);

        m_Target.Orthographic = EditorGUILayout.Toggle("Orthographic", m_Target.Orthographic);

        m_Target.OrthographicSize = EditorGUILayout.FloatField("Orthographic Size", m_Target.OrthographicSize);

        m_Target.material = EditorGUILayout.ObjectField("Material", m_Target.material, typeof(Material), false) as Material;
    }
}
