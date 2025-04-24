using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class WalkingEnemy : Enemy
{
    [Header("Basic Enemy Data")]
    private NavMeshAgent agent;
   // [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private List<Vector3> patrolPointsPositions ;
    [SerializeField] private int patrolPointIndex = 1;
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private bool isPatrolpointSet=false;
    [SerializeField] private bool isPatroller = true;
    [SerializeField] private Vector3 targetPosition;
    public LayerMask whatIsGround;
    public int patrolSpeed = 3;
    public int chaseSpeed = 10;



    // Start is called before the first frame update
    protected override void Start()
    {
        //base.start calls the basic start of the parent object
        base.Start();
        //we initialize the enemy patrol points
        InitializeWalkingEnemy();
        

    }

    // Update is called once per frame
    protected override void Update()
    {
        //we call the update of the regular enemy
        base.Update();
       
       
    }

   
    //iddle state, if the enemy is a patroller, the character will move
    protected override void Iddle()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.Sleep();
        agent.speed = patrolSpeed;
        //if is a patroller and a patrol point is not set, set a new patrol point
        if (!isPatrolpointSet && isPatroller) 
        {
        NextPatrolDestination();
        }
        //if is a patroller and has a point set, go to the point
        if (isPatrolpointSet && isPatroller)
        {
          agent.SetDestination(walkPoint);
        }
        //if is not a patroller and has moved away from its post, return to its post. we use return to break the action.
        if (!isPatroller && transform.position != patrolPointsPositions[0])
        {
            agent.SetDestination(patrolPointsPositions[0]);
            return;
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        // if the 
        if (distanceToWalkPoint.magnitude <= 0.1f) 
        {
           
            patrolPointIndex = (patrolPointIndex + 1) % patrolPointsPositions.Count;

            isPatrolpointSet = false;

        }



    }
    // script to chase the player, we set the position of the player, we make it look towards it and follow at chase speed.
    protected override void ChasePlayer()
    {
        targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPosition);
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }
    //attack distance, we make sure the enemy stops and attacks. This is a very simple behavior, we could create a smarter coroutine but at the moment that is fine.
    protected override void AttackPlayer()
    {
        targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPosition);
        agent.speed = 0;
        //check if the current script has an Iattack interface
        IAttack attackComponent = GetComponent<IAttack>();

        // If it has the IAttack interface, call its Attack method
        if (attackComponent != null)
        {
            attackComponent.Attack(); // Calls Attack() from IAttack interface
        }

        else 
        {
        //do nothing, but since it does not have the interface it can do something else
        }
    }


    //Detect any patrol point and add it to the list, so the character can move from one point to the other
    private void InitializeWalkingEnemy()
    {
        agent = GetComponent<NavMeshAgent>();
        
        patrolPointsPositions.Clear();

        Transform[] alltransforms = GetComponentsInChildren<Transform>();

        foreach (Transform child in alltransforms)
        {

            if (child.CompareTag("PatrolPoint"))
            {

                if (!Physics.Raycast(child.position, -transform.up, out RaycastHit hit, Mathf.Infinity, whatIsGround))
                {
                    Debug.Log(child.gameObject.name + " point is not on ground removing it");
                    Destroy(child.gameObject);

                }
                else {
                    child.position = hit.point;
                    patrolPointsPositions.Add(child.position);
                    Destroy(child.gameObject);
                }
            }


            
        }

        walkPoint = patrolPointsPositions[patrolPointIndex];
    }

    //change patrol walpoint destination to the next one.
    private void NextPatrolDestination()
    {
        walkPoint = patrolPointsPositions[patrolPointIndex];



        isPatrolpointSet = true;

    }

}
