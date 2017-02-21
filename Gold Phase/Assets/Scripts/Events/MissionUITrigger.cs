using UnityEngine;
using System.Collections;

public class MissionUITrigger : MonoBehaviour {

    public GameObject MissionUI;

    void Start() {
        MissionUI.SetActive(false);
    }

    void OnTriggerEnter() {
        MissionUI.SetActive(true);
    }

    void OnTriggerExit() {
        MissionUI.SetActive(false);
    }
}
