using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System;
using Object = UnityEngine.Object;

public class L002EditorAssetBundleWindow : EditorWindow
{
	public class RItem
	{
		public string name;
	}

	public abstract class RList<T> : List<T> where T : RItem
	{
		public bool drawSingleObject = false;

		private Vector2 scrollPosition;
		private Vector2 scrollPos2;
		private int selectedIndex;

		private T _itItem = null;
		private bool _itIsSelected = false;

		protected string currentItemLabel
		{
			get
			{
				if (_itIsSelected) return "<< " + _itItem.name + " >>";
				return _itItem.name;
			}
		}

		public virtual void Reset()
		{
			scrollPosition = Vector2.zero;
			scrollPos2 = Vector2.zero;
			selectedIndex = 0;
		}

		public void OnGUI(Rect position)
		{
			var list = this;
			EditorGUILayout.BeginHorizontal();
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			for (int i = 0; i < list.Count; ++i)
			{
				var item = list[i];
				var toggle = EditorGUILayout.ToggleLeft(item.name, selectedIndex == i);
				if (toggle)
				{
					selectedIndex = i;
					scrollPos2 = Vector2.zero;
				}
			}
			EditorGUILayout.EndScrollView();
			if(drawSingleObject)
			{
				if(selectedIndex >= 0 && selectedIndex < list.Count)
				{
					scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2);
					DrawItem(list[selectedIndex], selectedIndex, true);
					EditorGUILayout.EndScrollView();
				}
			}
			else
			{
				EditorGUILayout.BeginVertical(GUILayout.Width(300));
				var count = (int)((position.height - 30) / GetItemHeight());
				for (int i = 0; i < count; ++i)
				{
					var j = i + selectedIndex - count / 2;
					if (j >= 0 && j < list.Count)
					{
						_itItem = list[j];
						_itIsSelected = j == selectedIndex;
						DrawItem(_itItem, j, _itIsSelected);
					}
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();
		}

		public virtual int GetItemHeight() { return 20; }
		public abstract void DrawItem(T item, int index, bool isSelected);
	}

	class RShader : RItem
	{
		public Shader value;
	}

	class RShaderList : RList<RShader>
	{
		public override void DrawItem(RShader item, int index, bool isSelected)
		{
			EditorGUILayout.ObjectField(currentItemLabel, item.value, typeof(Shader), true);
		}
	}

	class RTexture : RItem
	{
		public Texture2D value;
	}

	class RTextureList : RList<RTexture>
	{
		public override int GetItemHeight()
		{
			return 64;
		}

		public override void DrawItem(RTexture item, int index, bool isSelected)
		{
			EditorGUILayout.ObjectField(currentItemLabel, item.value, typeof(Texture2D), true);
		}
	}

	class RMaterial : RItem
	{
		public Material value;
	}

	class RMaterialList : RList<RMaterial>
	{
		public override void DrawItem(RMaterial item, int index, bool isSelected)
		{
			EditorGUILayout.ObjectField(currentItemLabel, item.value, typeof(Material), true);
		}
	}

	class RGameObject : RItem
	{
		public GameObject value;
	}

	class RGameObjectList : RList<RGameObject>
	{
		public override void DrawItem(RGameObject item, int index, bool isSelected)
		{
			EditorGUILayout.ObjectField(currentItemLabel, item.value, typeof(GameObject), true);
		}
	}

	class RGUISkin : RItem
	{
		public GUISkin value;
	}

	class RGUISkinList : RList<RGUISkin>
	{
		public override void DrawItem(RGUISkin item, int index, bool isSelected)
		{
			EditorGUILayout.ObjectField(currentItemLabel, item.value, typeof(GUISkin), true);
		}
	}

	class RFont : RItem
	{
		public Font value;
	}

	class RFontList : RList<RFont>
	{
		public override void DrawItem(RFont item, int index, bool isSelected)
		{
			EditorGUILayout.ObjectField(currentItemLabel, item.value, typeof(Font), true);
		}
	}

	class RUnknown : RItem
	{
		public string type;
		public string value;
	}

	RShaderList listShader = new RShaderList();
	RTextureList listTexture = new RTextureList();
	RMaterialList listMaterial = new RMaterialList();
	RGameObjectList listGameObject = new RGameObjectList();
	RGUISkinList listGUISkin = new RGUISkinList();
	RFontList listFont = new RFontList();
	List<RUnknown> listUnknown = new List<RUnknown>();

	void GetResources()
	{
		MethodInfo method = typeof(EditorGUIUtility).GetMethod("GetEditorAssetBundle", BindingFlags.NonPublic | BindingFlags.Static);
		AssetBundle bundle = method.Invoke(null, null) as AssetBundle;

		foreach (UnityEngine.Object obj in bundle.LoadAllAssets())
		{
			Add(obj.name, obj);
		}
	}

	public void Add(string name, object obj, object tag = null)
	{
		var type = obj.GetType();
		if (type == typeof(Shader))
		{
			var item = new RShader();
			item.name = name;
			item.value = obj as Shader;
			listShader.Add(item);
		}
		else if (type == typeof(Texture2D))
		{
			var item = new RTexture();
			item.name = name;
			item.value = obj as Texture2D;
			listTexture.Add(item);
		}
		else if(type == typeof(Material))
		{
			var item = new RMaterial();
			item.name = name;
			item.value = obj as Material;
			listMaterial.Add(item);
		}
		else if (type == typeof(GameObject))
		{
			var item = new RGameObject();
			item.name = name;
			item.value = obj as GameObject;
			listGameObject.Add(item);
		}
		else if (type == typeof(GUISkin))
		{
			var item = new RGUISkin();
			item.name = name;
			item.value = obj as GUISkin;
			listGUISkin.Add(item);
		}
		else if (type == typeof(Font))
		{
			var item = new RFont();
			item.name = name;
			item.value = obj as Font;
			listFont.Add(item);
		}
		else
		{
			var item = new RUnknown();
			item.name = name;
			item.type = type.Name;
			item.value = obj.ToString();
			listUnknown.Add(item);
		}
	}

	private int selected = 0;
	private Vector2 scrollPosition;
	private GUIContent[] toolbarContents;

	void OnEnable()
	{
		titleContent = new GUIContent("GetEditorAssetBundle");

		GetResources();
		toolbarContents = new GUIContent[]
		{
			new GUIContent("Shader"),
			new GUIContent("Texture"),
			new GUIContent("Material"),
			new GUIContent("GameObject"),
			new GUIContent("GUISkin"),
			new GUIContent("Font"),
			new GUIContent("Unkown"),
		};
	}

	void OnGUI()
	{
		selected = GUILayout.Toolbar(selected, toolbarContents);
		if(GUI.changed)
		{
			scrollPosition = Vector2.zero;
		}
		switch (selected)
		{
			case 0:
				listShader.OnGUI(position);
				break;
			case 1:
				listTexture.OnGUI(position);
				break;
			case 2:
				listMaterial.OnGUI(position);
				break;
			case 3:
				listGameObject.OnGUI(position);
				break;
			case 4:
				listGUISkin.OnGUI(position);
				break;
			case 5:
				listFont.OnGUI(position);
				break;
			case 6:
				scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
				foreach (var item in listUnknown)
				{
					EditorGUILayout.LabelField(item.name, string.Format("[{0}]{1}", item.type, item.value));
				}
				EditorGUILayout.EndScrollView();
				break;
			default:
				break;
		}
	}
}
