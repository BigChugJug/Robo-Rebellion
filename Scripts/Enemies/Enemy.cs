using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public abstract class Enemy : MonoBehaviour
{
    public int health = 100;
    [SerializeField] protected Transform player;

    //State Checkers
    public float sightRange; //check if player is within range of sight
    public float attackRange; //check if player is within attack range
    public bool playerInSightRange; //boolean to do the check of sight
    public bool playerInAttackRange; //boolean to do the check of Attack
    public LayerMask whatIsPlayer;

    protected virtual void Start()
    {
        Invoke("FindPlayer",0f);
    }

    protected virtual void Update()
    {
        AlertStatus();
    }

    private void RangeCheck()
    {
        //Check for Sight and Attack Range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
    }


    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
    }

    //Draw Gizmos to easily visualize ranges
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    //all enemies have alert status depending the distance.... 
    private void AlertStatus()
    {
        //Check for Sight and Attack Range
        RangeCheck();

        if (!playerInSightRange && !playerInAttackRange)
        {
            
            Iddle();
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            
            ChasePlayer();
        }
        if (playerInAttackRange && playerInSightRange)
        {
            
            AttackPlayer();
        }
    }
    // these virtual statuses will be overriden by its child classes
    protected virtual void Iddle()
    { 
      
    }
    protected virtual void ChasePlayer()
    { 
    
    }

    protected virtual void AttackPlayer()
    { }

    

}

