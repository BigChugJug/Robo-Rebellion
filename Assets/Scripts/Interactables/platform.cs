using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class platform : Interacted, IInteract

{
    public List<Vector3> PlatformPointsPositions;
    private int platformPointIndex = 1;
    private Vector3 platformPoint;
    public bool looping = true;
    public float moveSpeed = 10f;
    private Coroutine movementRoutine;
    private Coroutine delayRoutine;
    public bool moving=false;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        InitializePlatform();
       
    }

    // fixed Update supercedes update this is vital for the platform to work and the player not to fall
    void FixedUpdate()
    {
        if (moving)
        {
            MovePlatform();
        }
    }
    //Interface action
    public void InteractAction()
    {
        if (oneTime)
        {
            if (!moving) // Only allow retriggering if it's not already moving
            {
                platformPointIndex = 0; // Reset to start position
                platformPoint = PlatformPointsPositions[platformPointIndex];
                moving = true;
            }
            return;
        }

        if (delayRoutine != null && !oneTime)
        {
            StopCoroutine(delayRoutine);
        }

        delayRoutine = StartCoroutine(ToggleDelay(delay));
    }

    private void InitializePlatform()
    {
        
        //clear the list of platform positions
        PlatformPointsPositions.Clear();
        //look for all children in the platfomr object
        Transform[] alltransforms = GetComponentsInChildren<Transform>();

        //evaluate all their childs
        foreach (Transform child in alltransforms)
        {
            //if their children are of the tag platform point
            if (child.CompareTag("PlatformPoint"))
            {
                //add their transform
                PlatformPointsPositions.Add(child.position);
                Destroy(child.gameObject);
               
            }

        }

       platformPoint = PlatformPointsPositions[platformPointIndex];
    }

    private void MovePlatform()
    {
        // Move the platform if it's not yet at the target point
        if (transform.position != platformPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, platformPoint, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Move to the next point in the list
            platformPointIndex++;

            if (!looping && platformPointIndex >= PlatformPointsPositions.Count)
            {
                platformPointIndex = 0; // Reset index
                moving = false; // Stop moving
                return;
            }

            if (platformPointIndex >= PlatformPointsPositions.Count)
            {
                platformPointIndex = 0; // Loop back
            }

            platformPoint = PlatformPointsPositions[platformPointIndex];
        }
    }

    private IEnumerator ToggleDelay(float delay)
    {
        yield return new WaitForSeconds (delay);
        moving = !moving;
    }

}
