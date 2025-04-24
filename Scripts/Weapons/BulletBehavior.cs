using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : Bullet
{
    public float totalLifetime; // Total bullet lifetime
    [SerializeField]private float remainingLifetime;
    private int initialDamage;

    // Start is called before the first frame update
    void Start()
    {
        totalLifetime = gameObject.GetComponent<SelfDelete>().Lifetime;
        remainingLifetime = totalLifetime;
        initialDamage = damageAmount; // Store the original damage value
    }

    // Update is called once per frame
    void Update()
    {
        remainingLifetime -= Time.deltaTime;
        if (hasDecayingDamage)
        {
            damageAmount = Mathf.Max(minimumDamageAmount, Mathf.RoundToInt(initialDamage * (remainingLifetime / totalLifetime)));
        }
        
        //move bullet Forward
        transform.position += transform.forward * Time.deltaTime * bulletSpeed;
    }

    //script to know when the bullet hits something
    


}
