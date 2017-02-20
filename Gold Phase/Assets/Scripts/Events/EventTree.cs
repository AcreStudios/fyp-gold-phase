using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class EventTree : MonoBehaviour {

    [System.Serializable]
    public struct Events {
        public string eventName;
        public string missionUI;

        public EventBody[] eventBody;
        [HideInInspector]
        public bool calibrated;
    }

    [System.Serializable]
    public struct EventBody {
        public string eventBodyName;
        public Triggers eventTriggers;
        public OnEventActive onEventActive;
        public Results results;
    }

    [System.Serializable]
    public struct Triggers {
        public Vector3 triggerPosition;
        public float triggerRadius;
        public float timer;
        public GameObject[] checkIfDestroyed;
        public KeyCode key;
    }

    [System.Serializable]
    public struct Results {
        public GameObject[] spawns;
        public GameObject[] toDestroy;
        public EventTree[] eventTreesToActivate;
        public string debugMessage;
        public string levelNameToLoad;
        public bool setAIAsHostile;
        public bool unableToProceed;
    }

    [System.Serializable]
    public struct OnEventActive {
        //[HideInInspector]
        public float timer;
        public float duration;
        public Vector3 location;
        public float randomRange;
        public GameObject[] spawns;
    }

    [Header("Tree Disabling Options")]
    [Tooltip("Disables the previous tree that called up this tree")]
    public bool disablePrevTree;
    [Tooltip("Disables all currently active trees")]
    public bool disableAllOtherActiveTrees;
    [Tooltip("Trees that will not be affected by the Disabling Options above")]
    public EventTree[] disableExceptions;

    [Header("On Event Tree End")]
    [Tooltip("Re-enables all trees disabled by this")]
    public bool reactivePrevTrees;
    [Tooltip("Trees that will not be affected by the Reactivating Options above")]
    public EventTree[] reactiveExceptions;

    [Header("Tree Events")]
    public Events[] events = new Events[0];
    public int currentEvent;

    public Text missionUI;
    [HideInInspector]
    public List<EventTree> treesDisabled;

    public void TreeEnabled(EventTree treeCalling) {
        enabled = true;
        treesDisabled = new List<EventTree>();

        if (disablePrevTree)
            if (TreeExceptionCheck(treeCalling, false, disableExceptions))
                treesDisabled.Add(treeCalling);


        if (disableAllOtherActiveTrees)
            for (var i = 0; i < EventData.eventTrees.Count; i++)
                if (TreeExceptionCheck(EventData.eventTrees[i], false, disableExceptions))
                    treesDisabled.Add(EventData.eventTrees[i]);
    }

    void OnEventTreeEnd() {
        enabled = false;

        if (reactivePrevTrees)
            for (var i = 0; i < treesDisabled.Count; i++)
                TreeExceptionCheck(treesDisabled[i], true, reactiveExceptions);
    }

    bool TreeExceptionCheck(EventTree treeToCheck, bool toggle, EventTree[] trees) {
        foreach (EventTree exception in trees)
            if (exception == treeToCheck || treeToCheck == this)
                return false;

        treeToCheck.enabled = toggle;
        return true;
    }

    void Awake() {
        EventData.eventTrees.Add(this);
    }

    void Update() {
        if (Application.isPlaying) {
            if (currentEvent < events.Length) {
                if (missionUI)
                    missionUI.text = events[currentEvent].missionUI;
                for (var i = 0; i < events[currentEvent].eventBody.Length; i++) {
                    EventActive(i, currentEvent);

                    if (CheckTrigger(i, currentEvent)) {
                        if (currentEvent == events.Length)
                            OnEventTreeEnd();
                        break;
                    }
                }
            }
        } else {
            for (var j = 0; j < events.Length; j++) {
                for (var k = 0; k < events[j].eventBody.Length; k++) {
                    for (var l = 0; l < events[j].eventBody[k].results.spawns.Length; l++)
                        if (events[j].eventBody[k].results.spawns[l] != null)
                            events[j].eventBody[k].results.spawns[l].SetActive(false);
                }
            }
        }
    }

    bool CheckTrigger(int bodyIndex, int eventIndex) {

        if (!events[eventIndex].calibrated) {
            for (var i = 0; i < events[eventIndex].eventBody.Length; i++)
                events[eventIndex].eventBody[i].eventTriggers.timer += Time.time;
            events[eventIndex].calibrated = true;
        }

        if (events[eventIndex].eventBody[bodyIndex].eventTriggers.triggerRadius > 0) {
            Collider[] temp;
            int playerIsInRange = 0;
            temp = Physics.OverlapSphere(events[eventIndex].eventBody[bodyIndex].eventTriggers.triggerPosition, events[eventIndex].eventBody[bodyIndex].eventTriggers.triggerRadius);

            foreach (Collider obj in temp)
                if (obj.transform.root.tag == "Player")
                    playerIsInRange++;

            if (!(playerIsInRange > 0))
                return false;
        }

        foreach (GameObject toCheck in events[eventIndex].eventBody[bodyIndex].eventTriggers.checkIfDestroyed)
            if (toCheck)
                return false;

        if (events[eventIndex].eventBody[bodyIndex].eventTriggers.timer > Time.time) {
            return false;
        }

        if (events[eventIndex].eventBody[bodyIndex].eventTriggers.key != KeyCode.None)
            if (!(Input.GetKey(events[eventIndex].eventBody[bodyIndex].eventTriggers.key)))
                return false;

        ActivateEvent(events[eventIndex].eventBody[bodyIndex].results);
        return true;
    }

    void EventActive(int bodyIndex, int eventIndex) {
        if (events[currentEvent].eventBody[bodyIndex].onEventActive.timer < Time.time) {
            events[eventIndex].eventBody[bodyIndex].onEventActive.timer += events[eventIndex].eventBody[bodyIndex].onEventActive.duration;
            foreach (GameObject toSpawn in events[currentEvent].eventBody[bodyIndex].onEventActive.spawns) {
                Vector3 spawnLoc = events[currentEvent].eventBody[bodyIndex].onEventActive.location;
                if (events[currentEvent].eventBody[bodyIndex].onEventActive.randomRange > 0) {
                    float tempRandomRange = events[currentEvent].eventBody[bodyIndex].onEventActive.randomRange;
                    spawnLoc += new Vector3(Random.Range(-tempRandomRange, tempRandomRange), 0.5f, Random.Range(-tempRandomRange, tempRandomRange));
                }
                Instantiate(toSpawn, spawnLoc, Quaternion.identity);
            }
        }

    }

    void ActivateEvent(Results endResult) {
        foreach (GameObject spawn in endResult.spawns)
            spawn.SetActive(true);

        foreach (GameObject destroy in endResult.toDestroy)
            Destroy(destroy);

        foreach (EventTree toActive in endResult.eventTreesToActivate)
            toActive.TreeEnabled(this);

        //foreach (EventResults results in endResult.scriptedEventsToTrigger)
        //results.ScriptedResult();

        if (endResult.debugMessage != "")
            Debug.Log(endResult.debugMessage);

        if (endResult.levelNameToLoad != "")
            SceneManager.LoadScene(endResult.levelNameToLoad);

        if (!endResult.unableToProceed) {
            currentEvent++;
        }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(EventTree))]
