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

	// Inputs
	[HideInInspector]
	public float horizontal, vertical;
	[HideInInspector]
	public bool leftShift;
	[HideInInspector]
	public float mouseX, mouseY;
	[HideInInspector]
	public bool LMB, RMB, MMB, reloadKey;


	void Awake() 
	{
		// Implement singleton
		if(!instance)
			instance = this;
		else
		{
			if(instance != this)
			{
				Destroy(instance.gameObject);
				instance = this;
			}
		}
	}
	
	void Update() 
	{
		HandleInputs();
	}

	private void HandleInputs() // All inputs in game are handled here 
	{
		horizontal = Input.GetAxisRaw(InputStrings.Horizontal);
		vertical = Input.GetAxisRaw(InputStrings.Vertical);
		//leftShift = Input.GetButton(InputStrings.LeftShift) || Input.GetKey(KeyCode.LeftShift);
		leftShift = Input.GetKey(KeyCode.LeftShift);

		mouseX = Input.GetAxis(InputStrings.MouseX);
		mouseY = Input.GetAxis(InputStrings.MouseY);

		//LMB = Input.GetButton(InputStrings.LMB) || Input.GetMouseButton(0);
		//RMB = Input.GetButton(InputStrings.RMB) || Input.GetMouseButton(1);
		//reloadKey = Input.GetButton(InputStrings.ReloadKey) || Input.GetKey(KeyCode.R);
		LMB = Input.GetMouseButton(0);
		RMB = Input.GetMouseButton(1);
		MMB = Input.GetMouseButtonDown(2);
		reloadKey = Input.GetKeyDown(KeyCode.R);
	}

	[Serializable]
	public class InputString 
	{
		public string Horizontal = "Horizontal";
		public string Vertical = "Vertical";
		public string LeftShift = "Fire3";
		public string MouseX = "Mouse X";
		public string MouseY = "Mouse Y";
		public string LMB = "Fire1";
		public string RMB = "Fire2";
		public string ReloadKey = "Reload";
	}
}
