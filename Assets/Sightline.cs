using UnityEngine;
using System.Collections;

public class Sightline : MonoBehaviour 
{
    private LizardAI m_parentEnemy;

    void Awake()
    {
        m_parentEnemy = transform.parent.GetComponent<LizardAI>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            m_parentEnemy.ChangeChargeState(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            m_parentEnemy.ChangeChargeState(false);
        }
    }
}
