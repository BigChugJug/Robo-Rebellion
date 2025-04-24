using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public int damageAmount = 5;
    public float tickTimeOverSeconds = 1;
    private Coroutine damageRoutine;
    private List<IDamageable> damageables = new List<IDamageable>();
    private bool isExiting = false;

    // when a damageable objet enters the collider... add it to the list of colliders
    private void OnTriggerEnter(Collider other)
    {
        //this creates a temporary damageable reference from the collider
        IDamageable newDamageable = other.GetComponent<IDamageable>();

        //if the object has the component and is not on the list, adde it to the list
        if (newDamageable != null && !damageables.Contains(newDamageable))
        {
            damageables.Add(newDamageable);
        }

        // Start the damage coroutine if it's not running
        if (damageRoutine == null)
        {
            damageRoutine = StartCoroutine(DamageOverTime());
        }
    }

    //if an object exits the collider, remove it from the list
    private void OnTriggerExit(Collider other)
    {
        //temporary object to check if the collider has IDamageable
        IDamageable exitingDamageable = other.GetComponent<IDamageable>();

        //if the object exists, meaning it has the Idamageable, remove it from the list
        if (exitingDamageable != null)
        {
            damageables.Remove(exitingDamageable);
        }

        // Stop the coroutine when no more damageables remain
        if (damageables.Count == 0 && damageRoutine != null)
        {
            StopCoroutine(damageRoutine);
            damageRoutine = null;
        }
    }

    //actual coroutine to make damage
    private IEnumerator DamageOverTime()
    {
        //while there is at least one Idamageable in the list, and the bool exist keep damaging
        while (damageables.Count > 0 && !isExiting)
        {
            //for each damageable in the list, i is the count minus one to avoid going out of the list, the i number has to be more or equal to 0 (In lists/arrays the starting object is always 0)
            for (int i = damageables.Count - 1; i >= 0; i--)
            {
                //for each one in the index i
                IDamageable damageable = damageables[i];

                //Check if the object still exists
                if (damageable is UnityEngine.Object obj && obj == null)
                {
                    //if it does not exist remove it from the list
                    damageables.RemoveAt(i); 
                    //after removing go ahead to apply the damage
                    continue;
                }
                //this is the actual call to damage
                damageable.TakeDamage(damageAmount);
            }

            //wait until the next tick
            yield return new WaitForSeconds(tickTimeOverSeconds);
        }
        //when there is no more damageables, end the coroutine
        damageRoutine = null; 
    }
}