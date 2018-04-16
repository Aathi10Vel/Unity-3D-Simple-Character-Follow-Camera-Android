using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class FollowCamera : MonoBehaviour {

	public Transform targetPlayer;
	Camera playerCamera;
	public PlayerMove getActions; // this will get the actions from the PLayermoveScript
	public float cameraDistance=5.0f;
	public float cameraSensitivity = 3.0f;
	public float camSmooth=2.0f;

	public float charSmooth=5.0f;
	public float smoothVel=10.0f;

	public Vector2 mouseLook;
	public Vector2 rotateVert;

	public Quaternion camRotateX;
	public Quaternion camRotateXY;

	public Vector3 lookOffset;
	public Vector3 camPosition;

	public Button switchButton;
	public Text dispText;
	public bool camSwitch;


	// Use this for initialization
	void Start () {
		camSwitch = true;
		getActions = FindObjectOfType<PlayerMove> ();
		playerCamera = GetComponent<Camera> ();
		lookOffset = playerCamera.transform.position - targetPlayer.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (camSwitch == true) {
			dispText.text = "TPS";
		}
		camControl ();	
	}

	public void camControl(){
		var mouseDir = new Vector2 (CrossPlatformInputManager.GetAxisRaw ("Mouse X"), CrossPlatformInputManager.GetAxisRaw ("Mouse Y"));
		mouseDir = Vector2.Scale (mouseDir, new Vector2 (cameraSensitivity* camSmooth, cameraSensitivity* camSmooth));
		rotateVert.x = Mathf.Lerp (rotateVert.x, mouseDir.x, 1f / camSmooth);
		rotateVert.y = Mathf.Lerp (rotateVert.y, mouseDir.y, 1f / camSmooth);
		mouseLook += rotateVert;
		mouseLook.y = Mathf.Clamp (mouseLook.y, -40, 40);

		camRotateX = Quaternion.Euler (0, mouseLook.x, 0);
		camRotateXY = Quaternion.Euler (-mouseLook.y, mouseLook.x, 0);

		if (getActions.actionPerform == true) {
			// This can smooth the Player Turn
			targetPlayer.eulerAngles = Vector3.up * Mathf.SmoothDampAngle (targetPlayer.transform.eulerAngles.y, camRotateX.eulerAngles.y, ref smoothVel, charSmooth * Time.deltaTime);
		} else {
			Vector3 lookPoint = targetPlayer.transform.position;
			playerCamera.transform.LookAt (lookPoint + lookOffset);
		}

		switchButton.onClick.AddListener (switchingCamera);

		switch (camSwitch) {
		case true:
			{
				camPosition=targetPlayer.position-(camRotateX*new Vector3(0,0,0)+new Vector3(0,-lookOffset.y,-lookOffset.z));
				break;
			}
		case false:
			{
				camPosition=targetPlayer.position-(camRotateXY*Vector3.forward*cameraDistance+new Vector3(0,-lookOffset.y,0));
				break;
			}
		default:
			{
				Debug.Log ("Not Working");
				break;
			}
		}

		playerCamera.transform.rotation = camRotateXY;
		playerCamera.transform.position = camPosition;

	}

	//This Method Can Call for Switching Camera by the UI Button inside the CamControl
	public void switchingCamera(){
		if (camSwitch == true) {
			camSwitch = false;
			dispText.text = "FPS";
		} else {
			camSwitch = true;
			dispText.text = "TPS";
		}
	}
}
