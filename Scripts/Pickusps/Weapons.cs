using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    // Reference to the player's controller script
    PlayerController playerController;

    // The weapon ID this pickup is supposed to unlock
    public string weaponID;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that triggered the pickup is on the "Player" layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("healsincoming"); // Debug message when player enters the trigger

            // Get the PlayerController component from the player
            playerController = other.gameObject.GetComponent<PlayerController>();

            // Make sure the player's weapon array exists
            if (playerController.weapons != null && playerController.weapons != null) // <-- redundant check, but harmless
            {
                // Loop through all the player's weapon slots
                // i is a variable, starts with 0, the maximum is the array's length and the i++ is an incremental of one each time
                for (int i = 0; i < playerController.weapons.Length; i++)
                {
                    //this is the slot, or cell in the array
                    WeaponSlot slot = playerController.weapons[i];

                    // Get the Weapon component from the prefab in the current slot
                    Weapon weapontoUnlock = slot.weaponPrefab.GetComponent<Weapon>();

                    // Check if the weapon exists and its ID matches the one we're trying to unlock
                    if (weapontoUnlock != null && weapontoUnlock.weaponID == weaponID)
                    {
                        // Unlock this weapon slot so it can be selected by scrolling
                        slot.isUnlocked = true;

                        // Auto-equip the newly unlocked weapon
                        playerController.weaponIndex = i;
                        playerController.ReplaceWeapon(i);

                        break; // Exit the loop since we've found and unlocked the weapon
                    }
                    else
                    {
                        // Fallback log if something went wrong (e.g., weaponID didn't match)
                        Debug.Log("We have a problem boss");
                    }
                }

                // Destroy the pickup object so it can’t be used again
                Destroy(gameObject);
            }
        }
    }




}
