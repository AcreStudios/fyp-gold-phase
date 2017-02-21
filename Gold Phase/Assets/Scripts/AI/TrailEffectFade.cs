using UnityEngine;
using System.Collections;

public class TrailEffectFade : MonoBehaviour {

    // Use this for initialization
    public Renderer toGetColor;
    public float rateOfDecay;
    float currentA;

    void Start() {
        toGetColor = GetComponent<Renderer>();

    }
    public void ObjectActive() {
        currentA = 1;
        gameObject.SetActive(true);
    }

    void Update() {
        if (currentA > 0) {
            currentA -= rateOfDecay;
            toGetColor.material.color = new Color(1,1,1,currentA);
        } else
            gameObject.SetActive(false);
          
    }
}
