using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DroneEnemy : Enemy, IAttack
{
    public float minimumHeight = 3f;
    private float raycastLength = 10f;
    private Rigidbody droneBody;
    public float liftForce = 10f;
    public float damping = 0.5f;
    public float maxSpeed = 10f;
    private float lastKnownHeight = 0f;
    public float adjustment = 1f;
    public GameObject[] turretHeads;
    public bool sleeping = true;
    public float rotationSpeed = 5f;
    public float moveSpeed = 5f;



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Invoke("Initialize",0.5f);
    }
    public void FixedUpdate()
    {
        if (!sleeping)
        {
            RaycastHit groundHit;
            if (Physics.Raycast(transform.position, -transform.up, out groundHit, raycastLength))
            {

                // Store the last known ground height
                lastKnownHeight = groundHit.distance + adjustment;

                // Calculate the difference in height from the desired height
                float difference = minimumHeight + adjustment - groundHit.distance;

                // Calculate the force needed to reach the desired height (without damping applied yet)
                float force = difference * liftForce;

                if (droneBody !=null)
                {
                    // Use a lerp on the force to slowly change it
                    float dampedForce = Mathf.Lerp(droneBody.linearVelocity.y, force, damping * Time.fixedDeltaTime);

                    //check current velocity
                    Vector3 velocity = droneBody.linearVelocity;
                    velocity.y = Mathf.Clamp(dampedForce, -maxSpeed, maxSpeed);
                    droneBody.linearVelocity = velocity;
                }
              

            }
            else
            {

                // Hover at last known height
                float hoverDifference = lastKnownHeight - transform.position.y;

                // Calculate a hover force that stabilizes the drone near its last position
                float hoverForce = hoverDifference * liftForce;

                // Apply damping to smooth the hovering
                float dampedHoverForce = Mathf.Lerp(droneBody.linearVelocity.y, hoverForce, damping * Time.fixedDeltaTime);

                // Adjust velocity to maintain altitude
                Vector3 velocity = droneBody.linearVelocity;
                velocity.y = Mathf.Clamp(dampedHoverForce, -maxSpeed, maxSpeed);
                droneBody.linearVelocity = velocity;

            
            }

        }
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void Attack()
    {

    }
    private void Initialize()
    {
        lastKnownHeight = transform.position.y + adjustment;
        droneBody = GetComponent<Rigidbody>();
        List<GameObject> turretHeadsList = new List<GameObject>();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("TurretHead"))
            {
                turretHeadsList.Add(child.gameObject);
            }
        }

        turretHeads = turretHeadsList.ToArray();
    }

    protected override void Iddle()
    {
        base.Iddle();
        sleeping = true;
        RaycastHit groundHit;
        SphereCollider dronecollider = GetComponent<SphereCollider>();
        float radius = dronecollider.radius;
        if (Physics.Raycast(transform.position, -transform.up, out groundHit, raycastLength))
        {
            if (groundHit.distance <= radius && droneBody != null)
            {
                droneBody.Sleep();
            }
        }

            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

    }

    protected override void ChasePlayer()
    {
        base.ChasePlayer();
        MoveToTarget(player);
        
    }

    protected override void AttackPlayer()
    {
        base.AttackPlayer();
        if (player != null)
        {
            MoveToTarget(player);
        }
        TurretheadRotate();
        TurretAttack();
    }

    void MoveToTarget(Transform target)
    {
        sleeping = false;
        if (target == null) return;

        // Rotate towards the target (only on the Y-axis)
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // Ignore vertical rotation
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attackRange && droneBody !=null)
        {
            Vector3 moveDirection = (target.position - transform.position).normalized;
            moveDirection.y = 0;
            droneBody.linearVelocity = new Vector3(moveDirection.x * moveSpeed, droneBody.linearVelocity.y, moveDirection.z * moveSpeed);

        }

        else
        {
            droneBody.linearVelocity = new Vector3(0, droneBody.linearVelocity.y, 0);
        }

        
    }
    
     void TurretheadRotate()
    {
        foreach (GameObject turrethead in turretHeads)
        {
            // Get direction to player
            Vector3 direction = player.position - turrethead.transform.position;

            // Lock rotation on the Y-axis (allow only vertical rotation)
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Extract the current Y rotation
            float currentYRotation = turrethead.transform.eulerAngles.y;

            // Apply only the X and Z rotation from LookRotation, keeping Y unchanged
            turrethead.transform.rotation = Quaternion.Euler(
                targetRotation.eulerAngles.x, // Keep the up/down rotation
                currentYRotation,             // Preserve the original Y rotation
                targetRotation.eulerAngles.z  // Keep roll if necessary (usually should be 0)
            );
        }

    }

    void TurretAttack()
    {
        foreach (GameObject turrethead in turretHeads)
        {
            IAttack attackComponent = turrethead.GetComponent<IAttack>();

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

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            
            if (droneBody != null)
            {
                float pushForce = 2f;
                // Calculate push direction: away from the contact point
                Vector3 pushDirection = transform.position - collision.contacts[0].point;
                pushDirection.y = 0; // Optional: Remove Y force to prevent lifting

                // Normalize and apply force
                droneBody.AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);

            }
        }
    }

}
