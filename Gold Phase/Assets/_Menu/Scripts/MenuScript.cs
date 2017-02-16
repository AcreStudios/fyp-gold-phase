using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public string currentState;
    public Inputs inputs;

    [System.Serializable]
    public class StartScreen {
        public GameObject StartCanvas;
        public Button PressToStart;
		public GameObject StartBackground;
    }
    public StartScreen startScreen;

    [System.Serializable]
    public class MainMenu {
		public GameObject MenuBackground;
        public GameObject MainCanvas;
        public GameObject GORedNewGame;
        public GameObject GORedSettings;
        public GameObject GORedCredits;
        public GameObject GORedQuit;
        public GameObject NewGame;
        public GameObject Settings;
        public GameObject Credits;
        public GameObject Quit;
    }
    public MainMenu mainMenu;

    [System.Serializable]
    public class SettingsMenu {
		public GameObject SettingsBackground;
        public GameObject SettingsCanvas;
        public GameObject GORedGame;
        public GameObject GORedAudio;
        public GameObject GORedVideo;
        public GameObject GORedBack;
        public GameObject Game;
        public GameObject Audio;
        public GameObject Video;
        public GameObject Back;
    }
    public SettingsMenu settingsMenu;

    [System.Serializable]
    public class CreditsMenu {
		public GameObject CreditsBackground;
        public GameObject CreditsCanvas;
        public GameObject GORedCreditsBack;
    }
    public CreditsMenu creditsMenu;

    [System.Serializable]
    public class QuitMenu {
		public GameObject QuitBackground;
        public GameObject QuitCanvas;
        public GameObject GORedYes;
        public GameObject GORedNo;
        public GameObject Yes;
        public GameObject No;
    }
    public QuitMenu quitMenu;

    [System.Serializable]
    public class DevMenu {
		public GameObject DevBackground;
        public GameObject DevCanvas;
        public GameObject Sequence1;
        public GameObject Sequence2;
        public GameObject Sequence3;
    }
    public DevMenu devMenu;
    
    [System.Serializable]
    public class LoadingScreen {
		public GameObject LoadingBackground;
        public GameObject LoadingCanvas;
        public GameObject PressToContinue;
        public GameObject PressToContinueButton;
        public GameObject LoadingText;
        public GameObject DoneText;
        public GameObject LoadingIcon;
        public GameObject DoNot;
    }
    public LoadingScreen loadingScreen;

    void Start () {
        #region Start Screen
        startScreen.StartCanvas.SetActive(true);

        startScreen.PressToStart.enabled = true;
		startScreen.StartBackground.SetActive(true);

        #endregion
        #region Main Menu
        mainMenu.MainCanvas.SetActive(false);
		mainMenu.MenuBackground.SetActive(true);
        mainMenu.GORedNewGame.SetActive(true);
        mainMenu.GORedSettings.SetActive(false);
        mainMenu.GORedCredits.SetActive(false);
        mainMenu.GORedQuit.SetActive(false);

        mainMenu.NewGame.SetActive(false);
        mainMenu.Settings.SetActive(true);
        mainMenu.Credits.SetActive(true);
        mainMenu.Quit.SetActive(true);
        #endregion
        #region Settings Menu
        settingsMenu.SettingsCanvas.SetActive(false);
		settingsMenu.SettingsBackground.SetActive(true);

        settingsMenu.GORedGame.SetActive(true);
        settingsMenu.GORedAudio.SetActive(false);
        settingsMenu.GORedVideo.SetActive(false);
        settingsMenu.GORedBack.SetActive(false);

        settingsMenu.Game.SetActive(false);
        settingsMenu.Audio.SetActive(true);
        settingsMenu.Video.SetActive(true);
        settingsMenu.Back.SetActive(true);
        #endregion
        #region Credits Menu
        creditsMenu.CreditsCanvas.SetActive(false);
		creditsMenu.CreditsBackground.SetActive(true);

        creditsMenu.GORedCreditsBack.SetActive(true);
        #endregion
        #region Quit Menu
        quitMenu.QuitCanvas.SetActive(false);
		quitMenu.QuitBackground.SetActive(true);

        quitMenu.GORedYes.SetActive(false);
        quitMenu.GORedNo.SetActive(true);

        quitMenu.Yes.SetActive(true);
        quitMenu.No.SetActive(false);
        #endregion
        #region Loading Screen
        loadingScreen.LoadingCanvas.SetActive(false);
		loadingScreen.LoadingBackground.SetActive(true);

        loadingScreen.PressToContinue.SetActive(false);
        loadingScreen.PressToContinueButton.SetActive(false);

        loadingScreen.LoadingText.SetActive(false);
        loadingScreen.DoneText.SetActive(false);
        //loadingScreen.LoadingIcon.SetActive(false);
        loadingScreen.DoNot.SetActive(false);
        #endregion
        #region Dev Menu
        devMenu.DevCanvas.SetActive(false);
        devMenu.Sequence1.SetActive(false);
        devMenu.Sequence2.SetActive(false);
        devMenu.Sequence3.SetActive(false);
		devMenu.DevBackground.SetActive(true);
        #endregion
    }

    // Handles all MouseOver Events
    #region Main Menu Mouseover Handler
    public void MONewGame() {
        mainMenu.NewGame.SetActive(false);
        mainMenu.Settings.SetActive(true);
        mainMenu.Credits.SetActive(true);
        mainMenu.Quit.SetActive(true);

        mainMenu.GORedNewGame.SetActive(true);
        mainMenu.GORedSettings.SetActive(false);
        mainMenu.GORedCredits.SetActive(false);
        mainMenu.GORedQuit.SetActive(false);

        currentState = "MainMenu: MO NewGame";
        print(currentState);
    }
    public void MOSettings() {
        mainMenu.NewGame.SetActive(true);
        mainMenu.Settings.SetActive(false);
        mainMenu.Credits.SetActive(true);
        mainMenu.Quit.SetActive(true);

        mainMenu.GORedNewGame.SetActive(false);
        mainMenu.GORedSettings.SetActive(true);
        mainMenu.GORedCredits.SetActive(false);
        mainMenu.GORedQuit.SetActive(false);

        currentState = "MainMenu: MO Settings";
        print(currentState);
    }
    public void MOCredits() {
        mainMenu.NewGame.SetActive(true);
        mainMenu.Settings.SetActive(true);
        mainMenu.Credits.SetActive(false);
        mainMenu.Quit.SetActive(true);

        mainMenu.GORedNewGame.SetActive(false);
        mainMenu.GORedSettings.SetActive(false);
        mainMenu.GORedCredits.SetActive(true);
        mainMenu.GORedQuit.SetActive(false);

        currentState = "MainMenu: MO Credits";
        print(currentState);
    }
    public void MOQuit() {
        mainMenu.NewGame.SetActive(true);
        mainMenu.Settings.SetActive(true);
        mainMenu.Credits.SetActive(true);
        mainMenu.Quit.SetActive(false);

        mainMenu.GORedNewGame.SetActive(false);
        mainMenu.GORedSettings.SetActive(false);
        mainMenu.GORedCredits.SetActive(false);
        mainMenu.GORedQuit.SetActive(true);

        currentState = "MainMenu: MO Quit";
        print(currentState);
    }
    #endregion
    #region Settings Mouseover Handler
    public void MOGame() {
        settingsMenu.GORedGame.SetActive(true);
        settingsMenu.GORedAudio.SetActive(false);
        settingsMenu.GORedVideo.SetActive(false);
        settingsMenu.GORedBack.SetActive(false);

        settingsMenu.Game.SetActive(false);
        settingsMenu.Audio.SetActive(true);
        settingsMenu.Video.SetActive(true);
        settingsMenu.Back.SetActive(true);

        currentState = "SettingsMenu: MO Game";
        print(currentState);
    }
    public void MOAudio() {
        settingsMenu.GORedGame.SetActive(false);
        settingsMenu.GORedAudio.SetActive(true);
        settingsMenu.GORedVideo.SetActive(false);
        settingsMenu.GORedBack.SetActive(false);

        settingsMenu.Game.SetActive(true);
        settingsMenu.Audio.SetActive(false);
        settingsMenu.Video.SetActive(true);
        settingsMenu.Back.SetActive(true);

        currentState = "SettingsMenu: MO Audio";
        print(currentState);
    }
    public void MOVideo() {
        settingsMenu.GORedGame.SetActive(false);
        settingsMenu.GORedAudio.SetActive(false);
        settingsMenu.GORedVideo.SetActive(true);
        settingsMenu.GORedBack.SetActive(false);

        settingsMenu.Game.SetActive(true);
        settingsMenu.Audio.SetActive(true);
        settingsMenu.Video.SetActive(false);
        settingsMenu.Back.SetActive(true);

        currentState = "SettingsMenu: MO Video";
        print(currentState);
    }
    public void MOBack() {
        settingsMenu.GORedGame.SetActive(false);
        settingsMenu.GORedAudio.SetActive(false);
        settingsMenu.GORedVideo.SetActive(false);
        settingsMenu.GORedBack.SetActive(true);

        settingsMenu.Game.SetActive(true);
        settingsMenu.Audio.SetActive(true);
        settingsMenu.Video.SetActive(true);
        settingsMenu.Back.SetActive(false);

        currentState = "SettingsMenu: MO Back";
        print(currentState);
    }
    #endregion
    #region Quit Mouseover Handler
    public void MOYes() {
        quitMenu.GORedYes.SetActive(true);
        quitMenu.GORedNo.SetActive(false);

        quitMenu.Yes.SetActive(false);
        quitMenu.No.SetActive(true);

        currentState = "QuitMenu: MO Yes";
        print(currentState);
    }
    public void MONo() {
        quitMenu.GORedYes.SetActive(false);
        quitMenu.GORedNo.SetActive(true);

        quitMenu.Yes.SetActive(true);
        quitMenu.No.SetActive(false);

        currentState = "QuitMenu: MO No";
        print(currentState);
    }
    #endregion

    // Handles all Button Press Events. Call functions from editor
    #region Button Press Events
    public void SelectSettings() {
        settingsMenu.SettingsCanvas.SetActive(true);
        mainMenu.MainCanvas.SetActive(false);
        currentState = "Settings Screen";
        print(currentState);
    }
    public void SelectBack() {
        settingsMenu.SettingsCanvas.SetActive(false);
        creditsMenu.CreditsCanvas.SetActive(false);
        devMenu.DevCanvas.SetActive(false);
        mainMenu.MainCanvas.SetActive(true);
        currentState = "Menu Screen";
        print(currentState);
    }
    public void SelectNewGame() {
        loadingScreen.LoadingCanvas.SetActive(true);
        mainMenu.MainCanvas.SetActive(false);
        loadingScreen.LoadingText.SetActive(true);
        loadingScreen.DoNot.SetActive(true);
        //loadingScreen.LoadingIcon.SetActive(true);
        currentState = "Loading Screen";
        print(currentState);

        StartCoroutine(loadingtime());
    }
    public void SelectCredits() {
        creditsMenu.CreditsCanvas.SetActive(true);
        mainMenu.MainCanvas.SetActive(false);
        currentState = "Credits Screen";
        print(currentState);
    }
    public void SelectQuit() {
        quitMenu.QuitCanvas.SetActive(true);
        mainMenu.MainCanvas.SetActive(false);
        currentState = "Quit Screen";
        print(currentState);
    }
    public void SelectNewQuit() {
        mainMenu.MainCanvas.SetActive(false);
        startScreen.StartCanvas.SetActive(true);
        currentState = "Start Screen";
        print(currentState);
    }
    public void SelectStart() {
        mainMenu.MainCanvas.SetActive(true);
        startScreen.StartCanvas.SetActive(false);
        currentState = "Menu Screen";
        print(currentState);
    }
    public void SelectNo() {
        quitMenu.QuitCanvas.SetActive(false);
        mainMenu.MainCanvas.SetActive(true);
        currentState = "Menu Screen";
        print(currentState);
    }
    public void SelectYes() {
        Application.Quit();
    }
    public void loadApplication() {
        if(inputs.keySpace) {
            SceneManager.LoadScene("AcreHQ_EventTree");
        }
    }
    public void SelectDevMenu() {
        mainMenu.MainCanvas.SetActive(false);
        devMenu.DevCanvas.SetActive(true);
        devMenu.Sequence1.SetActive(true);
        devMenu.Sequence2.SetActive(true);
        devMenu.Sequence3.SetActive(true);
        currentState = "Dev Screen";
    }
    public void SelectInterior() {
        SceneManager.LoadScene("AcreHQ_EventTree");
    }
    public void SelectExterior() {
        //SceneManager.LoadScene("Eden");
    }
    #endregion

    
    IEnumerator loadingtime() {
        Debug.Log("Scene Loading");
        yield return new WaitForSeconds(3);
        loadingScreen.PressToContinue.SetActive(true);
        loadingScreen.PressToContinueButton.SetActive(true);
        loadingScreen.DoNot.SetActive(false);
        //loadingScreen.LoadingIcon.SetActive(false);
        loadingScreen.LoadingText.SetActive(false);
        loadingScreen.DoneText.SetActive(true);
        currentState = "Loading Done";
    }
}
