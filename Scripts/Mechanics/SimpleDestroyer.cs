using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDestroyer : MonoBehaviour, IKillable
{
    public float wait = 2;
   public void OnChildKilled(GameObject child)
    {
        Debug.Log("Core has been destroyed, Time to die");
        Destroy(gameObject, wait);
    }
}
