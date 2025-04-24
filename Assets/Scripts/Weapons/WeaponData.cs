using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponAmmoData
{
    public string weaponID;
    public int maxAmmo;
    public int currentAmmo;

    public WeaponAmmoData(string id, int ammo, int maxamo)
    {
        weaponID = id;
        maxAmmo = maxamo;
        currentAmmo = ammo;
    }
}
