using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShowNameInScene : MonoBehaviour
{
	public string objName = "";
	// Use this for initialization
	private void OnDrawGizmos()
	{
		Handles.Label(transform.position, gameObject.name);
	}
}
