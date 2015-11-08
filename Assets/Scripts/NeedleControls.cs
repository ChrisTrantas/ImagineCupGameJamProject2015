using UnityEngine;
using System.Collections;

public class NeedleControls : MovingObject {

    private PlatformerCharacter2D m_player;
    private int m_damage = 1;

	// Use this for initialization
	void Start () {
	
	}

    protected override void Awake()
    {
        base.Awake();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
    }

	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 collisionNormal = other.contacts[0].normal;
            if (m_player.IsShielding())
            {
                this.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.right * 10);
            }
            else if (!m_player.dealDamage(m_damage, collisionNormal))
            {
                AddKnockback(collisionNormal * -1);
                Destroy(this.gameObject, 0);
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
