using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// Components
	private Transform trans;
	private Animator anim;
	private CharacterController charController;
	private PlayerInputManager playerInput;
	private ThirdPersonCamera tpCam;

	[Header("Movement Settings")]
	public float WalkSpeed = 2f;
	public float RunSpeed = 5f;
	public float SpeedSmoothTime = .1f;
	private float speedSmoothVelocity;
	private float currentSpeed;
	public float TurnSmoothTime = .2f;
	private float turnSmoothVelocity;
	private Vector3 currentRotation;
	public float gravity = -9.81f;
	private float velocityY;

	// Inputs
	private float horizontal, vertical;
	private bool leftShift;

	void Awake() 
	{
		// Cache
		trans = GetComponent<Transform>();
		anim = GetComponent<Animator>();
		charController = GetComponent<CharacterController>();
	}

	void Start()
	{
		// Get manager
		playerInput = PlayerInputManager.GetInstance();
		tpCam = ThirdPersonCamera.GetInstance();
	}
	
	void Update() 
	{
		MovePlayer();
	}

	private void MovePlayer() // Movement logic 
	{
		Vector2 input = new Vector2(playerInput.horizontal, playerInput.vertical);
		Vector2 inputDir = input.normalized;

		// Get target rotation
		if(inputDir != Vector2.zero)
		{
			float targetRot = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + tpCam.transform.eulerAngles.y;
			currentRotation = Vector3.up * Mathf.SmoothDampAngle(currentRotation.y, targetRot, ref turnSmoothVelocity, TurnSmoothTime);
			trans.eulerAngles = currentRotation;
		}

		// Get target speed
		bool running = playerInput.leftShift;
		float targetSpeed = ((running) ? RunSpeed : WalkSpeed) * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, SpeedSmoothTime);

		// Add gravity
		if(CheckGrounded())
			velocityY = 0f;
		else
			velocityY += gravity * Time.deltaTime;

		// Move character
		Vector3 velocity = trans.forward * currentSpeed + Vector3.up * velocityY;
		charController.Move(velocity * Time.deltaTime);
		currentSpeed = new Vector2(charController.velocity.x, charController.velocity.z).magnitude;

		// Animate character
		float runAnimPercent = currentSpeed / RunSpeed;
		float walkAnimPercent = currentSpeed / WalkSpeed * .5f;
		float forward = ((running) ? runAnimPercent : walkAnimPercent) * inputDir.y;
		float strafe = ((running) ? runAnimPercent : walkAnimPercent) * inputDir.x;

		anim.SetFloat("forward", forward, SpeedSmoothTime, Time.deltaTime);
		anim.SetFloat("strafe", strafe, SpeedSmoothTime, Time.deltaTime);
	}

	private bool CheckGrounded() // Spherecasting downwards to check ground 
	{
		RaycastHit hit;
		Vector3 origin = charController.center;
		Vector3 dir = Vector3.down;

		if(Physics.SphereCast(origin, charController.radius, dir, out hit, charController.height * .5f))
			return true;
		else
			return false;
	}
}
