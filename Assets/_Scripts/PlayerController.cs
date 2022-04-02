using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 InputVector;
    [SerializeField] float speed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 speedFactor = InputVector * Time.deltaTime * speed;
        transform.position = (Vector2)transform.position + speedFactor;
        
    }

    void OnMovement(InputValue value)
    {
        InputVector = value.Get<Vector2>();
    }
}
