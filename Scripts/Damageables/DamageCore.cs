using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCore : MonoBehaviour, IDamageable
{
    [SerializeField] int health;
    // Start is called before the first frame update
    void Start()
    {
        Enemy enemyparent = GetComponentInParent<Enemy>();
       
        if (enemyparent != null)
        {
            health = enemyparent.health;
        }
        // Ensure weak point can detect hits separately from the parent
        Collider parentCollider = GetComponentInParent<Collider>();
        Collider myCollider = GetComponent<Collider>();

        if (parentCollider != null && myCollider != null)
        {
            Physics.IgnoreCollision(myCollider, parentCollider);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        
        if (health <= 0)
        {
            Debug.Log(gameObject.name + " has been destroyed");
            //this will destroy the object that this core is attached to, making virtually anything destructible
            KillParent();
            Destroy(gameObject);
        }
    }

    private void KillParent()
    {
        IKillable killableParent = GetComponentInParent<IKillable>(); // Find the first parent with IKillable
        if (killableParent != null)
        {
            killableParent.OnChildKilled(gameObject);
        }
    }

}
