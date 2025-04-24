using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureZone : MonoBehaviour
{
    [SerializeField] private CheckpointSpawn checkpointSpawner;
    public void Start()
    {
        checkpointSpawner = transform.parent.GetComponent<CheckpointSpawn>();
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.GetComponent<PlayerController>())
        {
            checkpointSpawner.SetCheckPoint();
            
        }
    }
}
