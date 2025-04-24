using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyFingers : MonoBehaviour
{
    public float radiuscheck = 1f;
    public bool isPlatformRound = false;
    private bool isPlayerInside = false;

    private void Update()
    {
        IsPlayerInside();
       
    }
    private void IsPlayerInside()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radiuscheck);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                bool isPlayerGrounded = collider.GetComponent<PlayerController>().isGrounded;
                if (isPlayerGrounded)
                {
                    collider.gameObject.transform.SetParent(transform);
                }
              isPlayerInside = true;
            }
            else
            {
                isPlayerInside = false;
            }

        }
        
    }

    
    //if the player enters, it becomes parented to the platform, sowhen it moves, player moves with it
    private void OnTriggerEnter(Collider collision)
    {
        
        if (collision.transform.tag == "Player")
        {
            collision.gameObject.transform.SetParent(transform);


        }
    }

    //if the player exits the collider, it becomes unparented to the platform, it becomes free.
    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.gameObject.transform.SetParent(null);
        }
    }

    private void OnDrawGizmos()
    {
        if (!isPlatformRound) { return; }
        
        // Set the Gizmo color (use different colors based on isGrounded state for clarity)
        Gizmos.color = isPlayerInside ? Color.green : Color.red;

        Gizmos.DrawWireSphere(transform.position, radiuscheck);
    }

   
}
