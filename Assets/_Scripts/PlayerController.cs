using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 InputVector;
    [SerializeField] float baseSpeed = 3f;
    float speed;
    [SerializeField] private FieldOfView fieldOfView;
    private Rigidbody2D rb;

    private bool hidingTrigger;
    private GameObject hidingSpot;

    private enum State
    {
        Roaming, Hiding
    }
    State state = State.Roaming;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        speed = baseSpeed;
    }


    private void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (state)
        {
            case State.Roaming:
                Vector2 speedFactor = InputVector * Time.fixedDeltaTime * speed;
                rb.MovePosition(rb.position + speedFactor);
                break;
            case State.Hiding:
                break;
            default:
                break;
        }
        
        fieldOfView.SetOrigin(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HideSpot"))
        {
            hidingTrigger = true;
            hidingSpot = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("HideSpot"))
        {
            hidingTrigger = false;
        }
    }


    public void OnMovement(InputValue value)
    {
        InputVector = value.Get<Vector2>();
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed&& hidingTrigger)
        {
            if (state == State.Roaming)
            {
                state = State.Hiding;
                transform.position = hidingSpot.transform.position;
                rb.bodyType = RigidbodyType2D.Static;
            } 
            
        }

        /*if (value.isPressed && state == State.Hiding)
        {
            transform.position = new Vector3(30f, 10f);
            state = State.Roaming;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }*/
    }
}
