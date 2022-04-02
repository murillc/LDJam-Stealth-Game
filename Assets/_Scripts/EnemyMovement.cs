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
}
