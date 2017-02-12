using UnityEngine;
using System.Collections;

public class StartScreenControllerScript : MonoBehaviour {

    public MenuScript MenuScript;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.anyKeyDown) {
            MenuScript.SelectStart();
        }
	}
}
