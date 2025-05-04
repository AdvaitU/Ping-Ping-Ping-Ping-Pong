using System.Collections.Generic;
using UnityEngine;

// With reference to: https://www.youtube.com/watch?v=eHrbL_oShnw

public class Portal : MonoBehaviour
{

    [SerializeField] private Transform destination;

    private void OnTriggerEnter2D(Collider2D puck)
    {

        if (puck.CompareTag("Puck"))
        {
            puck.transform.position = new Vector3(destination.position.x, puck.transform.position.y, 0);
            //Debug.Log(puck.transform.position.y);
        }
            
    }

    private void OnTriggerExit2D(Collider2D puck)
    {
        //Debug.Log(puck.transform.position.y);
    }
}