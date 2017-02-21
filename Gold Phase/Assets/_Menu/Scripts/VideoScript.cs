using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VideoScript : MonoBehaviour {

    public MovieTexture phaseLogo;
    public GameObject toDestroy;

   // public Canvas MainCanvas;
    public GameObject GOMainCanvas;

	// Use this for initialization
	void Start () {

        GOMainCanvas.SetActive(false);

        StartCoroutine(fadeout());
        GetComponent<Renderer>().material.mainTexture = phaseLogo;
        phaseLogo.Play();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator fadeout() {
        yield return new WaitForSeconds(7);
        GOMainCanvas.SetActive(true);
        toDestroy.SetActive(false);
    }
}
