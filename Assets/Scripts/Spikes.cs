using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour 
{
    public int damage = 1;

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<MovingObject>().dealDamage(damage, other.contacts[0].normal);
        }
    }
}
