using UnityEngine;
using System.Collections;

public class AIFunctions : MonoBehaviour {

    public enum CoverType {
        None, High, Low
    }
    [Header("Cover Settings")]
    public Transform minHeightForCover;
    public Transform maxHeightForCover;
    public float durationInCover;
    public bool ableToHide;

    [Header("Target-Related Settings")]
    public Vector3 destination;
    public Collider destinationMarker;
    public Transform target;
    protected Transform[] guns = new Transform[1];
    protected Transform linecastCheck;
    protected Vector3 startingPoint;
    protected bool showGunEffect;
    protected Animator animator;
    protected UnityEngine.AI.NavMeshAgent agent;
    public CoverType coverType;
    //protected Vector3 
    //Health hpScript;

    void Awake() {
        //hpScript = GetComponent<Health>();
    }

    public virtual void DamageRecieved() {
        if (!CivillianManager.instance.hostile)
            CivillianManager.instance.AISetToHostile();

        // if (hpScript.curHealth <= 0)
        // enabled = false;
    }

    public IEnumerator ChangeObjectLocation(GameObject obj, Vector3 location) {
        yield return new WaitForSeconds(0.1f);
        obj.transform.position = location;
    }

    public Vector3 GetDestinationPoint(float range) {
        if (ableToHide) {
            Vector3 tempGradient = target.position - transform.position;
            tempGradient = Mathf.Abs(tempGradient.x) >= Mathf.Abs(tempGradient.z) ? tempGradient / Mathf.Abs(tempGradient.x) : tempGradient / Mathf.Abs(tempGradient.z);
            tempGradient.y = 0;

            for (var j = -1; j < 2; j++) {
                if (j != 0) {
                    //Debug.DrawLine(target.position, target.position + (tempGradient * -(range / 4)) + ((new Vector3(-(tempGradient.z), 0, tempGradient.x) * range / 4) * j), Color.black, 5);
                    Collider[] colliders = Physics.OverlapSphere(target.position + (tempGradient * -(range / 4)) + ((new Vector3(-(tempGradient.z), 0, tempGradient.x) * range / 4) * j), range / 2);

                    for (var i = 0; i < colliders.Length; i++) {
                        if (colliders[i].transform.CompareTag("Untagged"))
                            if (colliders[i].bounds.center.y + colliders[i].bounds.extents.y > minHeightForCover.position.y) {
                                Vector3 temp = Vector3.zero;
                                int vectorAffected;
                                float maxValue;

                                temp = colliders[i].bounds.center - target.position;

                                temp.x = (temp.x / Mathf.Abs(temp.x)) * colliders[i].bounds.extents.x;
                                temp.z = (temp.z / Mathf.Abs(temp.z)) * colliders[i].bounds.extents.z;

                                if (colliders[i].bounds.center.y + colliders[i].bounds.extents.y > maxHeightForCover.position.y) {
                                    coverType = CoverType.High;
                                    if (Mathf.Abs(colliders[i].bounds.center.x - target.position.x) > Mathf.Abs(colliders[i].bounds.center.z - target.position.z))
                                        temp.x *= -1;
                                    else
                                        temp.z *= -1;

                                    vectorAffected = 0;
                                    maxValue = temp[vectorAffected] + 0.5f;
                                } else {
                                    coverType = CoverType.Low;
                                    if (Mathf.Abs(colliders[i].bounds.center.x - target.position.x) < Mathf.Abs(colliders[i].bounds.center.z - target.position.z))
                                        vectorAffected = 0;
                                    else
                                        vectorAffected = 2;

                                    maxValue = colliders[i].bounds.extents[vectorAffected];
                                }


                                for (var k = -maxValue; k <= maxValue; k++) { //-ve +ve is the problem need some fixes
                                    //Debug.Log(k);
                                    Vector3 cachedVector = new Vector3();
                                    cachedVector = temp;
                                    cachedVector[vectorAffected] = k;
                                    cachedVector += colliders[i].bounds.center;
                                    cachedVector.y = transform.position.y;
                                    Debug.DrawLine(target.position, cachedVector, Color.red, 1);
                                    // if (CheckIfPosAvail(cachedVector))
                                    //return cachedVector;
                                }
                            }
                    }
                }
            }
        }
        coverType = CoverType.None;
        return ArcBasedPosition(target.position - transform.position, target.position, range);
    }

    public Vector3 ArcBasedPosition(Vector3 givenVector, Vector3 targetPos, float givenLength) {
        givenVector.y = 0;
        targetPos.y = transform.position.y;

        Vector3 gradient = Mathf.Abs(givenVector.x) >= Mathf.Abs(givenVector.z) ? givenVector / Mathf.Abs(givenVector.x) : givenVector / Mathf.Abs(givenVector.z);

        for (var i = -givenLength; i < givenLength + 1; i++) {
            Vector3 currentPosInCircle = gradient * i;
            Vector3 reflexedGradient = new Vector3(-(gradient.z), 0, gradient.x) * (givenLength - Mathf.Abs(i));

            if (CheckIfPosAvail(targetPos + (Vector3.Normalize(currentPosInCircle - reflexedGradient) * givenLength)))
                return targetPos + (Vector3.Normalize(currentPosInCircle - reflexedGradient) * givenLength);

            if (CheckIfPosAvail(targetPos + (Vector3.Normalize(currentPosInCircle + reflexedGradient) * givenLength)))
                return targetPos + (Vector3.Normalize(currentPosInCircle + reflexedGradient) * givenLength);
        }
        return transform.position;
    }

    public bool CheckIfPosAvail(Vector3 temp) {
        temp.y = transform.position.y;
        bool hitFloor = false;

        Collider[] inCollision = Physics.OverlapCapsule(temp, temp, 1);

        foreach (Collider collision in inCollision) {
            if (collision != destinationMarker)
                if (collision.transform.tag == "Marker")
                    return false;

            if (collision.transform.tag == "Floor")
                hitFloor = true;
        }

        if (hitFloor)
            destinationMarker.transform.position = temp;

        return hitFloor;
    }
}
