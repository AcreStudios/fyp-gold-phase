using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivillianManager : MonoBehaviour {

    [System.Serializable]
    public struct TaskList {
        public string taskName;
        public TaskLocation[] tasks;
        public string taskTag;
        public int taskCapacity;
        public float taskTimer;

        public Animation animationWhenPerformingTask;
    }

    [System.Serializable]
    public struct TaskLocation {
        public Transform taskLocation;
        public GameObject[] civillianOnTask;
    }

    [HideInInspector]
    public List<CivillianAI> civillianAIList = new List<CivillianAI>();
    [HideInInspector]
    public List<AI> aiList = new List<AI>();

    public static CivillianManager instance;
    public bool hostile;
    public int civillianCount;
    public GameObject civillian;
    public TaskList[] taskLists;
    public AudioClip[] sfxList;

    MeshFilter[] floors;

    void Awake() {
        instance = this;
        //Debug.Log(gameObject);
        for (var i = 0; i < taskLists.Length; i++) {
            GameObject[] taskLocations = GameObject.FindGameObjectsWithTag(taskLists[i].taskTag);
            taskLists[i].tasks = new TaskLocation[taskLocations.Length];
            for (var j = 0; j < taskLists[i].tasks.Length; j++) {
                taskLists[i].tasks[j].taskLocation = taskLocations[j].transform;
                taskLists[i].tasks[j].civillianOnTask = new GameObject[taskLists[i].taskCapacity];
            }
        }

        GameObject[] floor = GameObject.FindGameObjectsWithTag("Floor");
        floors = new MeshFilter[floor.Length];

        for (var i = 0; i < floors.Length; i++)
            floors[i] = floor[i].GetComponent<MeshFilter>();

        for (var i = 0; i < civillianCount; i++)
            SpawnOnRandomFloor(civillian);

    }

    public TaskLocation TaskQuery(GameObject query, out float taskDuration, out int taskUser) {
        for (var i = 0; i < taskLists.Length; i++)
            for (var j = 0; j < taskLists[i].tasks.Length; j++)
                for (var k = 0; k < taskLists[i].tasks[j].civillianOnTask.Length; k++)
                    if (!taskLists[i].tasks[j].civillianOnTask[k]) {
                        taskDuration = taskLists[i].taskTimer;
                        taskUser = k;

                        taskLists[i].tasks[j].civillianOnTask[k] = query;
                        return taskLists[i].tasks[j];
                    }

        taskDuration = 0.1f;
        taskUser = 0;
        return new TaskLocation();
    }

    public void PlayRandomSound(Vector3 position) {
        //if (sfxList.Length > 0)
            //SoundManager.instance.PlaySoundOnce(position, sfxList[Random.Range(0, sfxList.Length)]);
    }

    public void SpawnOnRandomFloor(GameObject civillian) {
        MeshFilter floorChoose = floors[Random.Range(0, floors.Length)];

        Vector3 spawnLocation = floorChoose.mesh.vertices[Random.Range(0, floorChoose.mesh.vertices.Length)];
        Instantiate(civillian, new Vector3(spawnLocation.x * floorChoose.transform.localScale.x, 0, spawnLocation.z * floorChoose.transform.localScale.z) + floorChoose.transform.position, Quaternion.identity);
    }

    public void AISetToHostile() {
        hostile = true;

        for (var i = 0; i < civillianAIList.Count; i++) 
            civillianAIList[i].Scared();

        for (var i = 0; i < aiList.Count; i++)
            aiList[i].HostileKnown();
    }
}
