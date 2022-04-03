using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;

    [SerializeField] private float _moveSpeed;
    
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = MoveSpeed; } }

    [SerializeField] private Transform prefabFieldOfView;
    private FieldOfView fieldOfView;
    [SerializeField] private float viewDistance = 8f;
    [SerializeField] private float fov = 40f;

    [SerializeField] private float aimAngle = 0f;
    [SerializeField] float rotationSpeed = 10f;

    private const float detectionTime = 10f;
    private float timer = 0f;

    private const float trapTime = 5f;

    private GameObject player;

    // Player hiding variables
    private bool playerHiding = false;
    private bool seenPlayerNotHiding = false;

    private bool inSight = false;

    public enum State
    {
        Roaming, Chasing, Confused, Retreating, Trapped
    }
    private State state = State.Roaming;

    [SerializeField] private Vector3 _moveTarget;
    public Vector3 MoveTarget { get { return _moveTarget; } set { _moveTarget = MoveTarget; } }

    //Pathfinding
    private int currentPathIndex;
    private List<Vector3> pathVectorList;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fieldOfView = Instantiate(prefabFieldOfView).GetComponent<FieldOfView>();

        fieldOfView.SetFOV(fov);
        fieldOfView.SetAimDirectionFloat(aimAngle);
        fieldOfView.SetViewDistance(viewDistance);
        fieldOfView.SetOrigin(transform.position);

        SetTargetPosition(_moveTarget);
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 enemyPos = transform.position;

        //transform.position = new Vector3(enemyPos.x + MoveSpeed * Time.deltaTime, enemyPos.y, enemyPos.z);
        if (player.GetComponent<PlayerController>().GetState() == PlayerController.State.Hiding)
        {
            playerHiding = true;
        } else
        {
            playerHiding = false;
        }

        switch (state)
        {
            case State.Roaming:
            {
                    SetTargetPosition(_moveTarget);
                    if (LocateTargetPlayer()) { state = State.Chasing; }
                    break;
            }
            case State.Chasing:
            {
                    SetTargetPosition(player.transform.position);
                    aimAngle = GetAngleFromVector((player.transform.position - transform.position).normalized);

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
                            } else
                            {
                                seenPlayerNotHiding = true;
                                Debug.Log("Seen player not hiding");
                            }
                        }
                        // continues to be in sight
                        else if (inSight) {
                            if (playerHiding)
                            {
                                if (seenPlayerNotHiding)
                                {
                                    Debug.Log("Keep going!");
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
                        
                        
                    } else
                    {
                        if (timer > 0.5f)
                        {
                            Debug.Log("Lost sight...");
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
                    // Stay still
                    SetTargetPosition(transform.position);
                    break;
                }
            case State.Retreating:
                {
                    // TODO make him retreat to an entry / exit point
                    SetTargetPosition(new Vector3(31f, 4f));
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
        }
    }

    IEnumerator Retreat()
    {
        yield return new WaitForSeconds(trapTime);
        state = State.Retreating;
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
            if (Vector3.Angle(vectorToPlayer, GetVectorFromAngle(aimAngle)) < fov / 2f) {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, vectorToPlayer, viewDistance);

                
                if (raycastHit2D.collider != null)
                {
                    Debug.Log(raycastHit2D.collider.name);
                    if (raycastHit2D.collider.CompareTag("Player"))
                    {
                        Debug.Log("see player");
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
            Debug.Log("pathVectorList null");
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
            
        }
        else
        {
            currentPathIndex++;
            
            if (currentPathIndex >= pathVectorList.Count)
            {
                pathVectorList = null;
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
