using UnityEngine;
using System.Collections;

public class L001Coroutine : MonoBehaviour
{
	public string componentName = "Component";

	void Awake()
	{
		Debug.Log(S("Awake"));
		StartCoroutine(Coroutine1());
		StartCoroutine(Coroutine2());
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

	IEnumerator Coroutine1()
	{
		while(true)
		{
			Debug.Log(S("Coroutine WaitForEndOfFrame"));
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator Coroutine2()
	{
		while(true)
		{
			Debug.Log(S("Coroutine WaitForFixedUpdate"));
			yield return new WaitForFixedUpdate();
		}
	}

	private string S(string message)
	{
		return string.Format("{0}/{1}: {2}", this.name, this.componentName, message);
	}
}
