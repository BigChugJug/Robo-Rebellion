using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon, IWeapon
{
   
    public override void Fire()
    {
        if (!canFire || !gameObject.activeInHierarchy) return;
        if (currentAmmo >  0)
        {
            canFire = false;
            StartCoroutine(PistolFire());
        }
        base.Fire();
    }


    private IEnumerator PistolFire()
    {
       
        foreach (Transform shooterPoint in shooterPoints)
        {
            Quaternion originalLocalRotation = shooterPoint.localRotation;

            // Apply spread to the shooter's forward direction
            Quaternion spreadRotation = ApplySpread(shooterPoint.rotation, spreadAngle);
            if (flash != null)
            {
                Instantiate(flash, shooterPoint.position, spreadRotation, shooterPoint);
                SFXSource.Play();
            }
            Instantiate(bullet, shooterPoint.position, spreadRotation);
            
        }

        yield return new WaitForSeconds(1/firerate);
        canFire = true;
    }

    


}
