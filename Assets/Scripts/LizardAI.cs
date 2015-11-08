using System;
using UnityEngine;
using System.Collections;
//using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets._2D;

public class LizardAI : MovingObject 
{
    [SerializeField] private float m_ChargeSpeed = 20f; 
    [SerializeField] private float m_MaxChargeSpeed = 20f;
    [SerializeField] private int m_sightRadius = 10;
    [SerializeField] private float m_chargeCooldown = .5f;
    [SerializeField] private float m_stunTime = .5f;

    private PlatformerCharacter2D m_player;
    private Rigidbody2D m_playerRigidBody2D;
    private Transform sightline;
    private bool m_charging = false;
    private bool m_canCharge = true;
    private bool m_queuedDirectionChange = false;
    private int m_damage = 1;
    private bool m_stunned = false;
    

	// Use this for initialization
	protected override void Awake () 
    {
        base.Awake();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
        m_playerRigidBody2D = m_player.GetComponent<Rigidbody2D>();
        sightline = m_Rigidbody2D.transform.FindChild("Sightline").transform;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public void FixedUpdate()
    {
        Move(direction.x, false);
        if (m_queuedDirectionChange)
        {
            m_queuedDirectionChange = false;
            changeDirections();
        }
    }

    public override void Move(float move, bool jump)
    {
        if (!m_stunned)
        {
            if (m_charging)
            {
                m_Rigidbody2D.AddForce(m_Rigidbody2D.velocity * friction * -1);

                if (move * m_Rigidbody2D.velocity.x < m_MaxSpeed)
                {
                    m_Rigidbody2D.AddForce(Vector2.right * move * m_ChargeSpeed);
                }

                if (Math.Abs(m_Rigidbody2D.velocity.x) > m_MaxSpeed)
                {
                    m_Rigidbody2D.velocity = new Vector2(Mathf.Sign(m_Rigidbody2D.velocity.x) * m_MaxChargeSpeed, m_Rigidbody2D.velocity.y);
                }
            }
            else
            {
                base.Move(move, jump);
            }
        }
    }

    protected override void Flip()
    {
        if (!m_charging)
        {
            base.Flip();
        }
        else
        {
            m_queuedDirectionChange = true;
        }
    }

    public void changeDirections()
    {
        Flip();
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 collisionNormal = other.contacts[0].normal;
            if (!m_player.dealDamage(m_damage, collisionNormal))
            {
                AddKnockback(collisionNormal*-1);
                m_stunned = true;

                if (gameObject.active)
                {
                    StartCoroutine(RemoveStun(m_stunTime));
                }
            }
            ChangeChargeState(false);
        }
    }

    public void ChangeChargeState(bool charging)
    {
        Debug.Log("changing charge state");
        if (!charging)
        {
            m_canCharge = false;
            m_charging = charging;

            if (gameObject.active)
            {
                StartCoroutine(ChargeOffCooldown(m_chargeCooldown));
            }
        }
        if (charging && m_canCharge && !m_charging)
        {
            m_charging = charging;
        }
    }

    IEnumerator ChargeOffCooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        m_canCharge = true;
    }

    IEnumerator RemoveStun(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        m_stunned = false;
    }

    public override bool dealDamage(int dmg, Vector3 collisionNormal)
    {
        m_health -= dmg;
        CheckIfDead();
        return true;
    }

    protected override void AddKnockback(Vector3 collisionNormal)
    {
        m_Rigidbody2D.velocity = Vector2.zero;
        m_Rigidbody2D.AddForce(new Vector2(Mathf.Sign(collisionNormal.x) * -1 * m_knockbackForce.x, 0));
        m_isKnockedback = true;

        if (gameObject.active)
        {
            StartCoroutine(StopKnockback(m_knockbackTime));
        }
    }
}
