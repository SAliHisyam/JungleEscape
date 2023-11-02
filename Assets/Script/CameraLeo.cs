using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLeo : MonoBehaviour
{
    public Transform targetCamera;
    public float smoothspeed;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 positioncamera = targetCamera.localPosition + offset;
        Vector3 smoothcamera = Vector3.Lerp(transform.position, positioncamera, smoothspeed);
        transform.position = smoothcamera;
    }
}
