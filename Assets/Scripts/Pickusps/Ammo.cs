using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    
    GameManager gameManager;
    public int percentage = 100;
    public void Start()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            
            gameManager = FindFirstObjectByType<GameManager>();
            foreach (WeaponAmmoData data in gameManager.weaponAmmoList)
            {
                int amountReloaded = (data.maxAmmo * percentage) / 100;
                Debug.Log(amountReloaded);
                data.currentAmmo += amountReloaded;
                
                if (data.maxAmmo < data.currentAmmo)
                {
                    data.currentAmmo = data.maxAmmo;
                }

                GameManager.Instance.SetAmmo(data.weaponID, data.currentAmmo, data.maxAmmo);
               
            }
            Weapon currentWeapon = FindFirstObjectByType<Weapon>();
            if (currentWeapon != null)
            {
                WeaponAmmoData updatedWD = gameManager.weaponAmmoList.Find(w => w.weaponID == currentWeapon.weaponID);
                if (updatedWD != null)
                {
                    currentWeapon.currentAmmo = updatedWD.currentAmmo;
                    currentWeapon.UpdateGUIAmmo();
                }
            }
            Destroy(gameObject);
            
        }
    }

   
}
