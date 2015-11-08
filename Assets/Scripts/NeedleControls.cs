using UnityEngine;
using System.Collections;

public class NeedleControls : MovingObject
{
    private PlatformerCharacter2D m_player;
    private int m_damage = 1;
    private bool m_canKill = false;

    protected override void Awake()
    {
        base.Awake();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
        if(m_player.transform.position.x < transform.position.x)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
            m_FacingRight = false;
        }
    }

    void FixedUpdate()
    {
        Move(direction.x, false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 collisionNormal = new Vector3(-direction.x, 0, 0);
            if (!m_player.dealDamage(m_damage, collisionNormal))
            {
                Flip();
                m_canKill = true;
            }
            else
            {
                AddKnockback(collisionNormal * -1);
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && m_canKill)
        {
            other.GetComponent<MovingObject>().dealDamage(m_damage, direction);
            Destroy(this.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        m_canKill = true;
    }

    public override bool dealDamage(int dmg, Vector3 collisionNormal)
    {
        return false;
    }
}
