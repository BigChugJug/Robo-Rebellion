using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ColapsingPlatform : Interacted, IInteract
{
    private bool dropped = false;
    private Quaternion targetRotation;
    private Coroutine rotationCoroutine;
    private float rotationTime = 1;
    public bool isTimed;
    public float timer = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void InteractAction()
    {
        if (oneTime && dropped)
        {
            return;
        }
        dropped = !dropped;

        float rotationAmount = dropped ? 90f : -90f;
        targetRotation = transform.localRotation * Quaternion.Euler(rotationAmount, 0, 0);
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }
        rotationCoroutine = StartCoroutine(RotateTrap(targetRotation));
    }

    public IEnumerator RotateTrap(Quaternion rotation)
    {
        yield return new WaitForSeconds(delay);
        Quaternion startRotation = transform.localRotation;
        float elapsedTime = 0f;

        // First rotation to target
        while (elapsedTime < rotationTime)
        {
            elapsedTime += Time.deltaTime; // Gradually increase time
            transform.localRotation = Quaternion.Lerp(startRotation, rotation, elapsedTime / rotationTime);
            yield return null;
        }

        transform.localRotation = rotation; // Ensure exact alignment

        if (isTimed)
        {
            yield return new WaitForSeconds(timer);

            elapsedTime = 0f;
            dropped = !dropped;
            // Rotate back to original position
            while (elapsedTime < rotationTime)
            {
                elapsedTime += Time.deltaTime; // Gradually increase time
                transform.localRotation = Quaternion.Lerp(rotation, startRotation, elapsedTime / rotationTime);
                yield return null;
                
            }

            transform.localRotation = startRotation; // Ensure exact reset
        }
    }



}
