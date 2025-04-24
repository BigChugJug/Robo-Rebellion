using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : Spawner, IInteract
{
    public bool randomized = false;
    public bool activated = false;
    
    public override void Start()
    {
        base.Start();
       

    }

    public void InteractAction ()
    {
        if (activated)
        { return; }
        if (randomized)
        {
            indexNumber = Random.Range(0, objectToSpawn.Length - 1);
        }

        SpawnObject(indexNumber);
        activated = true;
    }

}
