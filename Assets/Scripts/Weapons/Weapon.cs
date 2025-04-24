using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{

    [Header("Main Weapon Info")]
    public string weaponID;
    public int maxAmmo = 20;
    public int currentAmmo;

    [Header("Main Variables")]
    public GameObject flash;
    public GameObject bullet; // Bullet prefab
    //public AudioClip SFX; //Fire SFX
    public GameManager gameManager;
    public AudioSource SFXSource; //Fire SFX Audio Source
    public Transform[] shooterPoints; // Muzzle positions
    public float firerate = 2f; // Shots per second
    public int bulletCount = 1; // Number of bullets
    public float spreadAngle = 5f; // Spread in degrees
    public bool canFire = true;
    public bool isFiring = false;

    [Header ("UIVariables")]
    public TextMeshProUGUI uiAmmo;
     

    public virtual void Start()
    {
       currentAmmo = maxAmmo;
        uiAmmo = GameObject.Find("RemainingAmmo").GetComponent<TextMeshProUGUI>();
        
        int? tempAmmo = GameManager.Instance.GetAmmo(weaponID);
        if (tempAmmo == null )
        {
            uiAmmo.text = currentAmmo.ToString();
        }
        
        if (tempAmmo.HasValue && tempAmmo.Value < currentAmmo )
        {
            currentAmmo = tempAmmo.Value;
           
        }

        if (tempAmmo.HasValue)
        {
            uiAmmo.text = currentAmmo.ToString();
        }
        
    }

    public virtual void Fire()
    {
       ConsumeAmmo();
    }

    public Quaternion ApplySpread(Quaternion originalRotation, float maxSpread)
    {
        
        // Create small random rotation offsets
        float angleX = Random.Range(-maxSpread, maxSpread); // Vertical spread
        float angleY = Random.Range(-maxSpread, maxSpread); // Horizontal spread

        // Generate a spread rotation relative to the shooter's local axes
        Quaternion spreadRotation = Quaternion.Euler(angleX, angleY, 0);

        // Apply spread correctly relative to the original direction
        return originalRotation * spreadRotation;
    }

    public virtual void ConsumeAmmo()
    {
        foreach (Transform shooterPoint in shooterPoints)
        {
            currentAmmo -= 1;
        }

        if (currentAmmo < 0)
        {
            currentAmmo = 0;
        }
        GameManager.Instance.SetAmmo(weaponID, currentAmmo, maxAmmo);
        uiAmmo.text = currentAmmo.ToString();


    }

    public virtual void UpdateGUIAmmo()
    {
        uiAmmo.text = currentAmmo.ToString();
    }
  
}
