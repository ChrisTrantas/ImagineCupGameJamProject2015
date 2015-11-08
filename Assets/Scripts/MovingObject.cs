using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public abstract class MovingObject : MonoBehaviour 
{
    [SerializeField] protected int m_health;
    [SerializeField] protected Vector2 m_knockbackForce = new Vector2(500, 500);
    [SerializeField] protected float m_MaxSpeed = 10;              // The fastest the player can travel in the x axis.
    [SerializeField] protected float m_knockbackTime = 0.1f;
    [SerializeField] protected float friction = 2f;
    [SerializeField] protected float speed = 400f;
    [SerializeField] protected bool m_AirControl = true;                 // Whether or not a player can steer while jumping;

    protected Rigidbody2D m_Rigidbody2D;
    protected bool m_FacingRight = true;  // For determining which way the player is currently facing.
    protected bool m_isKnockedback = false;
    protected Vector2 direction;
    protected bool m_Grounded = true;            // Whether or not the player is grounded.

    protected virtual void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        direction = m_Rigidbody2D.transform.right;
    }

    public virtual void Move(float move, bool jump)
    {
        if (!m_isKnockedback && (m_Grounded || m_AirControl))
        {
            m_Rigidbody2D.AddForce(m_Rigidbody2D.velocity * friction * -1);

            if (move * m_Rigidbody2D.velocity.x < m_MaxSpeed)
            {
                m_Rigidbody2D.AddForce(Vector2.right * move * speed);
            }

            if (Math.Abs(m_Rigidbody2D.velocity.x) > m_MaxSpeed)
            {
                m_Rigidbody2D.velocity = new Vector2(Mathf.Sign(m_Rigidbody2D.velocity.x) * m_MaxSpeed, m_Rigidbody2D.velocity.y);
            }

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
    }

    protected virtual void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        direction *= -1;
        //Debug.Log("flip direction " + direction);
    }

    public abstract bool dealDamage(int dmg, Vector3 collisionNormal);

    protected virtual void CheckIfDead()
    {
        if (m_health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void AddKnockback(Vector3 collisionNormal)
    {
        m_Rigidbody2D.velocity = Vector2.zero;
        m_Rigidbody2D.AddForce(new Vector2(-1 * collisionNormal.x * m_knockbackForce.x, m_knockbackForce.y));
        m_isKnockedback = true;

        if (gameObject.activeSelf)
        {
            StartCoroutine(StopKnockback(m_knockbackTime));
        }
    }

    protected IEnumerator StopKnockback(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (m_isKnockedback)
        {
            m_isKnockedback = false;
        }
    }
}
