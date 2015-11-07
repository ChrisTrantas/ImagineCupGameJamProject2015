using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets._2D;

public class LizardAI : MonoBehaviour 
{
    [SerializeField] private float m_MaxSpeed = 10f; 

    private Rigidbody2D m_Rigidbody2D;
    private PlatformerCharacter2D m_player;
    private int direction = 1;
    public int damage = 1;

	// Use this for initialization
	void Awake () 
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public void FixedUpdate()
    {
        move();
    }

    public void move()
    {
        m_Rigidbody2D.velocity = new Vector2(m_MaxSpeed * direction, m_Rigidbody2D.velocity.y);
    }

    public void changeDirections()
    {
        direction *= -1;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 collisionNormal = other.contacts[0].normal;
            m_player.dealDamage(damage, collisionNormal);
        }
    }
}
