using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interacted, IInteract
{
    
    private bool opened = false;
    public float rotationTime = 2f;
    private Quaternion targetrotation;
    private Coroutine rotationCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Interact action inherited by the Interface
    public void InteractAction()
    {
        if (oneTime && opened) { return; }

        // Toggle the opened boolean
        opened = !opened;

        // Rotate the door by 90 degrees relative to its local Y-axis
        float rotationAmount = opened ? -90f : 90f;
        targetrotation = transform.localRotation * Quaternion.Euler(0, rotationAmount, 0);

        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }
        rotationCoroutine = StartCoroutine(RotateDoor(targetrotation));
    }


    private IEnumerator RotateDoor(Quaternion targetRot)
    {
        yield return new WaitForSeconds(delay);
        Quaternion startRotation = transform.localRotation;
        float elapsedTime = 0f;

        while (elapsedTime < rotationTime) // Lerp duration of 1 second
        {
            elapsedTime += Time.deltaTime * rotationTime;
            transform.localRotation = Quaternion.Lerp(startRotation, targetRot, elapsedTime);
            yield return null;
        }

        // Ensure it ends exactly at the target rotation
        transform.localRotation = targetRot;
    }


}
