using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSpawn : Spawner
{
    private GameManager manager;
    public CheckpointList list;
    public bool active;
    public GameObject player;
    public int myIndexNo = 10;
    // Start is called before the first frame update

    public void Awake()
    {
       // SpawnObject();
        myIndexNo = System.Array.IndexOf(list.checkPoints, this);
    }
    public override void Start()
    {
        
       
       
    }

    // Update is called once per frame
   public override void Update()
    {
        
    }

    public override void SpawnObject(int index)
    {
        if (active)
        {
            index = 0;
            base.SpawnObject(index);
        }    
        
    }

    public void SetCheckPoint ()
    {
        GameManager.Instance.checkpointNo = myIndexNo;
    }


}
