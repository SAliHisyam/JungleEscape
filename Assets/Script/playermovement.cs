using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    public float speed;

    [SerializeField] public float jumpPower;

    [SerializeField] float fallMultiplier;

    public Rigidbody2D rb;

    bool isGrounded = true;

    Vector2 vecGravity;


    // Start is called before the first frame update
    void Start()
    {
        speed = 20.0f;
        jumpPower = 40.0f;
        fallMultiplier = 5.0f;
        rb = GetComponent<Rigidbody2D>();
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
    }

    // Update is called once per frame
    void Update()
    {
        //maju
        rb.velocity = new Vector2(speed, rb.velocity.y);

        //loncat
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {

            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
        //turun dari loncat lebih cepat
        if(rb.velocity.y < 0)
        {
            rb.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
        }

    }
    //prevent jumping on the air
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
