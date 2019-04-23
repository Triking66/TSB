using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraController : MonoBehaviour
{
    public Transform target;
    public float smoothness = 0.125f;
    public Vector3 offset;
    Vector3 velocity = new Vector3(1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.transform.position + offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothness);
            transform.position = smoothedPosition;
            //transform.LookAt(target);
        }
    }
}
