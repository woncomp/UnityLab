using UnityEngine;
using System.Collections;

public class L001Script : MonoBehaviour
{
	public string componentName = "Component";

	void Awake()
	{
		Debug.Log(S("Awake"));
	}

	void Start ()
	{
		Debug.Log(S("Start"));
	}
	
	void Update ()
	{
		Debug.Log(S("Update " + Time.deltaTime));
	}

	void FixedUpdate()
	{
		Debug.Log(S("FixedUpdate " + Time.deltaTime));
	}

	void LateUpdate()
	{
		Debug.Log(S("LateUpdate"));
	}

	void OnEnable()
	{
		Debug.Log(S("OnEnable"));
	}

	void OnDisable()
	{
		Debug.Log(S("OnDisable"));
	}

	private string S(string message)
	{
		return string.Format("{0}/{1}: {2}", this.name, this.componentName, message);
	}
}
