using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2Int mouseWorldPosInt;
    [SerializeField] private Camera mainCam;

    public enum State
    {
        Roaming, Hiding
    }

    private State state = State.Roaming;

    [SerializeField] private float baseSpeed = 3f;
    [SerializeField] private FieldOfView fieldOfView;

    Vector2 InputVector;

    private float speed;
    private bool hidingTrigger;
    private Rigidbody2D rb;
    private GameObject hidingSpot;
    public bool seen = false;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        speed = baseSpeed;
    }

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

        if (collision.CompareTag("DocumentSearchZone"))
        {
            DocumentAltering.instance.inDocumentRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("HideSpot"))
        {
            hidingTrigger = false;
        }

        if (collision.CompareTag("DocumentSearchZone"))
        {
            DocumentAltering.instance.inDocumentRange = false;
        }
    }

    public void OnMovement(InputValue value)
    {
        InputVector = value.Get<Vector2>();
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed && state == State.Hiding)
        {
            Vector2 exitPoint = hidingSpot.GetComponent<HidingSpot>().GetExitPoint();
            transform.position = new Vector3(transform.position.x + exitPoint.x, transform.position.y + exitPoint.y);
            state = State.Roaming;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        if (value.isPressed && hidingTrigger)
        {
            if (state == State.Roaming)
            {
                state = State.Hiding;
                transform.position = hidingSpot.transform.position;
                rb.bodyType = RigidbodyType2D.Static;
            }
        }

        if (value.isPressed && DocumentAltering.instance.inDocumentRange)
        {
            Debug.Log("searching documents");
        }
    }

    public State GetState()
    {
        return state;
    }

    public void OnMousePosition(InputValue value)
    {
        Vector2 pos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPosInt = new Vector2Int((int)pos.x, (int)pos.y);
    }

    public void OnMouseClick(InputValue value)
    {
        if (value.isPressed)
        {
            TrapManager.instance.SpawnTrap(mouseWorldPosInt.x, mouseWorldPosInt.y);
        }
    }
}
