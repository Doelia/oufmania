﻿using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour {
	public Transform rond;
	public float x;
	public float y;
	public float minSize;
	public Transform north;
	public Transform south;
	public Transform east;
	public Transform west;

	public float pauseDuration = 0;

	private float elapsedTime;

	private bool started = false;
	private float cpt = 0;
	public float reduceFactor = 0.01f;
	
	void Start () {
		if(rond != null) {
			rond.position = new Vector3(x, y, -9);
			ignoreCollisions();
			this.gameObject.SetActive(false);
		}
	}

	void ignoreCollisions() {
		Physics2D.IgnoreCollision(north.GetComponent<BoxCollider2D>(), east.GetComponent<BoxCollider2D>());
		Physics2D.IgnoreCollision(north.GetComponent<BoxCollider2D>(), west.GetComponent<BoxCollider2D>());
		Physics2D.IgnoreCollision(south.GetComponent<BoxCollider2D>(), east.GetComponent<BoxCollider2D>());
		Physics2D.IgnoreCollision(south.GetComponent<BoxCollider2D>(), west.GetComponent<BoxCollider2D>());
		/*Physics2D.IgnoreCollision(south.GetComponent<BoxCollider2D>(), north.GetComponent<BoxCollider2D>());
		Physics2D.IgnoreCollision(east.GetComponent<BoxCollider2D>(), west.GetComponent<BoxCollider2D>());*/
	}
	
	void Update () {
		if(rond != null) {
			ignoreCollisions();

			if ((cpt*reduceFactor <= minSize) ||
			    (elapsedTime > pauseDuration && cpt * reduceFactor < 1 - (1 * reduceFactor))) {
				cpt++;
				int force = 1000;
				Vector3 forceDirection = new Vector3(0, -force, 0);
				north.GetComponent<Rigidbody2D>().velocity = forceDirection;
				
				forceDirection = new Vector3(0, force, 0);
				south.GetComponent<Rigidbody2D>().velocity = forceDirection;
				
				forceDirection = new Vector3(force, 0, 0);
				east.GetComponent<Rigidbody2D>().velocity = forceDirection;
				
				forceDirection = new Vector3(-force, 0, 0);
				west.GetComponent<Rigidbody2D>().velocity = forceDirection;
				
				rond.localScale = new Vector3(1 - reduceFactor * cpt,
				                              1 - reduceFactor * cpt,
				                              1);
			} else {
				elapsedTime += Time.deltaTime;
				/*Vector3 forceDirection = new Vector3(0, 0, 0);
				north.GetComponent<Rigidbody2D>().velocity = forceDirection;
				south.GetComponent<Rigidbody2D>().velocity = forceDirection;
				east.GetComponent<Rigidbody2D>().velocity = forceDirection;
				west.GetComponent<Rigidbody2D>().velocity = forceDirection;*/
			}

		}
	}
}
