using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTurretHead : MonoBehaviour, IAttack
{
    public Transform shooterpoint;
    public GameObject bullet;
    public float attackingSpeed = 1f;
    private Coroutine AttackingRoutine;
    public Transform player;

    public void Start()
    {
        //find the player
        player = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
    }

    //call the attacking coroutine
    public void Attack()
    {
        if (AttackingRoutine == null)
        {

            AttackingRoutine = StartCoroutine("TurretAttack");
        }
    }
    //this is the coroutine that will keep attacking with a attacking speed intervals
    private IEnumerator TurretAttack()
    {
        yield return new WaitForSeconds(attackingSpeed);
       // the shooterpoint has to exist, and so the bullet for the turret to attack
        if (shooterpoint != null && bullet != null)
        {
            //create a bullet, each bullet will handle its own movement....
            Instantiate(bullet, shooterpoint.position, shooterpoint.rotation);
        }
        //end the coroutine, so it can be called again without calling multiples
        AttackingRoutine = null;
    }
}
