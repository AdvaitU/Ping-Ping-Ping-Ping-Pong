using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationZone : MonoBehaviour
{
    public GameObject puck;

    public float accelerationMultiplier = 2f; // Multiplier for acceleration

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Puck"))
        {
            Debug.Log("Puck entered acceleration zone");

            Rigidbody2D puckRb = other.GetComponent<Rigidbody2D>();
            Debug.Log(puckRb);
            if (puckRb != null)
            {
                // Apply acceleration to the puck's velocity
                puckRb.AddForce(new Vector2(puckRb.velocity.x, puckRb.velocity.y * accelerationMultiplier), ForceMode2D.Impulse);
                //puckRb.velocity = new Vector2(puckRb.velocity.x, puckRb.velocity.y * accelerationMultiplier);
                Debug.Log(puckRb.velocity);
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
