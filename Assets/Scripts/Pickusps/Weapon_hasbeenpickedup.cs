using UnityEngine;
using UnityEngine.Events;

public class Weapon_hasbeenpickedup : MonoBehaviour
{
    public UnityEvent uEvent;
    public GameObject TriggerObject;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == TriggerObject)
            uEvent.Invoke();
    }
}

