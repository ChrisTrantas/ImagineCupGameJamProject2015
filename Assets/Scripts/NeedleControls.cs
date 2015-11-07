using UnityEngine;
using System.Collections;

public class NeedleControls : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 collisionNormal = other.contacts[0].normal;
            //if (!m_player.dealDamage(m_damage, collisionNormal))
            //{
            //}
        }
    }
}
