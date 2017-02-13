using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInputManager : MonoBehaviour
{
	public static PlayerInputManager instance;
	public static PlayerInputManager GetInstance() 
	{
		return instance;
	}

	// Input strings
	public InputString InputStrings;

	[HideInInspector]
	public float horizontal, vertical;
	[HideInInspector]
	public bool leftShift;
	[HideInInspector]
	public float mouseX, mouseY;

	void Awake() 
	{
		// Implement singleton
		instance = this;
	}
	
	void Update() 
	{
		HandleInputs();
	}

	private void HandleInputs() // All inputs in game are handled here 
	{
		horizontal = Input.GetAxisRaw(InputStrings.Horizontal);
		vertical = Input.GetAxisRaw(InputStrings.Vertical);
		leftShift = Input.GetButton(InputStrings.LeftShift);

		mouseX = Input.GetAxis(InputStrings.MouseX);
		mouseY = Input.GetAxis(InputStrings.MouseY);
	}

	[Serializable]
	public class InputString
	{
		public string Horizontal = "Horizontal";
		public string Vertical = "Vertical";
		public string LeftShift = "Fire3";
		public string MouseX = "Mouse X";
		public string MouseY = "Mouse Y";
	}
}
