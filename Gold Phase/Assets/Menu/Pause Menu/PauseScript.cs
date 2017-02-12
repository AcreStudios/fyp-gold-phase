using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseScript : MonoBehaviour {

    public string currentState;
    public GameObject PauseTrigger;
    public CursorLockMode cursorLocked = CursorLockMode.Locked;
    public CursorLockMode cursorUnlocked = CursorLockMode.None;

    [System.Serializable]
    public class PauseCanvas {
        public GameObject pauseCanvas;
        public GameObject resume_Red;
        public GameObject reload_Red;
        public GameObject quitTitle_Red;
        public GameObject quit_Red;
        public GameObject resume_Blk;
        public GameObject reload_Blk;
        public GameObject quitTitle_Blk;
        public GameObject quit_Blk;
        public GameObject PausePrefab;
    }
    public PauseCanvas pauseCanvas;

    [System.Serializable]
    public class ReloadCanvas {
        public GameObject reloadCanvas;
        public GameObject reloadYes_Red;
        public GameObject reloadYes_Blk;
        public GameObject reloadNo_Red;
        public GameObject reloadNo_Blk;
    }
    public ReloadCanvas reloadCanvas;

    [System.Serializable]
    public class QuitTitleCanvas {
        public GameObject quitTitleCanvas;
        public GameObject QTYes_Red;
        public GameObject QTNo_Red;
        public GameObject QTYes_Blk;
        public GameObject QTNo_Blk;
    }
    public QuitTitleCanvas quitTitleCanvas;

    [System.Serializable]
    public class QuitCanvas {
        public GameObject QCYes_Red;
        public GameObject QCNo_Red;
        public GameObject QCYes_Blk;
        public GameObject QCNo_Blk;
        public GameObject quitCanvas;
    }
    public QuitCanvas quitCanvas;

    [System.Serializable]
    public class Inputs {
        public bool LMB;
        public bool escKey;
        public bool keyW;
        public bool keyS;
        public bool keyEnter;
        public bool keyArrowUp;
        public bool keyArrowDown;
    }
    public Inputs inputs;

    [System.Serializable]
    public class InputStrings {
        public string selectK;
        public string escKey;
        public string wKey;
        public string sKey;
        public string enterKey;
        public string arrowKeyDown;
        public string arrowKeyUp;
    }
    public InputStrings inputStrings;

    //Declare an scripts that I want to access here


	// Use this for initialization
    void Awake() {
        #region input
        inputStrings.selectK = "SelectK";
        inputStrings.escKey = "escKeyK";
        inputStrings.wKey = "wKey";
        inputStrings.sKey = "sKey";
        inputStrings.enterKey = "enterKey";
        inputStrings.arrowKeyDown = "downArrow";
        inputStrings.arrowKeyUp = "upArrow";
        #endregion
    }
    void Start () {
        PauseTrigger.SetActive(true);
        Cursor.lockState = cursorUnlocked;
        #region Pause Canvas
        pauseCanvas.pauseCanvas.SetActive(true);

        pauseCanvas.resume_Red.SetActive(true);
        pauseCanvas.reload_Red.SetActive(false);
        pauseCanvas.quitTitle_Red.SetActive(false);
        pauseCanvas.quit_Red.SetActive(false);

        pauseCanvas.resume_Blk.SetActive(false);
        pauseCanvas.reload_Blk.SetActive(true);
        pauseCanvas.quitTitle_Blk.SetActive(true);
        pauseCanvas.quit_Blk.SetActive(true);

        //currentState = "Pause Screen";
        #endregion
        #region Reload Canvas
        reloadCanvas.reloadCanvas.SetActive(false);

        reloadCanvas.reloadNo_Red.SetActive(true);
        reloadCanvas.reloadYes_Red.SetActive(false);

        reloadCanvas.reloadNo_Blk.SetActive(false);
        reloadCanvas.reloadYes_Blk.SetActive(true);

        //currentState = "Reload Screen";
        #endregion
        #region Quit Title Canvas
        quitTitleCanvas.quitTitleCanvas.SetActive(false);

        quitTitleCanvas.QTNo_Red.SetActive(true);
        quitTitleCanvas.QTYes_Red.SetActive(false);

        quitTitleCanvas.QTNo_Blk.SetActive(false);
        quitTitleCanvas.QTYes_Blk.SetActive(true);

        //currentState = "Quit Title Screen";
        #endregion
        #region Quit Canvas
        quitCanvas.quitCanvas.SetActive(false);

        quitCanvas.QCNo_Red.SetActive(true);
        quitCanvas.QCYes_Red.SetActive(false);

        quitCanvas.QCNo_Blk.SetActive(false);
        quitCanvas.QCYes_Blk.SetActive(true);

        //currentState = "Quit Screen";
        currentState = "MOResume";
        #endregion
    }
	
	// Update is called once per frame
	void Update () {
        PauseMenuLogic();
        HandleInput();
	}
    #region MO Pause Canvas
    public void MOResume() {
        pauseCanvas.resume_Red.SetActive(true);
        pauseCanvas.reload_Red.SetActive(false);
        pauseCanvas.quitTitle_Red.SetActive(false);
        pauseCanvas.quit_Red.SetActive(false);
        pauseCanvas.resume_Blk.SetActive(false);
        pauseCanvas.reload_Blk.SetActive(true);
        pauseCanvas.quitTitle_Blk.SetActive(true);
        pauseCanvas.quit_Blk.SetActive(true);
        currentState = "MOResume";
    }
    public void MOReload() {
        pauseCanvas.resume_Red.SetActive(false);
        pauseCanvas.reload_Red.SetActive(true);
        pauseCanvas.quitTitle_Red.SetActive(false);
        pauseCanvas.quit_Red.SetActive(false);
        pauseCanvas.resume_Blk.SetActive(true);
        pauseCanvas.reload_Blk.SetActive(false);
        pauseCanvas.quitTitle_Blk.SetActive(true);
        pauseCanvas.quit_Blk.SetActive(true);
        currentState = "MOReload";
    }
    public void MOQuitTitle() {
        pauseCanvas.resume_Red.SetActive(false);
        pauseCanvas.reload_Red.SetActive(false);
        pauseCanvas.quitTitle_Red.SetActive(true);
        pauseCanvas.quit_Red.SetActive(false);
        pauseCanvas.resume_Blk.SetActive(true);
        pauseCanvas.reload_Blk.SetActive(true);
        pauseCanvas.quitTitle_Blk.SetActive(false);
        pauseCanvas.quit_Blk.SetActive(true);
        currentState = "MOQuitTitle";
    }
    public void MOQuit() {
        pauseCanvas.resume_Red.SetActive(false);
        pauseCanvas.reload_Red.SetActive(false);
        pauseCanvas.quitTitle_Red.SetActive(false);
        pauseCanvas.quit_Red.SetActive(true);
        pauseCanvas.resume_Blk.SetActive(true);
        pauseCanvas.reload_Blk.SetActive(true);
        pauseCanvas.quitTitle_Blk.SetActive(true);
        pauseCanvas.quit_Blk.SetActive(false);
        currentState = "MOQuit";
    }
    #endregion
    #region MO Reload Canvas
    public void MOReloadYes() {
        reloadCanvas.reloadNo_Red.SetActive(false);
        reloadCanvas.reloadYes_Red.SetActive(true);
        reloadCanvas.reloadNo_Blk.SetActive(true);
        reloadCanvas.reloadYes_Blk.SetActive(false);
        currentState = "MOReloadYes";
    }
    public void MOReloadNo() {
        reloadCanvas.reloadNo_Red.SetActive(true);
        reloadCanvas.reloadYes_Red.SetActive(false);
        reloadCanvas.reloadNo_Blk.SetActive(false);
        reloadCanvas.reloadYes_Blk.SetActive(true);
        currentState = "MOReloadNo";
    }
    #endregion
    #region MO Quit Title Canvas
    public void MOQTYes() {
        quitTitleCanvas.QTYes_Red.SetActive(true);
        quitTitleCanvas.QTNo_Red.SetActive(false);
        quitTitleCanvas.QTYes_Blk.SetActive(false);
        quitTitleCanvas.QTNo_Blk.SetActive(true);
        currentState = "MOQTYes";
    }
    public void MOQTNo() {
        quitTitleCanvas.QTYes_Red.SetActive(false);
        quitTitleCanvas.QTNo_Red.SetActive(true);
        quitTitleCanvas.QTYes_Blk.SetActive(true);
        quitTitleCanvas.QTNo_Blk.SetActive(false);
        currentState = "MOQTNo";
    }
    #endregion
    #region MO Quit Canvas
    public void MOQCYes() {
        quitCanvas.QCYes_Red.SetActive(true);
        quitCanvas.QCNo_Red.SetActive(false);
        quitCanvas.QCYes_Blk.SetActive(false);
        quitCanvas.QCNo_Blk.SetActive(true);
        currentState = "MOQCYes";
    }
    public void MOQCNo() {
        quitCanvas.QCYes_Red.SetActive(false);
        quitCanvas.QCNo_Red.SetActive(true);
        quitCanvas.QCYes_Blk.SetActive(true);
        quitCanvas.QCNo_Blk.SetActive(false);
        currentState = "MOQCNo";
    }
    #endregion
    #region Select Button Handler
    public void SelectResume() {
        Time.timeScale = 1;
        pauseCanvas.pauseCanvas.SetActive(true);
        reloadCanvas.reloadCanvas.SetActive(false);
        quitTitleCanvas.quitTitleCanvas.SetActive(false);
        quitCanvas.quitCanvas.SetActive(false);
        pauseCanvas.PausePrefab.SetActive(false);
        PauseTrigger.SetActive(true);
        Cursor.lockState = cursorLocked;
    }
    public void SelectReload() {
        pauseCanvas.pauseCanvas.SetActive(false);
        reloadCanvas.reloadCanvas.SetActive(true);
        quitTitleCanvas.quitTitleCanvas.SetActive(false);
        quitCanvas.quitCanvas.SetActive(false);
        currentState = "MOReloadNo";
    }
    public void SelectQuitTitle() {
        pauseCanvas.pauseCanvas.SetActive(false);
        reloadCanvas.reloadCanvas.SetActive(false);
        quitTitleCanvas.quitTitleCanvas.SetActive(true);
        quitCanvas.quitCanvas.SetActive(false);
        currentState = "MOQTNo";
    }
    public void SelectQuit() {
        pauseCanvas.pauseCanvas.SetActive(false);
        reloadCanvas.reloadCanvas.SetActive(false);
        quitTitleCanvas.quitTitleCanvas.SetActive(false);
        quitCanvas.quitCanvas.SetActive(true);
        currentState = "MOQCNo";
    }
    public void SelectReloadYes() {
        //Reload Game
    }
    public void SelectQuitTitleYes() {
        SceneManager.LoadScene("Menu Return");
    }
    public void SelectQuitYes() {
        Application.Quit();
    }
    public void SelectNoReload() {
        pauseCanvas.pauseCanvas.SetActive(true);
        reloadCanvas.reloadCanvas.SetActive(false);
        quitTitleCanvas.quitTitleCanvas.SetActive(false);
        quitCanvas.quitCanvas.SetActive(false);
        currentState = "MOReload";
    }
    public void SelectNoQT() {
        pauseCanvas.pauseCanvas.SetActive(true);
        reloadCanvas.reloadCanvas.SetActive(false);
        quitTitleCanvas.quitTitleCanvas.SetActive(false);
        quitCanvas.quitCanvas.SetActive(false);
        currentState = "MOQuitTitle";
    }
    public void SelectNoQC() {
        pauseCanvas.pauseCanvas.SetActive(true);
        reloadCanvas.reloadCanvas.SetActive(false);
        quitTitleCanvas.quitTitleCanvas.SetActive(false);
        quitCanvas.quitCanvas.SetActive(false);
        currentState = "MOQuit";
    }
    #endregion
    private void HandleInput() {
        inputs.LMB = Input.GetButtonDown(inputStrings.selectK);
        inputs.keyW = Input.GetButtonDown(inputStrings.wKey);
        inputs.keyS = Input.GetButtonDown(inputStrings.sKey);
        inputs.keyEnter = Input.GetButtonDown(inputStrings.enterKey);
        inputs.keyArrowDown = Input.GetButtonDown(inputStrings.arrowKeyDown);
        inputs.keyArrowUp = Input.GetButtonDown(inputStrings.arrowKeyUp);
        inputs.escKey = Input.GetButtonDown(inputStrings.escKey);
    }

    private void PauseMenuLogic() {
        #region W Key Handler
        if((inputs.keyW)||(inputs.keyArrowUp)) {
            #region Pause Canvas
            if (currentState == "MOReload") {
                MOResume();
            }
            else if (currentState == "MOQuitTitle") {
                MOReload();
            }
            else if (currentState == "MOQuit") {
                MOQuitTitle();
            }
            #endregion
            #region Reload Canvas
            else if (currentState == "MOReloadYes"){
                MOReloadNo();
            }
            #endregion
            #region Quit Title Canvas
            else if (currentState == "MOQTYes") {
                MOQTNo();
            }
            #endregion
            #region Quit Canvas
            else if (currentState == "MOQCYes") {
                MOQCNo();
            }
            #endregion
        }
        #endregion
        #region S Key Handler
        else if ((inputs.keyS)||(inputs.keyArrowDown)) {
            #region Pause Canvas
            if (currentState == "MOResume") {
                MOReload();
            }
            else if (currentState == "MOReload") {
                MOQuitTitle();
            }
            else if (currentState == "MOQuitTitle") {
                MOQuit();
            }
            #endregion
            #region Reload Canvas
            else if (currentState == "MOReloadNo") {
                MOReloadYes();
            }
            #endregion
            #region Quit Title Canvas
            else if (currentState == "MOQTNo") {
                MOQTYes();
            }
            #endregion
            #region Quit Canvas
            else if (currentState == "MOQCNo") {
                MOQCYes();
            }
            #endregion
        }
        #endregion
        #region Enter Key Handler
        else if (inputs.keyEnter) {
            #region Pause Canvas
            if(currentState == "MOResume") {
                SelectResume();
            }
            else if(currentState == "MOReload") {
                SelectReload();
            }
            else if(currentState == "MOQuitTitle") {
                SelectQuitTitle();
            }
            else if(currentState == "MOQuit") {
                SelectQuit();
            }
            #endregion
            #region Reload Canvas
            else if (currentState == "MOReloadYes") {
                SelectReloadYes();
            }
            else if (currentState == "MOReloadNo") {
                SelectNoReload();
            }
            #endregion
            #region Quit Title Canvas
            else if (currentState == "MOQTYes") {
                SelectQuitTitleYes();
            }
            else if (currentState == "MOQTNo") {
                SelectNoQT();
            }
            #endregion
            #region Quit Canvas
            else if (currentState == "MOQCYes") {
                SelectQuitYes();
            }
            else if (currentState == "MOQCNo") {
                SelectNoQC();
            }
            #endregion
        }
        #endregion
        #region Esc Key Handler
        else if (inputs.escKey) {
            if((currentState == "Pause Screen")||(currentState == "MOResume")||(currentState == "MOReload")||(currentState == "MOQuitTitle")||(currentState == "MOQuit")) {
                SelectResume();
            }
            else if ((currentState == "Reload Screen")||(currentState == "MOReloadYes")||(currentState == "MOReloadNo")) {
                SelectNoReload();
            }
            else if ((currentState == "Quit Title Screen") || (currentState == "MOQTYes") || (currentState == "MOQTNo")) {
                SelectNoQT();
            }
            else if ((currentState == "Quit Screen") || (currentState == "MOQCYes") || (currentState == "MOQCNo")) {
                SelectNoQC();
            }
        }
        #endregion
    }
}
