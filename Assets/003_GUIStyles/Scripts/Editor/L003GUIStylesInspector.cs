using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(L003GUIStyles))]
public class L003GUIStylesInspector : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("EditorStyleViewer", GUILayout.Height(40)))
		{
			EditorStyleViewer.Init();
		}
	}
}
