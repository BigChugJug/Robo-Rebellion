using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance declaration, must be exactly as it is now, else it will break
    public PlayGame playGame;
    public GameObject player;
    public CheckpointList checkpointList;
    public int checkpointNo = 0;
    [Header("Persistent Info")]
    public List<WeaponAmmoData> weaponAmmoList = new List<WeaponAmmoData>();

    //same here, this must be exacltly like this, don't know why... syntax is crazy
    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
        // Subscribe to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded (Scene scene, LoadSceneMode mode)
    {
       // startPlaying();
        if (checkpointList != null)
        {
            
            checkpointList.activateCheckpoint(checkpointNo);
        }
       

    }

    public void Restart()
    { 
    checkpointNo = 0;
    }    

    public void startPlaying()
    {
        playGame.GameStart();
    }

    public int? GetAmmo(string weaponID)
    {
        WeaponAmmoData data = weaponAmmoList.Find(w => w.weaponID == weaponID);
        return data != null ? data.currentAmmo : (int?)null;
    }

    public void SetAmmo(string weaponID, int newAmmo, int maxAmmo)
    {
        WeaponAmmoData data = weaponAmmoList.Find(w => w.weaponID == weaponID);

        if (data != null)
        {
            data.currentAmmo = newAmmo;
        }
        else
        {
            weaponAmmoList.Add(new WeaponAmmoData(weaponID, newAmmo, maxAmmo));
        }
    }
}
