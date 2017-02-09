using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollDeath : MonoBehaviour
{
	// Components
	private Animator animator;
	private Collider[] colliderArray;
	private Rigidbody[] rigidbodyArray;

	void Awake()
	{
		animator = transform.parent.GetComponent<Animator>();
		colliderArray = GetComponentsInChildren<Collider>();
		rigidbodyArray = GetComponentsInChildren<Rigidbody>();
	}

	void Start()
	{
		// Init
		ToggleRagdoll(true);
	}

	public void ToggleRagdoll(bool state = false) // Toggle colliders and rigidbodies
	{
		if(!animator || colliderArray.Length == 0 || rigidbodyArray.Length == 0) return;

		animator.enabled = state;

		//foreach(Collider col in colliderArray)
		//	col.isTrigger = state;

		foreach(Rigidbody rb in rigidbodyArray)
			rb.isKinematic = state;
	}
}
