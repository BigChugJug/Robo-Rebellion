using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public WeaponSlot[] weapons;
    public PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    //    MountWeapons();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MountWeapons()
    {
        weapons = playerController.weapons;
    }


}
