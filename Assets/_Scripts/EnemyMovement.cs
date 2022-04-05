using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private GameObject exclamationMark;

    [SerializeField] private Transform prefabFieldOfView;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _chaseSpeed;
    [SerializeField] private float viewDistance;
    [SerializeField] private float fov;
    [SerializeField] private float aimAngle;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float trapTime;
    [SerializeField] private float detectionTime;

    [SerializeField] private bool reachedDestination = false;

    [SerializeField] private Light2D enemyLight;
    [SerializeField] private float turnSpeed;

    [SerializeField] private AudioSource deadAudio;

    private Coroutine yoyo;

    private GameObject player;
    private FieldOfView fieldOfView;
    private float timer = 0f;

    // Player hiding variables
    private bool playerHiding = false;
    [SerializeField] private bool seenPlayerNotHiding = false;
    [SerializeField] private bool inSight = false;

    public Vector3 spawnPoint;

    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = MoveSpeed; } }

    public enum State
    {
        Roaming, Chasing, Confused, Retreating, Trapped
    }
    [SerializeField] private State state = State.Roaming;

    [SerializeField] private Vector3 _moveTarget;
    public Vector3 MoveTarget { get { return _moveTarget; } set { _moveTarget = MoveTarget; } }

    //Pathfinding
    private int currentPathIndex;
    private List<Vector3> pathVectorList;

    // Start is called before the first frame update
    void Start()
    {
        _moveSpeed = _baseSpeed;

        transform.position = spawnPoint;

        player = GameObject.FindGameObjectWithTag("Player");
        fieldOfView = Instantiate(prefabFieldOfView).GetComponent<FieldOfView>();

        fieldOfView.SetFOV(fov);
        fieldOfView.SetAimDirectionFloat(aimAngle);
        fieldOfView.SetViewDistance(viewDistance);
        fieldOfView.SetOrigin(transform.position);

        SetTargetPosition(_moveTarget);

        EnemyUI.instance.caughtText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 enemyPos = transform.position;

        //transform.position = new Vector3(enemyPos.x + MoveSpeed * Time.deltaTime, enemyPos.y, enemyPos.z);
        if (player.GetComponent<PlayerController>().GetState() == PlayerController.State.Hiding)
        {
            playerHiding = true;
        }
        else
        {
            playerHiding = false;
        }

        switch (state)
        {
            case State.Roaming:
                {
                    _moveSpeed = _baseSpeed;

                    exclamationMark.SetActive(false);

                    SetTargetPosition(_moveTarget);

                    if (reachedDestination)
                    {
                        _moveTarget = Pathfinding.Instance.GetRandomWalkableNode().GetPosition();
                        reachedDestination = false;
                    }

                    if (LocateTargetPlayer()) { state = State.Chasing; }
                    break;
                }
            case State.Chasing:
                {
                    _moveSpeed = _chaseSpeed;

                    exclamationMark.SetActive(true);

                    Vector3 lastKnownPlayerPos = player.GetComponent<PlayerController>().lastKnownPos;
                    SetTargetPosition(lastKnownPlayerPos);
                    aimAngle = GetAngleFromVector((lastKnownPlayerPos - transform.position).normalized);

                    if (LocateTargetPlayer())
                    {
                        // On first sighting 
                        if (!inSight)
                        {
                            inSight = true;

                            // If player was hiding on first sighting
                            if (playerHiding)
                            {
                                seenPlayerNotHiding = false;
                            }
                            else
                            {
                                seenPlayerNotHiding = true;
                                // Debug.Log("Seen player not hiding");
                            }
                        }
                        // continues to be in sight
                        else if (inSight)
                        {
                            if (playerHiding)
                            {
                                if (seenPlayerNotHiding)
                                {
                                    //Debug.Log("Keep going!");
                                    // TODO speed up
                                }
                                else
                                {
                                    state = State.Confused;
                                }
                            }
                            else
                            {

                                timer = 0;
                            }
                        }


                    }
                    else
                    {
                        if (timer > 0.5f)
                        {
                            //Debug.Log("Lost sight...");
                            inSight = false;
                        }
                        timer += Time.deltaTime;
                    }

                    if (timer > detectionTime)
                    {
                        state = State.Roaming;
                    }
                    break;
                }
            case State.Confused:
                {
                    _moveSpeed = _baseSpeed;

                    exclamationMark.SetActive(false);

                    SetTargetPosition(transform.position);

                    aimAngle -= rotationSpeed * Time.deltaTime;

                    if (LocateTargetPlayer())
                    {
                        if (!playerHiding)
                        {
                            timer = 0;
                            state = State.Chasing;
                        }
                    }
                    else
                    {

                        timer += Time.deltaTime;
                    }

                    if (timer > detectionTime)
                    {
                        state = State.Roaming;
                    }
                    break;
                }
            case State.Trapped:
                {
                    _moveSpeed = _baseSpeed;

                    // Stay still
                    SetTargetPosition(transform.position);
                    break;
                }
            case State.Retreating:
                {
                    _moveSpeed = _baseSpeed;

                    // TODO make him retreat to an entry / exit point
                    SetTargetPosition(spawnPoint);
                    if (LocateTargetPlayer()) { state = State.Chasing; }
                    break;
                }
            default: break;
        }


        HandleMovement();
        // Must update view cone after handling movement
        UpdateViewCone();



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Trap")
        {
            // TODO play sound scream
            state = State.Trapped;
            StartCoroutine(Retreat());
            deadAudio.Play(0);
            GameObject.Destroy(collision.gameObject);
        }
    }

    IEnumerator Retreat()
    {
        yield return new WaitForSeconds(trapTime);
        state = State.Retreating;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // TODO let player know they've been caught
            PlayerStats.instance.AddHeat(35);
            state = State.Trapped;
            EnemyUI.instance.caughtText.SetActive(true);
            yoyo = StartCoroutine(Caught());

            DayNightCycle.instance.SwitchCycle();
            Destroy(this.gameObject);
        }
    }

    IEnumerator Caught()
    {
        yield return new WaitForSeconds(5f);
        DayNightCycle.instance.SetCycle(DayNightCycle.CycleEnum.DAY);
        EnemyUI.instance.caughtText.SetActive(false);
    }
    


    private void UpdateViewCone()
    {
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetAimDirectionFloat(aimAngle);
    }

    private bool LocateTargetPlayer()
    {
        // player in distance
        if (Vector2.Distance(transform.position, player.transform.position) < viewDistance)
        {
            // angle between enemy and player
            Vector3 vectorToPlayer = (player.transform.position - transform.position).normalized;

            // player within fov
            if (Vector3.Angle(vectorToPlayer, GetVectorFromAngle(aimAngle)) < fov / 2f)
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, vectorToPlayer, viewDistance);

                if (raycastHit2D.collider != null)
                {
                    //Debug.Log(raycastHit2D.collider.name);
                    if (raycastHit2D.collider.CompareTag("Player"))
                    {
                        //Debug.Log("see player");
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void HandleMovement()
    {
        if (pathVectorList == null)
        {
            //Debug.Log("pathVectorList null");
            return;
        }

        Vector3 targetPosition = pathVectorList[currentPathIndex];

        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector3 moveDir = (targetPosition - transform.position).normalized;

            float distanceBefore = Vector3.Distance(transform.position, targetPosition);

            transform.position = transform.position + moveDir * MoveSpeed * Time.deltaTime;

            if (state == State.Roaming || state == State.Retreating)
            {
                aimAngle = GetAngleFromVector(moveDir);
            }

            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, moveDir);
            enemyLight.transform.rotation = Quaternion.RotateTowards(enemyLight.transform.rotation, toRotation, turnSpeed * Time.deltaTime);

        }
        else
        {
            currentPathIndex++;

            if (currentPathIndex >= pathVectorList.Count)
            {
                pathVectorList = null;
                reachedDestination = true;
            }
        }

       
    }

    private void FixedUpdate()
    {
        // TODO replace with actual movement pattern
        //aimAngle -= rotationSpeed * Time.fixedDeltaTime;
    }

    private static Vector3 GetVectorFromAngle(float angleDeg)
    {
        float angleRad = angleDeg * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private static float GetAngleFromVector(Vector3 vector)
    {
        vector = vector.normalized;
        float n = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;

        pathVectorList = Pathfinding.Instance.FindPath(transform.position, targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

    public void SetState(State state)
    {
        this.state = state;
    }
}
