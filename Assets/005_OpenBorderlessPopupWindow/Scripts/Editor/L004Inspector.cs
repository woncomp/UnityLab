using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(L004Component))]
public class L004Inspector : Editor {

	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Open Window"))
		{
			L004Window.OpenWindow();
		}
	}
}
