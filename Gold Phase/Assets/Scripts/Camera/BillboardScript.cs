using UnityEngine;
using System.Collections;

public class BillboardScript : MonoBehaviour {

    public Camera my_camera;


	void Update () {
        transform.LookAt(transform.position + my_camera.transform.rotation * Vector3.back,
            my_camera.transform.rotation * Vector3.up);
	}
}
