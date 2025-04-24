using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SentryEnemy : WalkingEnemy, IAttack

{
    public Transform shooterpoint;
    public GameObject bullet;
    public float attackingSpeed = 1f;
   //[SerializeField] private bool attaked = false;
    [SerializeField] private Coroutine AttackingRoutine;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void Attack()
    {
        if (AttackingRoutine == null)
        {
            
            AttackingRoutine = StartCoroutine("RangedAttack");
        }
    }

    private IEnumerator RangedAttack()
    {
        yield return new WaitForSeconds(attackingSpeed);
       shooterpoint.transform.LookAt(player);
        if (shooterpoint != null && bullet != null)
        {
            Instantiate(bullet, shooterpoint.position, shooterpoint.rotation);
        }

        AttackingRoutine = null;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                float pushForce = 2f;
                // Calculate push direction: away from the contact point
                Vector3 pushDirection = transform.position - collision.contacts[0].point;
                pushDirection.y = 0; // Optional: Remove Y force to prevent lifting

                // Normalize and apply force
                rb.AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);

                
            }
        }
    }


   

}

