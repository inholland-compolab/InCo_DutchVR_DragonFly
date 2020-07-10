using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {
	public Vector3 startPos;
    public Vector3 offset;
    public Vector3 rotation;
	public float sensitivity = 0.01f;
	public Transform transformToRotate;
	public bool _canMove = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {		
		if(_canMove){
			if (Input.touchCount == 1) {
				Touch touch = Input.GetTouch(0);

					// Handle finger movements based on TouchPhase
				switch (touch.phase)
				{
					//When a touch has first been detected, change the message and record the starting position
					case TouchPhase.Began:
						startPos = Input.mousePosition;
						break;

					//Determine if the touch is a moving touch
					case TouchPhase.Moved:
						Rotate();
						break;

					case TouchPhase.Ended:
						// Report that the touch has ended when it ends
						break;
				}
			}else if(Input.GetMouseButton(0)){
				if(Input.GetMouseButtonDown(0)){
					startPos = Input.mousePosition;
				}
				Rotate();
			}
		}
	}
	public void Rotate(){
		Debug.Log("Rotating!");
		// offset
		offset = (Input.mousePosition - startPos);
		
		// apply rotation
		rotation.y = -(offset.x /*+ -offset.y*/) * sensitivity;
		
		// rotate
		transformToRotate.Rotate(rotation);
		
		// store mouse
		startPos = Input.mousePosition;
		// activeRotator.transform

	}
}
