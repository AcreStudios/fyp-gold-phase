using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PatrolModule : MonoBehaviour {

    public Vector3[] patrolLocations;
    public int currentLocation;
    //public int limit;
    [HideInInspector] public int valueToAdd;
}
