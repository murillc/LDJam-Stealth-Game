using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x + offset.x, playerTransform.position.y + offset.y, offset.z);
    }
}
