using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIOverseer : MonoBehaviour {

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

    public static AIOverseer instance;
    public int civillianCount;
    public GameObject civillian;
    public TaskList[] taskLists;
    public AudioClip[] gunShotList;
    public AudioClip[] flyByList;

    MeshFilter[] floors;

    void Awake() {
        instance = this;
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

    public void PlayRandomSound(AudioClip[] audioGroup, Vector3 position) {
        //if (audioGroup.Length > 0)
            //SoundManager.instance.PlaySoundOnce(position, audioGroup[Random.Range(0, audioGroup.Length)]);
    }

    public void SpawnOnRandomFloor(GameObject civillian) {
        MeshFilter floorChoose = floors[Random.Range(0, floors.Length)];

        Vector3 spawnLocation = floorChoose.mesh.vertices[Random.Range(0, floorChoose.mesh.vertices.Length)];
        Instantiate(civillian, new Vector3(spawnLocation.x * floorChoose.transform.localScale.x, 0, spawnLocation.z * floorChoose.transform.localScale.z) + floorChoose.transform.position, Quaternion.identity);
    }

    public void AIHostileRadius(Vector3 pos, float radius) {
        Collider[] objectsInRadius = Physics.OverlapSphere(pos, radius);

        foreach (Collider obj in objectsInRadius) {
            if (obj.CompareTag("Civillian"))
                civillianAIList[int.Parse(obj.name)].FindTarget();
            if (obj.CompareTag("Enemy"))
                aiList[int.Parse(obj.name)].FindTarget();
        }
    }
}
