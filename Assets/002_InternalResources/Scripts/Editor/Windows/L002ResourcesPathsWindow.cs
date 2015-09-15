using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System;

public class L002ResourcesPathsWindow : EditorWindow
{
	class RString
	{
		public string name;
		public string value;
	}

	class RTexture
	{
		public string name;
		public Texture2D value;
	}

	class RUnknown
	{
		public string name;
		public string type;
		public string value;
	}

	List<RString> listString = new List<RString>();
	List<RTexture> listTexture = new List<RTexture>();
	List<RUnknown> listUnknown = new List<RUnknown>();

	void GetResources()
	{
		Assembly ass = Assembly.GetAssembly(typeof(Editor));
		foreach (Type t in ass.GetTypes())
		{
			if (t.Name.EndsWith("EditorResourcesUtility"))
			{
				foreach (PropertyInfo pi in t.GetProperties())
				{
					ProcessRes(pi);
				}
			}
		}
	}

	void ProcessRes(PropertyInfo pi)
	{
		var obj = pi.GetValue(null, null);
		var type = obj.GetType();
		if(type == typeof(string))
		{
			var item = new RString();
			item.name = pi.Name;
			item.value = obj as string;
			listString.Add(item);
		}
		else if (type == typeof(Texture2D))
		{
			var item = new RTexture();
			item.name = pi.Name;
			item.value = obj as Texture2D;
			listTexture.Add(item);
		}
		else
		{
			var item = new RUnknown();
			item.name = pi.Name;
			item.type = type.Name;
			item.value = obj.ToString();
			listUnknown.Add(item);
		}
	}

	private int selected = 0;
	private GUIContent[] toolbarContents;

	void OnEnable()
	{
		titleContent = new GUIContent("EditorResourcesUtility");

		GetResources();
		toolbarContents = new GUIContent[]
		{
			new GUIContent("String"),
			new GUIContent("Texture"),
			new GUIContent("Unkown"),
		};
	}

	void OnGUI()
	{
		selected = GUILayout.Toolbar(selected, toolbarContents);
		switch (selected)
		{
			case 0:
				foreach (var item in listString)
				{
					EditorGUILayout.LabelField(item.name, item.value);
				}
				break;
			case 1:
				foreach (var item in listTexture)
				{
					EditorGUILayout.ObjectField(item.name, item.value, typeof(Texture2D), true);
				}
				break;
			case 2:
				foreach (var item in listUnknown)
				{
					EditorGUILayout.LabelField(item.name, string.Format("[{0}]{1}", item.type, item.value));
				}
				break;
			default:
				break;
		}
	}
}
