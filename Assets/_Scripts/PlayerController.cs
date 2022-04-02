using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 InputVector;
    [SerializeField] float speed = 3f;
    [SerializeField] private FieldOfView fieldOfView;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 speedFactor = InputVector * Time.fixedDeltaTime * speed;
        rb.MovePosition(rb.position + speedFactor);
        fieldOfView.SetOrigin(transform.position);
    }


    void OnMovement(InputValue value)
    {
        InputVector = value.Get<Vector2>();
    }
}
