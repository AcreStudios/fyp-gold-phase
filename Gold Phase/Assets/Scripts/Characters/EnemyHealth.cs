using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	// Components
	private RagdollDeath ragdoll;

	[Range(0f, 100f)]
	public float CurrentHealth;

	public MonoBehaviour[] ScriptsToDisableOnDeath;

	[Header("Debug")]
	public bool DebugDamage = false;
	public bool DebugDeath = false;

	void Awake() 
	{
		ragdoll = GetComponentInChildren<RagdollDeath>();
	}
	
	protected virtual void Update() 
	{
		// Debug via inspector in game
		if(DebugDamage)
		{
			DebugDamage = false;
			ReceiveDamage();
		}

		if(DebugDeath)
		{
			DebugDeath = false;
			Die();
		}
	}

	public virtual void ReceiveDamage(int damage = 10)
	{
		// Damage logic
		CurrentHealth -= damage;
		if(CurrentHealth <= 0f)
			Die();
	}

	public virtual void Die()
	{
		if(ragdoll)
			ragdoll.ToggleRagdoll();

		if(ScriptsToDisableOnDeath.Length > 0)
		{
			foreach(MonoBehaviour s in ScriptsToDisableOnDeath)
				s.enabled = false;
		}
	}
}
