using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointList : MonoBehaviour
{
    public CheckpointSpawn[] checkPoints;
    public void Awake()
    {
        GameManager.Instance.checkpointList = this;
    }

    public void activateCheckpoint(int index)
    {
        foreach (var indexno in checkPoints)
        {
            indexno.active = false;
        }
        checkPoints[index].active = true;
        checkPoints[index].SpawnObject(0);
    }    

}
