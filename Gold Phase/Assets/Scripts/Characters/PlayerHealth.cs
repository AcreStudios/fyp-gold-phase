using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;

public class PlayerHealth : EnemyHealth
{
	[Header("Screen Flash on Damage")]
	public Image RedTint;
	[Range(0f, 1f)]
	public float FlashOpacity = 1f, StayIntensity = 1f;
	public float DamageFlashSpeed = 4f;

	private Color currentRedTintTransparency = new Color(1f, 1f, 1f, 0f);

	[Header("Health Regen")]
	public float RegenSpeed = 40f;
	public float OutOfCombatDelay = 4f;
	private float outOfCombatTimer;
	private bool regeneratingHealth = false;

	[Header("Death")]
	public Grayscale GrayScreenEffect;
	public float FadeScreenDelay = .5f;
	public float FadeSpeed = .25f;
	public float ReloadSceneDelay = 1f;

	void Start()
	{
		if(GrayScreenEffect)
			GrayScreenEffect.enabled = false;
	}

	protected override void Update()
	{
		base.Update();

		LerpToTargetColor();
		CheckHealthRegenCondition();
	}

	public override void ReceiveDamage(float damage = 10)
	{
		if(dead) return;
		
		// Stops regen if received damage
		if(regeneratingHealth)
			StopCoroutine(RegenToFullHealth());

		base.ReceiveDamage(damage);

		// Damage feedback
		if(RedTint)
			RedTint.color = new Color(1f, 1f, 1f, FlashOpacity);

		// Reset regeneration cooldown timer
		outOfCombatTimer = OutOfCombatDelay;
	}

	public override void Die()
	{
		base.Die();

		if(GrayScreenEffect)
			StartCoroutine(ScreenFadeToGray());
	}

	private IEnumerator ScreenFadeToGray()
	{
		yield return new WaitForSeconds(FadeScreenDelay);

		GrayScreenEffect.enabled = true;

		float lerp = 0f;
		while(lerp < 1f)
		{
			lerp += FadeSpeed * Time.deltaTime;
			GrayScreenEffect.rampOffset = lerp;
			yield return null;
		}

		lerp = 1f;
		GrayScreenEffect.rampOffset = lerp;

		yield return new WaitForSeconds(ReloadSceneDelay);

		Debug.Log("Reloaded scene.");
	}

	private void LerpToTargetColor() // Fade out red tint to current health percentage 
	{
		currentRedTintTransparency.a = 0f + (1f - (CurrentHealth / 100f)) * StayIntensity;
		RedTint.color = Color.Lerp(RedTint.color, currentRedTintTransparency, DamageFlashSpeed * Time.deltaTime);
	}
	
	private void CheckHealthRegenCondition()
	{
		// If health is not full
		if(CurrentHealth > 0f && CurrentHealth < 100f)
		{
			// Run timer
			if(outOfCombatTimer > 0f)
				outOfCombatTimer -= Time.deltaTime;

			// Check timer
			if(outOfCombatTimer <= 0f && !regeneratingHealth)
				StartCoroutine(RegenToFullHealth());
		}
	}

	private IEnumerator RegenToFullHealth()
	{
		regeneratingHealth = true;

		while(CurrentHealth < 100f)
		{
			CurrentHealth += RegenSpeed * Time.deltaTime;
			yield return null;
		}

		CurrentHealth = 100f;
		regeneratingHealth = false;
	}
}
