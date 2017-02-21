using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeLookCamera : MonoBehaviour
{
	// Components
	Transform trans;

	public CursorLockMode CursorBehavior = CursorLockMode.Locked;

	public float LookSpeed = 50f;
	public float MoveSpeed = 25f;

	float rotationX = 0f;
	float rotationY = 0f;
	Vector3 targetPosition;

	void Awake()
	{
		// Cache components
		trans = GetComponent<Transform>();
	}

	void Start()
	{
		// Init
		targetPosition = trans.position;
		rotationX = trans.localEulerAngles.y;
		rotationY = trans.localEulerAngles.x;

		Cursor.lockState = CursorBehavior;
	}

	void Update()
	{
		// If shift key is pressed
		float speed = (Input.GetKey(KeyCode.LeftShift)) ? MoveSpeed * 2f : MoveSpeed;

		// Get and clamp inputs
		rotationX += Input.GetAxis("Mouse X") * Time.deltaTime * LookSpeed;
		rotationY += Input.GetAxis("Mouse Y") * Time.deltaTime * LookSpeed;
		rotationY = Mathf.Clamp(rotationY, -360f, 360f);

		// Rotate
		trans.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
		trans.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

		// Target pos for X/Z axes
		targetPosition += trans.forward * speed * Time.deltaTime * Input.GetAxis("Vertical");
		targetPosition += trans.right * speed * Time.deltaTime * Input.GetAxis("Horizontal");

		// Target pos for Y axis
		targetPosition += Vector3.up * speed * Time.deltaTime * (Input.GetKey(KeyCode.E) ? 1f : 0f);
		targetPosition -= Vector3.up * speed * Time.deltaTime * (Input.GetKey(KeyCode.Q) ? 1f : 0f);

		// Move
		trans.position = Vector3.Lerp(transform.position, targetPosition, .5f);
	}
}
