using UnityEngine;
using UnityEditor;
using System.Collections;

public class L004Window : EditorWindow
{
	static L004Window instance = null;

	public static void OpenWindow()
	{
		if (instance == null)
		{
			instance = (L004Window)ScriptableObject.CreateInstance<L004Window>();
			instance.position = new Rect(500, 300, 600, 400);
		}
		instance.ShowPopup();
		instance.Focus();
	}

	void Update()
	{
		if (EditorWindow.focusedWindow != this) this.Close();
	}
}
