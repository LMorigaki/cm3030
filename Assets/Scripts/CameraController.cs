using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
	public float rotationSpeed = 30;
	public float zoomSpeed = 2;
	public float defaultZoom = 50; 
	public float minZoom = 20;
	public float maxZoom = 100;
	public float cameraTilt = -10;
	public float radius = 20.0f;
	public Transform board;
	
	private Camera cam;
	private float angle = 0.0f;
	private Vector3 axisRotation;
	private Vector2 deltaPos;
	
	void Start() {
		cam = GetComponent<Camera>();
		transform.LookAt(board);
		
		axisRotation = board.position;
		cam.orthographicSize = defaultZoom;
    }
	
    void Update() {
		Vector3 forward = new Vector3(transform.forward.x, 0.0f, transform.forward.z);
		Vector3 right = new Vector3(transform.right.x, 0.0f, transform.right.z);
		Vector3 newPos = (deltaPos.y * forward) + (deltaPos.x * right);
		transform.position += newPos.normalized;
		axisRotation += newPos.normalized;
    }

	public void OnRotateCam(InputValue value)
	{
		if(!Mouse.current.rightButton.isPressed)
		{
			return;
		}
		
		Vector2 deltaMouse = value.Get<Vector2>();

		cam.transform.RotateAround(board.transform.position, Vector3.up, deltaMouse.x * Time.deltaTime * rotationSpeed);
		cam.transform.RotateAround(board.transform.position, transform.right, deltaMouse.y * Time.deltaTime * rotationSpeed * -1);
	}

	public void OnZoomCam(InputValue value)
	{
		float deltaScroll = value.Get<float>();
		Vector3 v3 = Vector3.zero;
		v3.z = zoomSpeed * deltaScroll * Time.deltaTime;
		cam.transform.Translate(v3, Space.Self);
	}

	public void OnMoveCam(InputValue value) {
		deltaPos = value.Get<Vector2>();
		//Debug.Log("Cam Moved: " + deltaPos);
	}
}
