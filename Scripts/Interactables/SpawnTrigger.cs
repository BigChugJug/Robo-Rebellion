using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : Interactor
{
    public List<GameObject> mobSpawners;
    // Start is called before the first frame update
    void Start()
    {
        InitializeSpawners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeSpawners()
    {
        MobSpawner[] spawners = GetComponentsInChildren<MobSpawner>();
        foreach (MobSpawner spawn in spawners)
        {
            mobSpawners.Add(spawn.gameObject);
        }

        items = mobSpawners.ToArray();
        mobSpawners.Clear();

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { Triggered(); }
    }

}
