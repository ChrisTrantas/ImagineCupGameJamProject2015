using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour 
{
    [SerializeField] protected int m_health;
    [SerializeField] protected Vector2 m_knockbackForce = new Vector2(500, 500);
    [SerializeField] protected float m_MaxSpeed;              // The fastest the player can travel in the x axis.
    [SerializeField] protected float m_knockbackTime = 0.1f;

    protected Rigidbody2D m_Rigidbody2D;
    protected bool m_FacingRight = true;  // For determining which way the player is currently facing.
    protected bool m_isKnockedback = false;

    protected virtual void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public abstract void dealDamage(int dmg, Vector3 collisionNormal);

    protected virtual void CheckIfDead()
    {
        if (m_health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void AddKnockback(Vector3 collisionNormal)
    {
        m_Rigidbody2D.AddForce(new Vector2(-1 * collisionNormal.x * m_knockbackForce.x, m_knockbackForce.y));
        m_isKnockedback = true;
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
