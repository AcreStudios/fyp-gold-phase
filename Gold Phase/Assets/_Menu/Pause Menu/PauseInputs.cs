using UnityEngine;
using System.Collections;

public class PauseInputs : MonoBehaviour {

    public bool escKey;
    public string escKeyString;

    public GameObject PausePrefab;
    public GameObject PauseTrigger;


    void Awake() {
        escKeyString = "escKeyK";
    }
	// Use this for initialization
	void Start () {
        PausePrefab.SetActive(false);
        PauseTrigger.SetActive(true);
	}

	// Update is called once per frame
	void Update () {
        HandleInput();
        PauseMenuTrigger();
	}

    private void HandleInput() {
        escKey = Input.GetButtonDown(escKeyString);
    }

    private void PauseMenuTrigger() {
        if(escKey) {
            Time.timeScale = 0;
            PausePrefab.SetActive(true);
            PauseTrigger.SetActive(false);
        }
    }
}
