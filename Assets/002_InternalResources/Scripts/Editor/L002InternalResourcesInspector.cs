using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Reflection;

[CustomEditor(typeof(L002InternalResources))]
public class L002InternalResourcesInspector : Editor
{

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("获取内置Resource的文件夹路径 ", GUILayout.Height(40)))
		{
			EditorWindow.GetWindow<L002ResourcesPathsWindow>().Show();
		}
		if (GUILayout.Button("获取内置Resource的 AssetBundle", GUILayout.Height(40)))
		{
			EditorWindow.GetWindow<L002EditorAssetBundleWindow>().Show();
		}
	}
}
