using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerUIBehavior : MonoBehaviour
{
    public GameObject deathUI;
    public EventSystem eventS;
    public Button deathSelected;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death()
    {
        Cursor.lockState = CursorLockMode.None; // Unlocks cursor movement
        Cursor.visible = true; // Makes the cursor visible

        deathUI.SetActive(true);

        // Clear any previous selection
        eventS.SetSelectedGameObject(null);

        // Set the new selected object
        eventS.SetSelectedGameObject(deathSelected.gameObject);
    }
}
