using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput; // for Using the Default Joystick from the Standard Asset.

public class PlayerMove : MonoBehaviour {

	public CharacterController playerController;
	public float playerSpeed=10.0f;
	public float playerGravity=20.0f;
	public float vGravity;
	public bool actionPerform;
	public Vector3 moveDirections;

	// Use this for initialization
	void Start () {
		actionPerform = false;
		playerController=GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		playerMovement ();
	}

	public void playerMovement(){
		if (moveDirections.x == 0 && moveDirections.z == 0) {
			actionPerform = false;
		} else {
			actionPerform = true;
		}

		moveDirections = new Vector3 (CrossPlatformInputManager.GetAxis ("Horizontal"), 0, CrossPlatformInputManager.GetAxis ("Vertical"));
		moveDirections = transform.TransformDirection (moveDirections);
		vGravity -= playerGravity * Time.deltaTime;
		moveDirections *= playerSpeed;
		moveDirections.y = vGravity;
		playerController.Move (moveDirections * Time.deltaTime);
	}
}
