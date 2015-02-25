﻿using UnityEngine;
using System.Collections;

public class Gravityx : MonoBehaviour {

	public Transform center;
	public float force;

	// Use this for initialization
	void Start () {

	}

	private void attract() {
		Vector3 direction = this.center.position - this.transform.position;
		direction = direction.normalized;
		this.transform.rigidbody.AddForce (direction*force);
	}


	// Update is called once per frame
	void Update () {
		this.attract ();
	}
}
