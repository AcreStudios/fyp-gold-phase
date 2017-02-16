using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
	// Singleton
	public static CombatUI instance;
	public static CombatUI GetInstance()
	{
		return instance;
	}

	[Header("UI Setup")]
	public RectTransform AmmobarTrans;
	private GameObject[] ammobar;

	public GameObject CrosshairGO;
	public GameObject AmmobarGO;
	
	public Image hitIndicatorImage;
	private Animator hitAnim;


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

		// Cache
		hitAnim = hitIndicatorImage.GetComponent<Animator>();
	}

	void Start() 
	{
		// Cache ammobar children
		ammobar = new GameObject[AmmobarTrans.childCount];

		for(int i = 0; i < AmmobarTrans.childCount; i++)
		{
			ammobar[i] = AmmobarTrans.GetChild(i).gameObject;
			ammobar[i].SetActive(true);
		}

		// Turn off at start
		ToggleUI(false);
	}

	public void UpdateAmmobar(int curAmmo) // Call this to update ammobar 
	{
		foreach(GameObject b in ammobar)
			b.SetActive(false);

		for(int i = 0; i < curAmmo; i++)
			ammobar[i].SetActive(true);
	}

	public void ToggleUI(bool enabled) // Turn it on/off 
	{
		CrosshairGO.SetActive(enabled);
		AmmobarGO.SetActive(enabled);
		hitIndicatorImage.enabled = enabled;

		// Initialise HitIndicator
		hitIndicatorImage.color = new Color(1f, 1f, 1f, 0f);
	}

	public void TriggerHitEnemy() // Play hit UI anim 
	{
		hitAnim.SetTrigger("hit");
	}
}
