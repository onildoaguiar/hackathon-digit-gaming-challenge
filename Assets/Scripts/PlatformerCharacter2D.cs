using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour
{
    [SerializeField] private float m_MaxSpeed = 200f;
    [SerializeField] private float m_JumpForce = 15000f;
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;

    private Transform m_GroundCheck;
    private bool m_Grounded;
    private Transform m_EdgePlatformCheck;
    private bool m_EdgePlatform;
    private Transform m_PlatformBelowCheck;
    private bool m_PlatformBelow;
    private Transform m_WallCheck;
    private bool m_Walled;
    private Transform m_BigWallCheck;
    private bool m_BigWalled;
    private Transform m_CanCrouchCheck;
    private bool m_CanCrouch;
    const float k_GroundedRadius = .2f;
    private Transform m_CeilingCheck;
    const float k_CeilingRadius = .01f;
    private Animator m_Anim;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    private GameController gameController;

    private void Awake()
    {
        // Setting references
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_EdgePlatformCheck = transform.Find("EdgePlatformCheck");
        m_PlatformBelowCheck = transform.Find("PlatformBelowCheck");
        m_WallCheck = transform.Find("WallCheck");
        m_BigWallCheck = transform.Find("BigWallCheck");
        m_CanCrouchCheck = transform.Find("CanCrouchCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        gameController = FindObjectOfType<GameController>();

    }

    public bool IsGrounded()
    {
        return m_Grounded;
    }

    public bool HasPlatform()
    {
        return m_EdgePlatform;
    }

    public bool HasPlatformBelow()
    {
        return m_PlatformBelow;
    }

    public bool IsWalled()
    {
        return m_Walled;
    }

    public bool IsBigWalled()
    {
        return m_BigWalled;
    }

    public bool CanCrouch()
    {
        return m_CanCrouch;
    }

    private void FixedUpdate()
    {
        m_Grounded = false;
        m_EdgePlatform = false;
        m_PlatformBelow = false;
        m_Walled = false;
        m_BigWalled = false;
        m_CanCrouch = true;

        // Check grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }
        m_Anim.SetBool("Ground", m_Grounded);

        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

        // Check edge platform
        colliders = Physics2D.OverlapCircleAll(m_EdgePlatformCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_EdgePlatform = true;
        }

        // Check platform below
        colliders = Physics2D.OverlapCircleAll(m_PlatformBelowCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_PlatformBelow = true;
        }

        // Check walled
        colliders = Physics2D.OverlapCircleAll(m_WallCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Walled = true;
        }

        // Check big walled
        colliders = Physics2D.OverlapCircleAll(m_BigWallCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_BigWalled = true;
        }

        // Check can crouch
        colliders = Physics2D.OverlapCircleAll(m_CanCrouchCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_CanCrouch = false;
        }
    }


    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch && m_Anim.GetBool("Crouch"))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        // Set whether or not the character is crouching in the animator
        m_Anim.SetBool("Crouch", crouch);

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            move = (crouch ? move * m_CrouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && jump && m_Anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // Triggers
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Exit":
                gameController.FinishDoor();
                break;
            case "KeyStage":
                gameController.GetKeyStage();
                break;
            case "MagicSword":
                gameController.GetMagicSword();
                break;
            default:
                break;
        }
    }
}

