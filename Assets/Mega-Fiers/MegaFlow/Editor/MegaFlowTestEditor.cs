
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(MegaFlowTest))]
public class MegaFlowTestEditor : Editor
{
	public override void OnInspectorGUI()
	{
		MegaFlowTest mod = (MegaFlowTest)target;

#if !UNITY_5 && !UNITY_2017 && !UNITY_2018 && !UNITY_2019
		EditorGUIUtility.LookLikeControls();
#endif

		float size = EditorGUILayout.FloatField("Size", mod.size);
		if ( size != mod.size )
		{
			mod.size = size;
			mod.MakeCube();
		}

		mod.x = EditorGUILayout.Slider("X", mod.x, 0.0f, 1.0f);
		mod.y = EditorGUILayout.Slider("Y", mod.y, 0.0f, 1.0f);
		mod.z = EditorGUILayout.Slider("Z", mod.z, 0.0f, 1.0f);

		if ( GUI.changed )
		{
			EditorUtility.SetDirty(target);
		}
	}

	[DrawGizmo(GizmoType.Selected)]	// | GizmoType.NotSelected)]
	static void RenderGizmo(MegaFlowTest flow, GizmoType gizmoType)
	{
		flow.DrawGizmo();
	}
}
