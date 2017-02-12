using UnityEngine;
using System.Collections.Generic;

public class Inputs : MonoBehaviour {
    #region Keyboard Inputs
    public bool LMB;
    public bool keyW;
    public bool keyS;
    public bool keyArrowUp;
    public bool keyArrowDown;
    public bool keyEnter;
    public bool escKey;
    public bool keySpace;
    #endregion
    #region Gamepad Inputs
    public bool buttonA;
    public bool buttonB;
    [Range(-1f, 1f)]
    public float dPad;
    #endregion
    #region Keyboard Inputs Initialization
    public string selectK;
    public string selectJ;
    public string wKey;
    public string sKey;
    public string arrowKeyDown;
    public string arrowKeyUp;
    public string enterKey;
    public string escKeyK;
    public string spaceKey;
    #endregion
    #region Gamepad Inputs Initialization
    public string DPad;
    public string aButton;
    public string bButton;
    #endregion

    public MenuScript menuScript;
    //public GameObject video;

    void Awake() {
        menuScript = GetComponent<MenuScript>();
        selectK = "SelectK";
        selectJ = "SelectJ";
        wKey = "wKey";
        sKey = "sKey";
        arrowKeyDown = "downArrow";
        arrowKeyUp = "upArrow";
        enterKey = "enterKey";
        escKeyK = "escKeyK";

        DPad = "dPad";
        aButton = "aButton";
        bButton = "bButton";
        spaceKey = "spaceKey";
}

	// Use this for initialization
	void Start () {
        //video.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
        HandleKeyboardInput();
        HandleGamepadInput();
        MenuLogic();
	}

    private void HandleKeyboardInput() {
        LMB = Input.GetButtonDown(selectK);
        keyW = Input.GetButtonDown(wKey);
        keyS = Input.GetButtonDown(sKey);
        keyArrowDown = Input.GetButtonDown(arrowKeyDown);
        keyArrowUp = Input.GetButtonDown(arrowKeyUp);
        keyEnter = Input.GetButtonDown(enterKey);
        escKey = Input.GetButtonDown(escKeyK);
        keySpace = Input.GetButtonDown(spaceKey);
    }

    private void HandleGamepadInput() {
        buttonA = Input.GetButtonDown(aButton);
        buttonB = Input.GetButtonDown(bButton);
        dPad = Input.GetAxis(DPad);
    }

