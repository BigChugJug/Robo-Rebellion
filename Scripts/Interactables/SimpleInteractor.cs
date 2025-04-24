using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInteractor : Interactor
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // on trigger enter, call the trigger.....
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { Triggered(); }
    }
}
