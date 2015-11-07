using UnityEngine;
using System.Collections;

public class RactusAI : MovingObject 
{
    public int sightRadius;

    public Rigidbody2D needleProjectile;
    private PlatformerCharacter2D m_player;
    private Rigidbody2D m_playerRigidBody2D;
    private Transform sightline;
    private Vector2 direction;


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
    
    private void CheckSightline()
    {
        if(Vector3.Distance(transform.position, m_player.gameObject.transform.position) <= sightRadius)
        {
            Rigidbody clone;
            clone = Instantiate(needleProjectile, transform.position, transform.rotation) as Rigidbody;
            clone.velocity = transform.TransformDirection(Vector3.forward * 10);
            
        }
    }

    public void FixedUpdate()
    {
    }

    public override void dealDamage(int dmg, Vector3 collisionNormal)
    {
        m_health -= dmg;
        CheckIfDead();
    }
}
