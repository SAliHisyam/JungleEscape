using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FordwardLeo : MonoBehaviour
{
    public float kecepatan = 2.0f;
    Rigidbody2D rb;
    public float jumPing = 300;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 gerakan = transform.right * kecepatan * Time.deltaTime;
        transform.Translate(gerakan, Space.World);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumPing);
        }
    }
}