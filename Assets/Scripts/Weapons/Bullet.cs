using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public GameObject HitFx = null;
    public GameObject HitNumbers = null;
    public int damageAmount = 10;
    public int minimumDamageAmount = 0;
    public bool hasDecayingDamage;
    public LayerMask whotohit;
    
    //handler of the damage event...
    private void OnCollisionEnter(Collision collision)
    {
        // Get IDamageable component from the collided object
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        // Check if the collided object is on the correct layer
        bool isCorrectLayer = ((1 << collision.gameObject.layer) & whotohit) != 0;

        if (damageable != null && isCorrectLayer)
        {
            damageable.TakeDamage(damageAmount);
            Debug.Log(collision.transform.name + " took " + damageAmount + " hitpoints of damage");
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Get IDamageable component from the collided object
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        // Check if the collided object is on the correct layer
        bool isCorrectLayer = ((1 << collision.gameObject.layer) & whotohit) != 0;

        if (damageable != null && isCorrectLayer)
        {
            damageable.TakeDamage(damageAmount);
            Debug.Log(collision.transform.name + " took " + damageAmount + " hitpoints of damage");
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
