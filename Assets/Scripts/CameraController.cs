using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
	public float rotationSpeed = 10;
	public float zoomSpeed = 2;
	public float minZoom = 1;
	public float maxZoom = 20;
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
		cam.orthographicSize = minZoom;
    }
	
    void Update() {
		Vector3 forward = new Vector3(transform.forward.x, 0.0f, transform.forward.z);
		Vector3 right = new Vector3(transform.right.x, 0.0f, transform.right.z);
		Vector3 newPos = (deltaPos.y * forward) + (deltaPos.x * right);
		transform.position += newPos.normalized;
		axisRotation += newPos.normalized;
    }

	public void OnRotateCam(InputValue value) {
		if(!Mouse.current.rightButton.isPressed) {
			return;
		}
		
		Vector2 deltaMouse = value.Get<Vector2>();
		
		Debug.Log("Cam rotated : " + deltaMouse);
		
	    angle += (deltaMouse.x * 0.01f) % 360;
		Vector3 circle = new Vector3(Mathf.Cos(angle) * radius, transform.position.y, Mathf.Sin(angle) * radius);
		circle += new Vector3(axisRotation.x, 0, axisRotation.z);
		//Vector3 centre = transform.position - circle;
		transform.position = circle;
		
		Vector3 lpos = (axisRotation + cameraTilt * Vector3.up) - transform.position;
		transform.rotation = Quaternion.LookRotation(lpos);
		//cam.transform.LookAt(board);
	}

	public void OnZoomCam(InputValue value) {
		float deltaScroll = value.Get<float>();
		Debug.Log("Cam Zoom : " + deltaScroll);

		float newSize = cam.orthographicSize + (zoomSpeed * deltaScroll);
		cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
	}

	public void OnMoveCam(InputValue value) {
		deltaPos = value.Get<Vector2>();
		Debug.Log("Cam Moved: " + deltaPos);
	}
}
