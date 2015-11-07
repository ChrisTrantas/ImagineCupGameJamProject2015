using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class LizardAI : MonoBehaviour 
{
    [SerializeField] private float m_MaxSpeed = 10f; 

    private Rigidbody2D m_Rigidbody2D;
    private int direction = 1;

	// Use this for initialization
	void Awake () 
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
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
        //Debug.Log("speed" + m_MaxSpeed);
        //Debug.Log("direction" + direction);
        m_Rigidbody2D.velocity = new Vector2(m_MaxSpeed * direction, m_Rigidbody2D.velocity.y);
    }

    public void changeDirections()
    {
        direction *= -1;
    }
}
