using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class AI : AIFunctions {

    public enum AIStates {
        Idle,
        Patrol,
        Escort,
        Attacking
    }

    public enum WeaponType {
        RaycastShooting,
        Area
    }

    [Header("Behaviours")]
    public AIStates currentState;
    public float reactionTime;
    public bool toEscort;
    public bool camp;
    protected AIStates defaultState;
    public float retaliationTiming;

    [Header("Weapons")]
    public float damage;
    public WeaponType attackType;
    public Vector3 weaponOffset = new Vector3(0, 1.276f, 0);
    public float attackInterval;
    public bool ableToDragPlayerOutOfCover;
    float attackTimer;

    [Header("Raycast Shooting Attack Settings")]
    public float gunSprayValue;
    public TrailEffectFade gunEffect;

    [Header("Area Attack Settings")]
    public float areaTestRadius;

    [Header("Debug")]
    public bool damageTest;
    public bool displayDebugMessage;

    PatrolModule patrolMod;
    float stateChangeTimer;
    float inCoverTimer;
    bool recentlyGotPoint;
    float retaliationTimer;

    void Start() {
        gameObject.tag = "Enemy";

        AIOverseer.instance.aiList.Add(this);
        gameObject.name = (AIOverseer.instance.aiList.Count - 1).ToString();

        guns[0] = transform.Find("Hanna_GunL");
        linecastCheck = transform.Find("LinecastChecker");

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        startingPoint = transform.position;

        animator = GetComponent<Animator>();

        if ((patrolMod = GetComponent<PatrolModule>()) != null) {
            if (patrolMod.patrolLocations.Length > 0) {
                defaultState = AIStates.Patrol;
                agent.destination = patrolMod.patrolLocations[0];
            } else {
                defaultState = AIStates.Idle;
            }
        } else {
            defaultState = AIStates.Idle;
        }

        destination = transform.position;
        currentState = defaultState;

        if (toEscort) {
            currentState = AIStates.Escort;
            agent.destination = patrolMod.patrolLocations[0];
        }
    }

    void Update() {

        if (damageTest) {
            damageTest = false;
            DamageRecieved();
        }

        switch (currentState) {
            case AIStates.Idle:
                if (target) {
                    stateChangeTimer = Time.time + reactionTime;
                    destination = GetDestinationPoint(range, ableToHide);
                    currentState = AIStates.Attacking;
                } else {
                    if ((startingPoint - transform.position).magnitude < 1) {
                        animator.SetInteger("TreeState", 0);
                    } else {
                        agent.destination = startingPoint;
                    }
                }
                break;

            case AIStates.Patrol:
                if (target != null) {
                    stateChangeTimer = Time.time + reactionTime;
                    destination = GetDestinationPoint(range, ableToHide);
                    currentState = AIStates.Attacking;
                } else {
                    if ((patrolMod.patrolLocations[patrolMod.currentLocation] - transform.position).magnitude < 1) {
                        if (patrolMod.currentLocation >= patrolMod.patrolLocations.Length - 1) {
                            patrolMod.valueToAdd = -1;
                        } else if (patrolMod.currentLocation <= 0) {
                            patrolMod.valueToAdd = 1;
                        }

                        patrolMod.currentLocation += patrolMod.valueToAdd;
                    } else {
                        agent.destination = patrolMod.patrolLocations[patrolMod.currentLocation];
                        animator.SetInteger("TreeState", 1);
                        transform.LookAt(agent.destination);
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    }
                }
                break;

            case AIStates.Escort:
                if (patrolMod.currentLocation < patrolMod.patrolLocations.Length) {
                    if (agent.velocity.sqrMagnitude == 0) {
                        transform.LookAt(target);
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                        animator.SetInteger("TreeState", 0);
                        if ((target.position - transform.position).sqrMagnitude < 5 && !recentlyGotPoint) {
                            patrolMod.currentLocation++;
                            agent.destination = patrolMod.patrolLocations[patrolMod.currentLocation];
                            recentlyGotPoint = true;
                        }
                    } else {
                        agent.destination = patrolMod.patrolLocations[patrolMod.currentLocation];
                        animator.SetInteger("TreeState", 1);
                        recentlyGotPoint = false;
                    }
                } else {
                    transform.LookAt(target);
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    animator.SetInteger("TreeState", 0);
                }
                break;

            case AIStates.Attacking:
                if (Time.time > stateChangeTimer) {
                    if (agent.velocity.sqrMagnitude == 0) {
                        if (target) {
                            if (Time.time > inCoverTimer) {
                                transform.LookAt(target);
                                Attack();
                            } else
                                switch (coverType) {
                                    case CoverType.Low:
                                        animator.SetInteger("TreeState", 3);
                                        break;
                                    case CoverType.High:
                                        break;
                                }

                            RaycastHit hit;

                            if (Physics.Linecast(new Vector3(destination.x, minHeightForCover.position.y, destination.z), target.position, out hit))
                                if (hit.transform.root == target) {
                                    destination = GetDestinationPoint(range, ableToHide);
                                    inCoverTimer = Time.time;
                                }

                            if ((target.position - transform.position).sqrMagnitude > range * range) {
                                destination = GetDestinationPoint(range, ableToHide);
                                inCoverTimer = Time.time;
                            }
                        }
                    } else {
                        animator.SetInteger("TreeState", 1);
                        transform.LookAt(destination);
                    }
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    agent.destination = destination;
                }
                break;
        }

        destinationMarker.transform.position = destination;
    }

    public override void DamageRecieved() {
        toEscort = false;
        currentState = AIStates.Attacking;

        if (coverType == CoverType.Low)
            inCoverTimer += durationInCover;

        retaliationTimer = Time.time + retaliationTiming;
        base.DamageRecieved();
    }

    public override Vector3 GetDestinationPoint(float targetRange, bool hide) {
        bool tempVar = ableToHide;

        if (retaliationTimer > Time.time) {
            //Debug.Log("Working");
            tempVar = false;
            targetRange = (target.position - transform.position).magnitude;

            if (targetRange > range)
                targetRange = range;
        }

        //Debug.Log(retaliationTimer + " " + Time.time);

        return base.GetDestinationPoint(range, tempVar);
    }

    public void Attack() {
        animator.SetInteger("TreeState", 2);
        if (Time.time > attackTimer) {
            Transform targetHit = null;
            switch (attackType) {
                case WeaponType.RaycastShooting:
                    Vector3 offset;
                    offset = new Vector3(Random.Range(-gunSprayValue, gunSprayValue), Random.Range(-gunSprayValue, gunSprayValue), 0);
                    foreach (Transform gun in guns) {
                        gun.LookAt(target.position + weaponOffset);

                        gunEffect.transform.position = gun.position;
                        gunEffect.gameObject.SetActive(true);
                        gunEffect.ObjectActive();
                        StartCoroutine(ChangeObjectLocation(gunEffect.gameObject, gun.position + gun.TransformDirection(0, 0, range) + offset));

                        RaycastHit hit;
                        Debug.DrawRay(gun.position, gun.TransformDirection(0, 0, range) + offset, Color.red);
                        AIOverseer.instance.PlayRandomSound(AIOverseer.instance.gunShotList, transform.position);

                        if (Physics.Raycast(gun.position, gun.TransformDirection(0, 0, range) + offset, out hit))
                            if (hit.transform.CompareTag("NearPlayer"))
                                AIOverseer.instance.PlayRandomSound(AIOverseer.instance.flyByList, hit.point);

                        if (Physics.Raycast(gun.position, gun.TransformDirection(0, 0, range) + offset, out hit))
                            targetHit = hit.transform.root;
                    }
                    break;
                case WeaponType.Area:
                    Collider[] units = Physics.OverlapSphere(transform.position + transform.TransformDirection(0, 0, range), areaTestRadius);

                    foreach (Collider unit in units)
                        if (unit.transform.CompareTag("Player"))
                            targetHit = unit.transform;
                    break;
            }

            AIOverseer.instance.AIHostileRadius(transform.position, range);

            if (targetHit != null)
                if (targetHit != transform) {
                    //Health hp = targetHit.GetComponent<Health>();
                    AIFunctions ai;
                    //if (hp && hp.isActiveAndEnabled)
                        //hp.ReceiveDamage(damage);

                    if (targetHit.tag == "Player")
                        if (ableToDragPlayerOutOfCover) {
//CoverSystem inst = targetHit.GetComponent<CoverSystem>();
                           // if (inst)
                                //inst.EnableController();
                        }

                    if ((ai = targetHit.GetComponent<AIFunctions>()) != null)
                        ai.destination = GetDestinationPoint((ai as AI).range, (ai as AI).ableToHide);

                    attackTimer = Time.time + attackInterval;
                }
        }
    }
}
