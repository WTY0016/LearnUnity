using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotation : MonoBehaviour
{

	[SerializeField]
	public float speed = 1;
	// Use this for initialization
	void Start () {
		
	}

	private void Update()
	{
		transform.Rotate(0, 1, 0);
	}
}
