﻿using UnityEngine;
using System.Collections;


public class tactilEvent : MonoBehaviour {

	public float timeBeforeLongTouch;
	public cube monCube;

	private bool touchScreen;
	private Vector2 mouvement;
	private float timeTouchTotal;


	void Start()
	{
		touchScreen = false;
		timeTouchTotal = 0F;
		mouvement = Vector2.zero;
	}


	void Update () 
	{
		if (Input.touchCount > 0) 
		{           
			switch (Input.GetTouch(0).phase) 
			{           
				case TouchPhase.Began:
					touchScreen=true;
				break;  

				case TouchPhase.Ended:
					if(mouvement.magnitude>=12 && touchScreen)
					{
						monCube.action4();
					}
					else if(timeTouchTotal>=timeBeforeLongTouch)
					{
						monCube.action3();
					}
					else
					{
						monCube.action1();
					}
					timeTouchTotal = 0F;
					touchScreen=false; 
					mouvement=Vector2.zero;
				break;  

				case TouchPhase.Stationary: 
					timeTouchTotal+=Time.deltaTime;
					if(timeTouchTotal>=timeBeforeLongTouch && touchScreen)
					{
						monCube.action2();
						touchScreen=false;
					}
				break;

				case TouchPhase.Moved: 
					mouvement+=Input.GetTouch(0).deltaPosition;	
					if(mouvement.magnitude<=4)
					{
						timeTouchTotal+=Time.deltaTime;
						if(timeTouchTotal>=timeBeforeLongTouch && touchScreen)
						{
							monCube.action2();
							touchScreen=false;
						}
					}
				break;                
			}  		
		}

	}
}
