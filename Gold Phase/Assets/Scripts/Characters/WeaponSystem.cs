using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponSystem : MonoBehaviour
{
	// Components
	private Animator anim;
	private PlayerInputManager playerInput;
	private ThirdPersonCamera tpCam;
	private CombatUI combatUI;
	private Animator crosshairAnim;

	[Header("Debug")]
	public bool DebugAim = false;
	public bool DebugWeaponRay = true;
	public bool DebugAimRay = true;
	public bool DebugShootRay = true;

	[Header("Weapon Setup")]
	public Transform BulletSpawnPoint;
	public BulletImpact[] BulletImpacts = new BulletImpact[1];

	[Header("Weapon Settings")]
	public float Damage = 30f;
	public float HeadshotMultiplier = 3f;
	public float LimbshotMultiplier = .5f;
	public float AttackRate = .3f;
	public float AttackRange = 100f;
	public bool AutoReload = false;
	public float ReloadDuration = 2f;
	public bool UseBulletSpread = true;
	public float BulletSpreadAmount = .02f;
	public LayerMask BulletLayer;

	private RaycastHit hit;
	private bool aiming;
	private bool shootCooldown;
	private bool reloading;
	private bool empty;

	[Header("Ammo Settings")]
	public int CurrentAmmo = 12;
	public int MaxAmmo = 12;

	[Header("Model Settings")]
	public bool DebugSpine = false;
	public Transform DebugSpineTrans;
	public float SpineOffsetX = 5f;
	public float SpineOffsetY = 5f;


	void Awake() 
	{
		// Cache
		anim = GetComponent<Animator>();
	}
	
	void Start() 
	{
		// Get manager
		playerInput = PlayerInputManager.GetInstance();
		tpCam = ThirdPersonCamera.GetInstance();
		combatUI = CombatUI.GetInstance();
	}
	
	void Update() 
	{
		UpdateWeaponLogic();
	}

	void LateUpdate() 
	{
		if(aiming && DebugSpine)
			RotateSpine();
	}

	private void UpdateWeaponLogic() 
	{
		// Aim
		aiming = playerInput.RMB || DebugAim;
		if(aiming)
			Aim();

		anim.SetBool("aim", aiming);
		combatUI.ToggleUI((aiming) ? true : false);

		// Reload
		if(playerInput.reloadKey)
			Reload();
	}

	private void Aim() 
	{
		// Debug
		if(BulletSpawnPoint && aiming && (DebugAim || DebugWeaponRay))
		{
			Transform bPoint = BulletSpawnPoint;
			Debug.DrawRay(bPoint.position, bPoint.forward * AttackRange, Color.red);
		}

		// Raycast forward
		Transform mainCamTrans = tpCam.camTrans;
		Vector3 start = mainCamTrans.position;
		Vector3 dir = mainCamTrans.forward;

		if(DebugAimRay)
			Debug.DrawRay(start, dir * AttackRange, Color.blue);

		if(Physics.Raycast(start, dir, out hit, AttackRange, BulletLayer))
		{
			// Shoot
			if(playerInput.LMB)
				Fire(start, dir, hit);
		}

		// Rotate model
		RotateSpine();
	}

	private void Fire(Vector3 start, Vector3 dir, RaycastHit hit) // Fire weapon 
	{
		Shoot(start, dir, hit);

		// Auto reload?
		if(AutoReload && CurrentAmmo == 0)
		{
			Reload();
			return;
		}

		if(CurrentAmmo > 0) return;

		// Play empty gun sound
		//if(soundManager)
		//{
		//	if(!_empty && activeWeapon.soundSettings.emptySound && activeWeapon.soundSettings.audioSrc)
		//	{
		//		soundManager.PlaySound(activeWeapon.soundSettings.audioSrc,
		//			activeWeapon.soundSettings.emptySound,
		//			true,
		//			activeWeapon.soundSettings.pitchMin,
		//			activeWeapon.soundSettings.pitchMax);

		//		StartCoroutine(FinishEmptyFire());
		//	}
		//}
	}

	private IEnumerator FinishEmptyFireSound() 
	{
		empty = true;
		yield return new WaitForSeconds(.4f);
		empty = false;
	}

	private void Shoot(Vector3 start, Vector3 dir, RaycastHit hit) // Shoot bullet 
	{
		if(CurrentAmmo <= 0 || shootCooldown || !BulletSpawnPoint)
			return;

		// Bullet spread
		dir += (Vector3)UnityEngine.Random.insideUnitCircle * BulletSpreadAmount;

		if(DebugShootRay)
			Debug.DrawRay(start, dir * AttackRange, Color.green, 2f);

		if(Physics.Raycast(start, dir, out hit, AttackRange, BulletLayer))
		{
			Health hp = hit.transform.root.GetComponent<Health>();
			if(hp && hp.isActiveAndEnabled)
			{
				combatUI.TriggerHitEnemy();

				// Hit different body part
				print(hit.collider);
				if(hit.collider is SphereCollider)
					hp.ReceiveDamage(Damage * HeadshotMultiplier);
				else if(hit.collider is CapsuleCollider)
					hp.ReceiveDamage(Damage * LimbshotMultiplier);
				else
					hp.ReceiveDamage(Damage);
			}
			else
				Debug.Log("Did not hit an enemy. Hit " + hit.transform.name + " instead!");

			AIFunctions ai = hit.transform.GetComponent<AIFunctions>();
			if(ai && ai.isActiveAndEnabled)
				ai.DamageRecieved();

			BulletImpactLogic(hit);

			
		}

		// Muzzle flash
		//if(MuzzleFlashPrefab)
		//{
		//	Vector3 bSpawnPos = weaponSettings.bulletSpawnPoint.position;
		//	GameObject mFlash = (GameObject)Instantiate(weaponSettings.muzzleFlashPrefab, bSpawnPos, Quaternion.identity);
		//	Transform mFlashTrans = mFlash.transform;
		//	mFlashTrans.SetParent(weaponSettings.bulletSpawnPoint);
		//	Destroy(mFlash, .5f);
		//}

		// Sounds
		//if(SoundManager.instance && soundSettings.audioSrc)
		//{
		//	if(soundSettings.gunshotSounds.Length > 0)
		//	{
		//		SoundManager.instance.PlaySoundOnce(weaponSettings.bulletSpawnPoint.position,
		//		soundSettings.gunshotSounds[Random.Range(0, soundSettings.gunshotSounds.Length)],
		//		2f,
		//		true,
		//		soundSettings.pitchMin,
		//		soundSettings.pitchMax);
		//	}
		//}

		// Shoot cooldown
		StartCoroutine(FinishShooting());

		// Update ammo
		CurrentAmmo--;

		if(combatUI)
			combatUI.UpdateAmmobar(CurrentAmmo);
	}

	private IEnumerator FinishShooting () // Loads next bullet; Cooldown interval between bullets
	{
		shootCooldown = true;
		yield return new WaitForSeconds(AttackRate);
		shootCooldown = false;
	}

	private void BulletImpactLogic(RaycastHit hit) // Logic when a bullet hits something 
	{
		if(BulletImpacts.Length == 0) return;
		
		foreach(BulletImpact bType in BulletImpacts)
		{
			foreach(Material mat in bType.Mats)
			{
				Renderer rend = hit.collider.GetComponent<Renderer>();

				if(!rend || rend.material != mat) return;

				// Decal prefabs
				if(bType.ImpactPrefab)
				{
					Vector3 hitPoint = hit.point;
					Quaternion lookRot = Quaternion.LookRotation(hit.normal);
					GameObject decal = Instantiate(bType.ImpactPrefab, hitPoint, lookRot);
					Transform decalTrans = decal.transform;
					Transform hitTrans = hit.transform;
					decalTrans.SetParent(hitTrans);
					Destroy(decal, 20f);
				}

				// Sound
				//SoundManager.GetInstance().PlaySoundOnce(hit.point, bType.impactSounds[Random.Range(0, bType.impactSounds.Length)]);
			}
		}
	}

	private void Reload() 
	{
		if(reloading || CurrentAmmo == MaxAmmo) return;

		// Play reload sound
		//if(soundManager)
		//{
		//	if(activeWeapon.soundSettings.reloadSound && activeWeapon.soundSettings.audioSrc)
		//	{
		//		soundManager.PlaySound(activeWeapon.soundSettings.audioSrc,
		//			activeWeapon.soundSettings.reloadSound,
		//			true,
		//			activeWeapon.soundSettings.pitchMin,
		//			activeWeapon.soundSettings.pitchMax);
		//	}
		//}

		StartCoroutine(FinishReloading());
	}

	private IEnumerator FinishReloading() 
	{
		reloading = true;
		yield return new WaitForSeconds(ReloadDuration);
		LoadAmmo();
		reloading = false;
	}

	private void LoadAmmo() 
	{
		CurrentAmmo = MaxAmmo;

		if(combatUI)
			combatUI.UpdateAmmobar(CurrentAmmo);
	}

	private void RotateSpine() // Makes the character spine face center 
	{
		Vector3 newRot = new Vector3(tpCam.camTrans.parent.localPosition.x + SpineOffsetX, tpCam.camTrans.parent.localPosition.y + SpineOffsetY, -tpCam.newY);

		DebugSpineTrans.Rotate(newRot);
	}
}

[Serializable]
public class BulletImpact
{
	public string ImpactType;
	public GameObject ImpactPrefab;
	public Material[] Mats;
	public AudioClip[] ImpactSounds;
}
