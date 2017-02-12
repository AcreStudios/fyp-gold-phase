using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BeforeMenu : MonoBehaviour {

    public GameObject DisclaimerCanvas;
    public GameObject SavingCanvas;

    public Image disclaimer;
    public Image saving;

    public string currentState;

	// Use this for initialization
	void Start () {   
        DisclaimerCanvas.SetActive(true);
        SavingCanvas.SetActive(false);

        StartCoroutine(disclaimerTime());
    }
	
	// Update is called once per frame
	void Update () {
        if ((Input.anyKeyDown) && (currentState == "Disclaimer")) {
            disclaimerClick();
        }
        if (DisclaimerCanvas.activeInHierarchy) {
            currentState = "Disclaimer";
        }
        else if(SavingCanvas.activeInHierarchy) {
            currentState = "Saving";
        }
	}

    public void nextScene() {
        SceneManager.LoadScene("Menu");
    }

    public void disclaimerClick() {
        savingTime();
    }

    IEnumerator disclaimerTime() {
        yield return new WaitForSeconds(8);
        DisclaimerCanvas.SetActive(false);

        StartCoroutine(savingTime());
    }

    IEnumerator savingTime() {
        SavingCanvas.SetActive(true);
        yield return new WaitForSeconds(5);
        nextScene();
    }
}
