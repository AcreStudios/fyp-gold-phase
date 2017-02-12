using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CAM_Flythrough : MonoBehaviour 
{
	public CursorLockMode cMode;

	public float lookSpeed = 50.0f;
	public float moveSpeed = 25.0f;

	float _rotationX = 0.0f;
	float _rotationY = 0.0f;
	Vector3 _targetPosition;
			
	void Start()
	{
		_targetPosition = transform.position;
		_rotationX = transform.localEulerAngles.y;
		_rotationY = transform.localEulerAngles.x;

		Cursor.lockState = cMode;
	}

	void Update()
	{
		_rotationX += Input.GetAxis("Mouse X") * Time.deltaTime * lookSpeed;
		_rotationY += Input.GetAxis("Mouse Y") * Time.deltaTime * lookSpeed;
		_rotationY = Mathf.Clamp(_rotationY, -360f, 360f);

		transform.localRotation = Quaternion.AngleAxis(_rotationX, Vector3.up);
		transform.localRotation *= Quaternion.AngleAxis(_rotationY, Vector3.left);

		_targetPosition += transform.forward * moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");
		_targetPosition += transform.right * moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");

		_targetPosition += Vector3.up * moveSpeed * Time.deltaTime * (Input.GetKey(KeyCode.E) ? 1.0f : 0.0f);
		_targetPosition -= Vector3.up * moveSpeed * Time.deltaTime * (Input.GetKey(KeyCode.Q) ? 1.0f : 0.0f);

		transform.position = Vector3.Lerp(transform.position, _targetPosition, 0.5f);
	}
}
