using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    public enum State
    {
        Roaming, Hiding
    }

    private State state = State.Roaming;
    [SerializeField] private Camera mainCam;
    [SerializeField] private float baseSpeed = 3f;
    [SerializeField] private float turnSpeed;
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private Light2D playerLight;

    Vector2 InputVector;

    private Vector3 previousPos;
    private Vector3 moveDir;

    private Rigidbody2D rb;
    private GameObject hidingSpot;

    private bool hidingTrigger;

    public bool seen = false;
    public Vector2Int mouseWorldPosInt;

    public Vector3 lastKnownPos;

    void Start()
    {
        previousPos = transform.position;
        moveDir = Vector3.zero;

        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //new Quaternion(0f, 0f, GetAngleFromVector(moveDir), 0f);
        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, moveDir);
        playerLight.transform.rotation = Quaternion.RotateTowards(playerLight.transform.rotation, toRotation, turnSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (transform.position != previousPos)
        {
            moveDir = (transform.position - previousPos).normalized;
            previousPos = transform.position;
        }

        switch (state)
        {
            case State.Roaming:
                lastKnownPos = transform.position;
                Vector2 speedFactor = InputVector * Time.fixedDeltaTime * baseSpeed;
                rb.MovePosition(rb.position + speedFactor);
                break;

            case State.Hiding:
                break;

            default:
                break;
        }

        fieldOfView.SetOrigin(transform.position);
    }

    public State GetState()
    {
        return state;
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

    public void OnShowTrapGrid(InputValue value)
    {
        if (value.isPressed)
        {
            GridDisplay.instance.ToggleGridDisplay();
        }
    }

    private float GetAngleFromVector(Vector3 vector)
    {
        vector = vector.normalized;

        float n = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

        if (n < 0) 
            n += 360;

        return n;
    }
}
