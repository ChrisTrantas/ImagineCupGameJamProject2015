using UnityEngine;
using System.Collections;

public class exitLevel : MonoBehaviour {

    public int index;
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player") 
        {
            Application.LoadLevel(index+1);
        }
    }
}
