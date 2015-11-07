using UnityEngine;
using System.Collections;

public class RactusAI : MovingObject 
{
    public int sightRadius;
    public float cooldownTime;
    private float timer = 0;
    private bool canAttack = true;

    public Rigidbody2D needleProjectile;
    private PlatformerCharacter2D m_player;
    private Rigidbody2D m_playerRigidBody2D;
    private int m_damage = 1;
    private Transform sightline;
    private Vector2 direction;

    void Start()
    {
        //needleProjectile = GameObject.FindGameObjectWithTag("Needle").GetComponent<Rigidbody2D>();

    }
    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
        m_playerRigidBody2D = m_player.GetComponent<Rigidbody2D>();
        direction = m_Rigidbody2D.transform.right;
        m_health = 1;
    }

    // Update is called once per frame
    void Update()
    {
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
            Rigidbody2D clone;
            clone = Instantiate(needleProjectile, transform.position - transform.right * 1.25f, transform.rotation) as Rigidbody2D;
            clone.velocity = transform.TransformDirection(-Vector3.right * 10);
            Destroy(clone.gameObject, 4.0f);
            canAttack = false;
        }
    }

    protected virtual void CheckIfDead()
    {
        if (m_health <= 0)
        {
            gameObject.SetActive(false);
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
        return true;
    }
}
