using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : Interacted, IInteract
{
    public bool rotating = false;
    public float rotationSpeed = 2.0f;
    public bool counterClockwise = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
        
        
       RotatePlatform();
        
    }


    public void InteractAction()
    {
        if (!oneTime)
        {
            rotating = !rotating;
        }
        if (oneTime) 
        { 
        rotating = true;
        }

    }

    public void RotatePlatform()
    {
        if (rotating && !counterClockwise) 
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
        }

        if (rotating && counterClockwise)
        {
            transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
    
}
