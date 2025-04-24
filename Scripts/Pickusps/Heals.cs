using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heals : MonoBehaviour
{
    PlayerController playerController;
    public float healAmount;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("healsincoming");
            playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController.health < playerController.maxHealth)
            {
                playerController.health += healAmount;
                playerController.healthbar.fillAmount = playerController.health / playerController.maxHealth;
                playerController.HealthTx.text = playerController.health.ToString();
                if (playerController.health > playerController.maxHealth)
                {
                    playerController.health = playerController.maxHealth;
                }
                Destroy(gameObject);
            }
        }    
    }





}
