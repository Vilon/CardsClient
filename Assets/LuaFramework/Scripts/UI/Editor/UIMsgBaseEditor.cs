using UnityEngine;
using UnityEditor;
using System.Collections;

[CanEditMultipleObjects]
[CustomEditor(typeof(UIMsgBase), true)]
public class UIMsgBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UIPageBase ui = (UIPageBase)target;

        EditorGUILayout.BeginHorizontal();
        ui.UseBoxCollider = GUILayout.Toggle(ui.UseBoxCollider, "阻挡", "button", GUILayout.Width(35));
        ui.UseBlackMask = GUILayout.Toggle(ui.UseBlackMask, "蒙版", "button", GUILayout.Width(35));
        ui.HideOldPages = GUILayout.Toggle(ui.HideOldPages, "关旧", "button", GUILayout.Width(35));
        ui.BackPopPrePages = GUILayout.Toggle(ui.BackPopPrePages, "弹旧", "button", GUILayout.Width(35));
        EditorGUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }
}