using UnityEngine;
using System.Collections;

public class L004InspectorSelectComponent : MonoBehaviour {
	
	[ContextMenu("SelectMe")]
	void SelectMe()
	{
		UnityEditor.Selection.objects = new Object[] { this };
	}
}