public class EventTreeEditor : Editor {

    public enum ButtonType {
        Trigger, OnEventActive
    }

    EventTree t;
    int currentTree;
    int currentEvent;
    int currentTrigger;
    ButtonType clickedType;

    void OnSceneGUI() {
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(90, 0, 0);


        if (t != null)
            //Handles.color = t.eventTrees[i].eventColors;
            for (var j = 0; j < t.events.Length; j++)
                for (var k = 0; k < t.events[j].eventBody.Length; k++) {
                    Handles.color = Color.red;
                    Handles.CircleCap(0, t.events[j].eventBody[k].eventTriggers.triggerPosition, rotation, t.events[j].eventBody[k].eventTriggers.triggerRadius);

                    float rad = 0;
                    if (t.events[j].eventBody[k].onEventActive.randomRange == 0)
                        rad = 1;
                    else
                        rad = t.events[j].eventBody[k].onEventActive.randomRange;
                    Handles.color = Color.blue;

                    Handles.CircleCap(0, t.events[j].eventBody[k].onEventActive.location, rotation, rad);
                }

        Event e;
        e = Event.current;

        if (e.type == EventType.keyDown) {
            if (e.keyCode == KeyCode.Q) {
                RaycastHit hit;

                Vector2 temp = e.mousePosition;
                temp.y = Screen.height - e.mousePosition.y;
                Ray ray = Camera.current.ScreenPointToRay(temp);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                    switch (clickedType) {
                        case ButtonType.Trigger:
                            t.events[currentEvent].eventBody[currentTrigger].eventTriggers.triggerPosition = hit.point;
                            t.events[currentEvent].eventBody[currentTrigger].eventTriggers.triggerRadius = 1;
                            break;
                        case ButtonType.OnEventActive:
                            t.events[currentEvent].eventBody[currentTrigger].onEventActive.location = hit.point;
                            break;
                    }
                }
            }
        }
    }

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        t = target as EventTree;

        if (t != null)
            for (var j = 0; j < t.events.Length; j++)
                for (var k = 0; k < t.events[j].eventBody.Length; k++) {
                    string buttonName = "Set trigger radius ";

                    if (t.events[j].eventName != "")
                        buttonName = buttonName + t.events[j].eventName + " [" + t.events[j].eventBody[k].eventBodyName + "]";
                    else
                        buttonName = buttonName + j.ToString();

                    if (GUILayout.Button(buttonName)) {
                        SceneView sceneView = SceneView.sceneViews[0] as SceneView;
                        sceneView.Focus();
                        currentEvent = j;
                        currentTrigger = k;
                        clickedType = ButtonType.Trigger;
                    }

                    buttonName = "Set OnEventActive radius ";
                    if (t.events[j].eventName != "")
                        buttonName = buttonName + t.events[j].eventName + " [" + t.events[j].eventBody[k].eventBodyName + "]";
                    else
                        buttonName = buttonName + j.ToString();

                    if (GUILayout.Button(buttonName)) {
                        SceneView sceneView = SceneView.sceneViews[0] as SceneView;
                        sceneView.Focus();
                        currentEvent = j;
                        currentTrigger = k;
                        clickedType = ButtonType.OnEventActive;
                    }
                }
    }
}
#endif

