using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(PatrolModule))] //It needs a "reference to be able to run the script? Hmm interesting.
public class PatrolEditor : Editor {
    PatrolModule t;
    int coordToChange;


    void OnSceneGUI() { //You must select scene itself (OnSceneGUI) to be able to detect the event
        if (t != null)
            if (t.patrolLocations.Length > 0) {
                Event e = Event.current;

                Quaternion rotation = Quaternion.identity;
                rotation.eulerAngles = new Vector3(90, 0, 0);
                Handles.color = Color.red;

                for (var i =0; i < t.patrolLocations.Length; i++) {
                    
                    Handles.CircleCap(0, t.patrolLocations[i], rotation, 1);
                    if (i > 0)
                        Handles.DrawDottedLine(t.patrolLocations[i - 1], t.patrolLocations[i], 4);
                    else {
                        Handles.DrawDottedLine(t.transform.position, t.patrolLocations[i], 4);
                    }
                }

                if (e.type == EventType.keyDown) {

                    if (e.keyCode == KeyCode.Q) {
                        RaycastHit hit;

                        Vector2 temp = e.mousePosition;
                        temp.y = Screen.height - e.mousePosition.y;
                        Ray ray = Camera.current.ScreenPointToRay(temp);

                        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                            //Debug.DrawLine(Vector3.zero, hit.point, Color.black);
                            t.patrolLocations[coordToChange] = hit.point;
                        }
                    }
                }
            }
    }

    public override void OnInspectorGUI() {

        DrawDefaultInspector();

        t = target as PatrolModule;
        if (t != null)
            if (t.patrolLocations.Length > 0)
                for (var i = 0; i < t.patrolLocations.Length; i++) {
                    if (GUILayout.Button("Set Patrol Point "+ i.ToString())) {
                        coordToChange = i;
                        SceneView sceneView = SceneView.sceneViews[0] as SceneView;
                        sceneView.Focus();
                    }
                }
    }
}
#endif