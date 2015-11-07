using UnityEngine;
using System.Collections;

public class WanderZoneTriggerCode : MonoBehaviour 
{
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<LizardAI>().changeDirections();
        }
    }
}
