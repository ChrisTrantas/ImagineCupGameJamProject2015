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
        Debug.Log("player " + m_player.transform.position + " Needle " + this.transform.position);
    }

    void FixedUpdate()
    {
        Move(direction.x, false);
        Debug.Log("updating " +m_player.transform.position+" needle "+ transform.position);
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
            MovingObject obj = other.GetComponent<MovingObject>();
            obj.dealDamage(m_damage, -direction);
            Destroy(this.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //m_canKill = true;
    }

    public override bool dealDamage(int dmg, Vector3 collisionNormal)
    {
        return false;
    }

    public void SetDirection(Vector2 pos)
    {
        direction = pos;
        m_FacingRight = direction.x > 0;
    }
}
