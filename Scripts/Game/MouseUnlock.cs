using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseUnlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None; // Locks the cursor movement
        Cursor.visible = true; // Hides the cursor from view 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
