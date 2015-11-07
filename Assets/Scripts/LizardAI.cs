﻿using UnityEngine;
using System.Collections;
//using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets._2D;

public class LizardAI : MovingObject 
{
    [SerializeField] private float m_ChargeSpeed = 20f; 
    [SerializeField] private int m_sightRadius = 10;
    [SerializeField] private float m_chargeCooldown = .5f;

    private PlatformerCharacter2D m_player;
    private Rigidbody2D m_playerRigidBody2D;
    private Transform sightline;
    private Vector2 direction;
    private bool m_charging = false;
    private bool m_canCharge = true;
    private bool m_queuedDirectionChange = false;
    private int m_damage = 1;
    

	// Use this for initialization
	protected override void Awake () 
    {
        base.Awake();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
        m_playerRigidBody2D = m_player.GetComponent<Rigidbody2D>();
        direction = m_Rigidbody2D.transform.right;
        sightline = m_Rigidbody2D.transform.FindChild("Sightline").transform;
        m_health = 1;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public void FixedUpdate()
    {
        move();
        if (m_queuedDirectionChange)
        {
            m_queuedDirectionChange = false;
            changeDirections();
        }
    }

    public void move()
    {
        if (m_charging)
        {
            m_Rigidbody2D.velocity = new Vector2(m_ChargeSpeed * direction.x, m_Rigidbody2D.velocity.y);
        }
        else
        {
            m_Rigidbody2D.velocity = new Vector2(m_MaxSpeed * direction.x, m_Rigidbody2D.velocity.y);
        }
    }

    public void changeDirections()
    {
        if (!m_charging)
        {
            direction *= -1;
            Vector3 position = sightline.localPosition;
            position.x *= -1;
            sightline.localPosition = position;
        }
        else
        {
            m_queuedDirectionChange = true;
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 collisionNormal = other.contacts[0].normal;
            m_player.dealDamage(m_damage, collisionNormal);
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
            StartCoroutine(ChargeOffCooldown(m_chargeCooldown));
        }
        if (m_canCharge && charging)
        {
            m_charging = charging;
        }
    }

    IEnumerator ChargeOffCooldown(float waittTime)
    {
        yield return new WaitForSeconds(waittTime);
        m_canCharge = true;
    }

    public override void dealDamage(int dmg, Vector3 collisionNormal)
    {
        m_health -= dmg;
        CheckIfDead();
    }
}
