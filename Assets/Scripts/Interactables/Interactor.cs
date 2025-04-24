using UnityEngine;
using System.Collections;

public abstract class Interactor : MonoBehaviour
{
    public bool oneTimeOnly;
    public GameObject[] items;
    public bool hasRecoverTime;
    public float recoverTime = 2;
    private Coroutine triggering;


    public void Triggered()
    {
        if (triggering != null) { return; } // Prevent multiple calls before recovery time
        triggering = StartCoroutine(Triggering());
    }

    public IEnumerator Triggering()
    {
        foreach (var item in items)
        {
            IInteract interact = item.GetComponent<IInteract>();
            if (interact != null)
            {
                interact.InteractAction();
            }
        }

        if (oneTimeOnly)
        {
            Destroy(gameObject);
            yield break; // Exit the coroutine immediately
        }

        if (hasRecoverTime)
        {
            yield return new WaitForSeconds(recoverTime);
        }

        triggering = null; // Ensure reset after coroutine ends
    }
}

