using UnityEngine.Assertions;
using UnityEngine;
using TMPro;
using System.Collections;

public class BallPhysics : MonoBehaviour
{
   
    private Rigidbody m_rb = null;
    public AudioSource snd;
  
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_rb.velocity = Vector3.zero;
        m_rb.rotation = Quaternion.identity;

    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //m_rb.velocity = Vector3.zero;
        //m_rb.MovePosition(startPosition);
        
        if (other.gameObject.tag == "Boundary")
        {         
            Debug.Log("boundary");     
        }
        else
        {
            if (other.gameObject.tag == "Goal" && projectile.kickBall)
            {
                Debug.Log("goal");
                snd.Play();
                projectile.score++;
                projectile.kickBall = false;
            }
            if (other.gameObject.tag == "Wall")
            {
                Debug.Log("wall");
            }
            if (other.gameObject.tag == "outofField")
            {
                Debug.Log("outfField");
            }
           
        }
 
    }
   
    // Update is called once per frame
    void Update()
    {
        
        
    }
  
}
