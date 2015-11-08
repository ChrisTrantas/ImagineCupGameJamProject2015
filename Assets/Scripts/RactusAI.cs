using UnityEngine;
using System.Collections;

public class RactusAI : MovingObject 
{
    public int sightRadius;
    public float cooldownTime;
    private float timer = 0;
    private bool canAttack = true;

    private PlatformerCharacter2D m_player;
    private int m_damage = 1;
    public GameObject needle;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
        direction = m_Rigidbody2D.transform.right;
    }
    
    public void FixedUpdate()
    {
        if(canAttack)
        {
            CheckSightline();
        }
        else
        {
            timer += (0.5f * Time.deltaTime);
            if(timer >= cooldownTime)
            {
                canAttack = true;
                timer = 0;
            }
        }
    }

    private void CheckSightline()
    {
        if(Vector3.Distance(transform.position, m_player.gameObject.transform.position) <= sightRadius)
        {
            GameObject clone = (GameObject) Instantiate(needle);
            clone.transform.position = transform.position;
            Destroy(clone.gameObject, 4.0f);
            canAttack = false;
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 collisionNormal = other.contacts[0].normal;
            if (!m_player.dealDamage(m_damage, collisionNormal))
            {
                AddKnockback(collisionNormal * -1);
            }
        }
    }

    public override bool dealDamage(int dmg, Vector3 collisionNormal)
    {
        m_health -= dmg;
        CheckIfDead();
        AddKnockback(collisionNormal);
        return true;
    }
}
