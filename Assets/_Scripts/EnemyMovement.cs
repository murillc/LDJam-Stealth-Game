using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = MoveSpeed; } }

    [SerializeField] private Transform prefabFieldOfView;
    private FieldOfView fieldOfView;
    [SerializeField] private float viewDistance = 8f;
    [SerializeField] private float fov = 40f;

    [SerializeField] private float aimAngle = 0f;
    [SerializeField] float rotationSpeed = 10f;

    private GameObject player;

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
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 enemyPos = transform.position;

        //transform.position = new Vector3(enemyPos.x + MoveSpeed * Time.deltaTime, enemyPos.y, enemyPos.z);

        UpdateViewCone();

        LocateTargetPlayer();

        HandleMovement();

    }

    private void UpdateViewCone()
    {
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetAimDirectionFloat(aimAngle);
        
    }

    private void LocateTargetPlayer()
    {
        // player in distance
        if (Vector2.Distance(transform.position, player.transform.position) < viewDistance)
        {
            // angle between enemy and player
            Vector3 vectorToPlayer = (player.transform.position - transform.position).normalized;
            

            // player within fov
            if (Vector3.Angle(vectorToPlayer, GetVectorFromAngle(aimAngle)) < fov / 2f) {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, vectorToPlayer, viewDistance);

                if (raycastHit2D.collider.gameObject.tag == "Player")
                {
                    Debug.Log(raycastHit2D.collider.gameObject.tag);
                }
                
                   
            }
        }
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

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;

        //THIS IS BECAUSE IF THE TRANSFORM IS NEGATIVE, IT DOES NOT WORK.
        Debug.Log("YOOOOOOOOOOOOOOOOOOOO: " + transform.position);

        pathVectorList = Pathfinding.Instance.FindPath(transform.position, targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            Debug.Log("pathVectorList NOT null");
            pathVectorList.RemoveAt(0);
        }
    }
}
