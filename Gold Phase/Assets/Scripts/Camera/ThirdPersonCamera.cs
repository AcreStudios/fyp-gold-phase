using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
	public static ThirdPersonCamera instance;
	public static ThirdPersonCamera GetInstance()
	{
		return instance;
	}
	
	// Components
	private Transform trans;
	private PlayerInputManager playerInput;

	[Header("Camera Setup")]
	public bool AutoMainCamera = true;
	public bool AutoTargetPlayer = true;
	public Transform Target;
	public CursorLockMode cursorBehavior = CursorLockMode.None;

	[Header("Pan Settings")]
	public float LookSensitivity = 10f;
	private float yaw, pitch;
	public Vector2 LookMinMaxAngle = new Vector2(-40f, 85f);

	[Header("Move Settings")]
	public float MoveSmoothTime = .015f;
	private Vector3 moveSmoothVelocity;
	private Vector3 currentPosition;
	public float RotateSmoothTime = .06f;
	private Vector3 rotationSmoothVelocity;
	private Vector3 currentRotation;

	[Header("Positioning")]
	public float DistanceFromTarget = 2f;
	public float PivotHeight = 1.3f;
	private Transform cameraPivot;

	void Awake() 
	{
		// Cache
		trans = GetComponent<Transform>();

		// Implement singleton
		instance = this;
	}
	
	void Start() 
	{
		// Get manager
		playerInput = PlayerInputManager.GetInstance();

		// Init
		SetupCamera();
	}
	
	void LateUpdate() 
	{
		RotateCamera();
		MoveCamera();
	}

	private void RotateCamera() 
	{
		yaw += playerInput.mouseX * LookSensitivity;
		pitch -= playerInput.mouseY * LookSensitivity;
		pitch = Mathf.Clamp(pitch, -Mathf.Abs(LookMinMaxAngle.x), LookMinMaxAngle.y);

		Vector3 targetRot = new Vector3(pitch, yaw);
		currentRotation = Vector3.SmoothDamp(currentRotation, targetRot, ref rotationSmoothVelocity, RotateSmoothTime);
		trans.eulerAngles = currentRotation;
	}

	private void MoveCamera() 
	{
		Vector3 targetPos = Target.position - trans.forward * DistanceFromTarget;
		currentPosition = Vector3.SmoothDamp(currentPosition, targetPos, ref moveSmoothVelocity, MoveSmoothTime);
		trans.position = currentPosition;
	}

	private void SetupCamera() 
	{
		// Find player and create camera pivot
		if(AutoTargetPlayer && !Target)
		{
			Transform player = GameObject.FindGameObjectWithTag("Player").transform;
			if(player)
			{
				// Create a camera pivot in player
				GameObject camPivot = new GameObject();
				camPivot.name = "Camera Pivot";

				cameraPivot = camPivot.transform;
				cameraPivot.transform.SetParent(player);
				cameraPivot.transform.localPosition = new Vector3(0f, PivotHeight, 0f);
				Target = cameraPivot.transform;
			}
			else
				Debug.LogWarning("There is no GameObject in scene tagged as Player!");
		}

		// Auto parent main camera to camera pivot
		if(AutoMainCamera)
		{
			Transform mainCam = Camera.main.transform;
			mainCam.SetParent(trans);
			mainCam.transform.position = new Vector3();
			mainCam.transform.rotation = new Quaternion();
		}

		Cursor.lockState = cursorBehavior;
	}
}
