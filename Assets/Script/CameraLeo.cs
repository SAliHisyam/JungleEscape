using System.Collections;
using UnityEngine;

public class CameraLeo : MonoBehaviour
{
    public Transform targetCamera;
    public float smoothspeed;
    public Vector3 offset;
    public playermovement playerMovement;

   
    // Variable to check if the camera should follow the player
    private bool shouldFollowPlayer = true;

    void Start()
    {
        // Get a reference to the playermovement script
        playerMovement = targetCamera.GetComponent<playermovement>();
    }

    void LateUpdate()
    {
        if (playerMovement != null && shouldFollowPlayer)
        {
            Vector3 targetPosition = new Vector3(targetCamera.position.x + offset.x, transform.position.y, transform.position.z);

            // Check if Leo is jumping
            if (!playerMovement.IsGrounded())
            {
                // If jumping, the camera won't follow the vertical movement
                targetPosition.y = transform.position.y;
            }

            // Set the camera position directly to the target position
            transform.position = targetPosition;
        }
    }

    // Add this method to stop following the player
    public void StopFollowingPlayer()
    {
        shouldFollowPlayer = false;
    }
}
