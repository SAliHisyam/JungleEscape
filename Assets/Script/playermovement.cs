using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class playermovement : MonoBehaviour
{
    public Animator animator;
    public float speed;
    public float jumpPower;
    public float fallMultiplier;
    public Rigidbody2D rb;

    public AudioSource footStepSoundEffect;
    public AudioSource jumpSoundEffect;

    private bool isGrounded = true;
    private bool isPlayingFootstep = false;
    private float originalFootstepVolume;

    private bool isGamePaused = false;
    private bool isPauseButtonClicked = false;
    private bool isCharacterAllowedToMove = true;

    // Cooldown duration in seconds
    float jumpCooldownDuration = 0.5f;
    bool isJumpCooldownActive = false;



    // Start is called before the first frame update
    void Start()
    {
        speed = 30.0f;
        jumpPower = 40.0f;
        fallMultiplier = 5.0f;
        rb = GetComponent<Rigidbody2D>();

        // Set the original footstep volume to 0.5 (50% volume)
        originalFootstepVolume = 0.5f;

        // Set the initial volume of footstep sound
        footStepSoundEffect.volume = originalFootstepVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGamePaused && isCharacterAllowedToMove)
        {
            // maju
            rb.velocity = new Vector2(speed, rb.velocity.y);

            // play footstep sound when running
            if (isGrounded && Mathf.Abs(rb.velocity.x) > 0.2f && !isPlayingFootstep)
            {
                footStepSoundEffect.Play();
                isPlayingFootstep = true;
            }
            else if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                isPlayingFootstep = false;
            }

            // loncat
            if (ShouldJump())
            {
                Jump();
            }

            // turun dari loncat lebih cepat
            if (rb.velocity.y < 0)
            {
                rb.velocity -= Vector2.up * fallMultiplier * Time.deltaTime;
            }

            if (isGrounded)
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isrunning", true);
                // Restore footstep volume when grounded
                footStepSoundEffect.volume = originalFootstepVolume;
            }
        }
        else
        {
            // Reset the flag when the game is paused
            isPauseButtonClicked = false;
        }
    }

    // prevent jumping on the air
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

    // Add this method to check if Leo is grounded
    public bool IsGrounded()
    {
        return isGrounded;
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("isrunning", true);
    }

    // Extracted the jump functionality to a separate method
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        animator.SetBool("isJumping", true);
        animator.SetBool("isrunning", false);
        isGrounded = false;
        jumpSoundEffect.Play();
        // Disable footstep sound while jumping
        isPlayingFootstep = false;
        // Mute footstep sound during jump
        footStepSoundEffect.volume = 0.0f;
    }

    // Method to check if the screen is being touched
    bool IsTouching()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            return touch.phase == TouchPhase.Began;
        }
        return false;
    }

    // Method to check if the platform is PC
    bool IsPC()
    {
        return Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
    }

    // Method to check if the platform is mobile
    bool IsMobile()
    {
        return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }

    // Method to pause the game
    public void PauseGame()
    {
        isGamePaused = true;
        // Mute footstep sound when the game is paused
        footStepSoundEffect.volume = 0.0f;

        // Stop footstep sound immediately
        this.StopFootstepSound();

        // Disable character movement and sound
        ToggleCharacterMovementAndSound(false);

        // Add additional pause logic if needed
    }

    // Custom method to stop footstep sound
    private void StopFootstepSound()
    {
        footStepSoundEffect.Stop();
    }

    // Method to resume the game
    public void ResumeGame()
    {
        isGamePaused = false;
        // Restore footstep volume when the game is resumed
        footStepSoundEffect.volume = originalFootstepVolume;
        // Resume footstep sound when the game is resumed
        footStepSoundEffect.Play();
        // Enable character movement and sound
        ToggleCharacterMovementAndSound(true);
        // Add additional resume logic if needed
    }

    // Example method for handling button click (attach this to your button's OnClick event)
    public void OnOptionButtonClick()
    {
        // Assuming your option button has the tag "option"
        if (IsMobile() && IsPointerOverOptionButton() && !isPauseButtonClicked)
        {
            isPauseButtonClicked = true;
            PauseGame();
            // Stop footstep sound when the option button is clicked
            StopFootstepSound();
            // Add additional logic related to the option button click if needed
        }
    }

    // Example method for handling button click (attach this to your button's OnClick event)
    public void OnResumeButtonClick()
    {
        // Assuming your resume button has the tag "resume"
        if (IsMobile() && IsPointerOverResumeButton() && isPauseButtonClicked)
        {
            isPauseButtonClicked = false;
            ResumeGame();
            // Add additional logic related to the resume button click if needed
        }
    }

    private void ToggleCharacterMovementAndSound(bool allowMovementAndSound)
    {
        isCharacterAllowedToMove = allowMovementAndSound;

        // Stop footstep sound immediately
        StopFootstepSound();

        // Stop any ongoing jump only if character is allowed to move
        if (allowMovementAndSound)
        {
            StopJump();
        }

        // Reset jump cooldown only if character is allowed to move
        if (allowMovementAndSound)
        {
            isJumpCooldownActive = false;
        }

        // Reset footstep flag
        isPlayingFootstep = false;

        // Reset animator parameters
        animator.SetBool("isJumping", false);
        animator.SetBool("isrunning", false);

        // Restore footstep volume
        footStepSoundEffect.volume = originalFootstepVolume;

        // Resume footstep sound if allowed
        if (allowMovementAndSound)
        {
            footStepSoundEffect.Play();
        }
    }

    // Custom method to stop any ongoing jump
    private void StopJump()
    {
        if (isGrounded) // Only stop jump if the character is currently grounded
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }

    // Method to check if the tap occurred on the "option" button
    bool IsPointerOverOptionButton()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("option"))
            {
                return true;
            }
        }

        return false;
    }

    bool IsPointerOverResumeButton()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("resume"))
            {
                return true;
            }
        }

        return false;
    }

    // Custom method to check if the character should jump
    // Custom method to check if the character should jump
    // Custom method to check if the character should jump
    bool ShouldJump()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            // Check if any button is pressed on PC
            if (IsPC() && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
            {
                // Check if the tap occurred on the "option" button
                if (!IsPointerOverOptionButton())
                {
                    if (!isGrounded)
                    {
                        // Add any additional conditions here if needed
                        return false;
                    }
                    else
                    {
                        // Character should jump only if not within the cooldown period
                        if (!IsJumpCooldownActive())
                        {
                            StartCoroutine(StartJumpCooldown());
                            return true;
                        }
                    }
                }
            }
            // Check if any touch is detected on mobile
            else if (IsMobile() && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // Check if the tap occurred on the "option" or "resume" button
                if (!IsPointerOverOptionButton() && !IsPointerOverResumeButton())
                {
                    if (!isGrounded)
                    {
                        // Add any additional conditions here if needed
                        return false;
                    }
                    else
                    {
                        // Character should jump only if not within the cooldown period
                        if (!IsJumpCooldownActive())
                        {
                            StartCoroutine(StartJumpCooldown());
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }


    // Method to check if the jump cooldown is active
    bool IsJumpCooldownActive()
    {
        return isJumpCooldownActive;
    }

    // Coroutine to start the jump cooldown
    IEnumerator StartJumpCooldown()
    {
        isJumpCooldownActive = true;
        yield return new WaitForSeconds(jumpCooldownDuration);
        isJumpCooldownActive = false;
    }
}
