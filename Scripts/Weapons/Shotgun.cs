using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon, IWeapon
{
    public override void Fire()
    {
        if (!canFire) return;
        
        if (currentAmmo > 0)
        {
            canFire = false;
            StartCoroutine(ShotgunFire());
        }
        base.Fire();
        
    }


    public IEnumerator ShotgunFire()
    {
        int bulletsPerShooter = bulletCount / shooterPoints.Length; // Divide bullets evenly
        int extraBullets = bulletCount % shooterPoints.Length; // Handle remainder bullets

        for (int i = 0; i < shooterPoints.Length; i++)
        {
            int shotsToFire = bulletsPerShooter + (i < extraBullets ? 1 : 0); // Distribute extra bullets evenly

            for (int j = 0; j < shotsToFire; j++)
            {
                // Generate random spread angles
                float angleX = Random.Range(-spreadAngle, spreadAngle);
                float angleY = Random.Range(-spreadAngle, spreadAngle);

                // Apply the spread in local space
                Quaternion spreadRotation = Quaternion.Euler(angleX, angleY, 0);

                // Convert to world space using the shooter's local rotation
                Quaternion finalRotation = shooterPoints[i].rotation * spreadRotation;

                // Instantiate bullet with adjusted rotation
                Instantiate(bullet, shooterPoints[i].position, finalRotation);
            }
        }

        SFXSource.Play();

        yield return new WaitForSeconds(1 / firerate);
        canFire = true;
    }
}