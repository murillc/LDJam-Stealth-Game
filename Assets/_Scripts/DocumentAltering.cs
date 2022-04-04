using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DocumentAltering : MonoBehaviour
{
    [SerializeField] private Image slider;

    [SerializeField] public bool altered = false;
    [SerializeField] public bool isbeingAltered;
    [SerializeField] public bool inDocumentRange;

    [SerializeField] public float timeToAlter;
    [SerializeField] public float currentAlterTime;

    private void Start()
    {
        currentAlterTime = 0;
    }

    private void Update()
    {
        if (isbeingAltered)
        {
            if (currentAlterTime < timeToAlter)
                currentAlterTime += Time.deltaTime;

            slider.fillAmount = currentAlterTime / timeToAlter;
        }
    }

    public void AlterDocuments()
    {
        isbeingAltered = true;
    }

    public void StopAltering()
    {
        isbeingAltered = false;
        currentAlterTime = 0;
        slider.fillAmount = currentAlterTime / timeToAlter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inDocumentRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inDocumentRange = false;
            StopAltering();
        }
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed && inDocumentRange)
        {
            AlterDocuments();
        }
    }
}
