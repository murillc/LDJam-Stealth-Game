using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = MoveSpeed; }}

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 enemyPos = transform.position;

        transform.position = new Vector3(enemyPos.x + MoveSpeed * Time.deltaTime, enemyPos.y, enemyPos.z);
    }
}
