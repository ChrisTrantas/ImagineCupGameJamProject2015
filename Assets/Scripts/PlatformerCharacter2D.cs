using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class PlatformerCharacter2D : MovingObject
{
    
    [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
    [Range(0, 1)]
    [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
    [SerializeField] private float m_immuneTime = 2;
    

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator m_Anim;            // Reference to the player's animator component.
    private bool m_shielding = false;
    private bool m_isImmune = false;
    

    protected override void Awake()
    {
        base.Awake();
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Anim = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }
        m_Anim.SetBool("Ground", m_Grounded);

        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        //m_Rigidbody2D.AddForce(new Vector2(-1 * 1 * m_knockbackForce.x, m_knockbackForce.y));
    }


    public override void Move(float move, bool jump)
    {
        if (m_shielding)
        {
            m_Anim.SetFloat("Speed", 0);
            return;
        }

        //only control the player if grounded or airControl is turned on
        if (!m_isKnockedback && (m_Grounded || m_AirControl))
        {
            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(move));

            base.Move(move, jump);
        }
        // If the player should jump...
        if (m_Grounded && jump && m_Anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }

        if (m_isKnockedback && m_Rigidbody2D.velocity.y < 0 && m_Grounded)
        {
            m_isKnockedback = false;
        }
    }


    

    public override bool dealDamage(int dmg, Vector3 collisionNormal)
    {
        if (m_shielding)
        {
            m_Rigidbody2D.velocity = Vector2.zero;
            return false;
        }

        if(m_isImmune)
        {
            return true;
        }

        m_health -= dmg;
        Debug.Log("Ouch, I took " + dmg + " damage");

        CheckIfDead();

        m_isImmune = true;
        //m_Rigidbody2D.velocity = Vector2.zero;
        AddKnockback(collisionNormal);

        if (gameObject.activeSelf) 
		{
			
			StartCoroutine (LoseImmunity (m_immuneTime));
		}
        m_Anim.SetFloat("Speed", 0);

        return true;
    }

    public void changeShieldState(bool shieldState)
    {
        m_shielding = shieldState;
        if (m_shielding)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    IEnumerator LoseImmunity(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Im mortal!");
        m_isImmune = false;
    }

    public bool IsImmune()
    {
        return m_isImmune;
    }

    public bool IsShielding()
    {
        return m_shielding;
    }
}
