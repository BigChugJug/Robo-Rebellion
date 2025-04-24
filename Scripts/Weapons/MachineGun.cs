using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : Weapon, IWeapon
{
    public Coroutine machineGunRoutine;

    public override void Fire()
    {
        if (isFiring) return; // Prevent multiple coroutines
        if (currentAmmo >  0)
        {
            isFiring = true;

            if (machineGunRoutine == null) // Ensure only one instance runs
            {
                machineGunRoutine = StartCoroutine(MachineGunFire());
            }
        }
        
    }

    private IEnumerator MachineGunFire()
    {
        while (isFiring && currentAmmo >0) // Keeps firing while button is held and there is still ammo
        {
            Debug.Log("pewpew"); // Replace with actual shooting logic

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
                base.Fire();
            }



            yield return new WaitForSeconds(1/firerate); // Control fire rate
        }
      
        machineGunRoutine = null; // Reset reference when stopping
    }
}
