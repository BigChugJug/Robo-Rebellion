using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurretEnemy : Enemy
{
    public GameObject turretHead;
    // Start is called before the first frame update
   protected override void Start()
    {
        base.Start();
        Initialize();
    }
   
    // Update is called once per frame
    protected override void Update()
    {
        base .Update();
    }

   private void Initialize()
    {

        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("TurretHead"))
            {
                turretHead = child.gameObject;
                break; // Stop after finding the first match
            }
        }

        
    }

    protected override void Iddle()
    {
        turretHead.transform.rotation = Quaternion.Euler(90,0,0);
        
    }

    protected override void ChasePlayer()
    {
        
        turretHead.transform.LookAt(player);
    }

    protected override void AttackPlayer()
    {
        //this makes the turret face the player at all times
        turretHead.transform.LookAt(player);
        IAttack attackComponent = turretHead.GetComponent<IAttack>();

        // If it has the IAttack interface, call its Attack method
        if (attackComponent != null)
        {
            attackComponent.Attack(); // Calls Attack() from IAttack interface
        }

        else
        {
            //do nothing, but if it does not have the interface it can do something else
        }
    }
}


