using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour, ISpawn
{
    public GameObject[] objectToSpawn;
    public int indexNumber =0;
    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void SpawnObject(int index)
    {
        Instantiate(objectToSpawn[index], transform.position, Quaternion.identity);
    }

}