    public void MenuLogic() {
        #region W Key Handler
        if((keyW)||(keyArrowUp)||(dPad > 0)) {
            #region MenuScript
            if(menuScript.currentState == ("MainMenu: MO Settings")) {
                menuScript.MONewGame();
            }
            else if(menuScript.currentState == "MainMenu: MO Credits") {
                menuScript.MOSettings();
            }
            else if(menuScript.currentState == "MainMenu: MO Quit") {
                menuScript.MOCredits();
            }
            #endregion
            #region SettingsScript
            else if(menuScript.currentState == "SettingsMenu: MO Audio") {
                menuScript.MOGame();
            }
            else if(menuScript.currentState == "SettingsMenu: MO Video") {
                menuScript.MOAudio();
            }
            else if(menuScript.currentState == "SettingsMenu: MO Back") {
                menuScript.MOVideo();
            }
            #endregion
            #region QuitScript
            if (menuScript.currentState == "QuitMenu: MO Yes") {
                menuScript.MONo();
            }
            #endregion
        }
        #endregion
        #region S Key Handler
        else if ((keyS)||(keyArrowDown)||(dPad < 0)) {
            #region MenuScript
            if(menuScript.currentState == "Menu Screen") {
                menuScript.MOSettings();
            }
            else if(menuScript.currentState == "MainMenu: MO NewGame") {
                menuScript.MOSettings();
            }
            else if(menuScript.currentState == "MainMenu: MO Settings") {
                menuScript.MOCredits();
            }
            else if(menuScript.currentState == "MainMenu: MO Credits") {
                menuScript.MOQuit();
            }
            #endregion
            #region SettingsScript
            else if(menuScript.currentState == "Settings Screen") {
                menuScript.MOAudio();
            }
            else if(menuScript.currentState == "SettingsMenu: MO Game") {
                menuScript.MOAudio();
            }
            else if(menuScript.currentState == "SettingsMenu: MO Audio") {
                menuScript.MOVideo();
            }
            else if(menuScript.currentState == "SettingsMenu: MO Video") {
                menuScript.MOBack();
            }
            #endregion
            #region QuitScript
            else if(menuScript.currentState == "QuitMenu: MO No") {
                menuScript.MOYes();
            }
            else if(menuScript.currentState == "Quit Screen") {
                menuScript.MOYes();
            }
            #endregion
        }
        #endregion
        #region Enter Key Handler
        else if ((keyEnter)||(buttonA)) {
            #region MenuScript
            if(menuScript.currentState == "MainMenu: MO NewGame") {
                menuScript.SelectNewGame();
            }
            else if(menuScript.currentState == "MainMenu: MO Settings") {
                menuScript.SelectSettings();
            }
            else if(menuScript.currentState == "MainMenu: MO Credits") {
                menuScript.SelectCredits();
            }
            else if(menuScript.currentState == "MainMenu: MO Quit") {
                menuScript.SelectQuit();
            }
            #endregion
            #region CreditsScript
            else if (menuScript.currentState == "Credits Screen") {
                menuScript.SelectBack();
            }
            #endregion
            #region SettingsScript
            else if (menuScript.currentState == "SettingsMenu: MO Back") {
                menuScript.SelectBack();
            }
            #endregion
            #region QuitScript
            else if(menuScript.currentState == "QuitMenu: MO No") {
                menuScript.SelectNo();
            }
            else if(menuScript.currentState == "QuitMenu: MO Yes") {
                menuScript.SelectYes();
            }
            #endregion
        }
        #endregion
        #region Escape Key Handler
        else if ((escKey)||(buttonB)) {
            #region MenuScript
            if (menuScript.currentState == "Menu Screen") {
                menuScript.SelectQuit();
            }
            else if (menuScript.currentState == "MainMenu: MO NewGame") {
                menuScript.SelectQuit();
            }
            else if (menuScript.currentState == "MainMenu: MO Settings") {
                menuScript.SelectQuit();
            }
            else if (menuScript.currentState == "MainMenu: MO Credits") {
                menuScript.SelectQuit();
            }
            else if (menuScript.currentState == "MainMenu: MO Quit") {
                menuScript.SelectQuit();
            }
            #endregion
            #region CreditsScript
            else if (menuScript.currentState == "Credits Screen") {
                menuScript.SelectBack();
            }
            #endregion
            #region SettingsScript
            else if (menuScript.currentState == "Settings Screen") {
                menuScript.SelectBack();
            }
            else if (menuScript.currentState == "SettingsMenu: MO Game") {
                menuScript.SelectBack();
            }
            else if (menuScript.currentState == "SettingsMenu: MO Audio") {
                menuScript.SelectBack();
            }
            else if (menuScript.currentState == "SettingsMenu: MO Video") {
                menuScript.SelectBack();
            }
            else if (menuScript.currentState == "SettingsMenu: MO Back") {
                menuScript.SelectBack();
            }
            #endregion
            #region QuitScript
            else if(menuScript.currentState == "Quit Screen") {
                menuScript.SelectNo();
            }
            else if(menuScript.currentState == "QuitMenu: MO No") {
                menuScript.SelectNo();
            }
            else if(menuScript.currentState == "QuitMenu: MO Yes") {
                menuScript.SelectNo();
            }
            #endregion
            #region DevScript
            else if(menuScript.currentState == "Dev Screen") {
                menuScript.SelectBack();
            }
            else if(menuScript.currentState == "Interior") {
                menuScript.SelectInterior();
            }
            else if(menuScript.currentState == "Exterior") {
                menuScript.SelectExterior();
            }
            #endregion
        }
        #endregion
        #region Space Key Handler
        if ((keySpace)&&(menuScript.currentState == "Loading Done")) {
            menuScript.loadApplication();
        }
        #endregion
        #region Misc Key Handler
        if((Input.anyKeyDown)&&(menuScript.currentState == "Start screen")) {
            menuScript.SelectStart();
        }
        #endregion
    }
}
