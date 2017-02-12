using UnityEngine;
using System.Collections;

public class Killurself : MonoBehaviour {

    public GameObject fadeObj;
	// Use this for initialization
	void Start () {
        StartCoroutine(fadeOuttahere());
	}

    IEnumerator fadeOuttahere() {
        yield return new WaitForSeconds(8);
        fadeObj.SetActive(false);
    }
}
