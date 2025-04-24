using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGame : MonoBehaviour
{
   
    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor movement
        Cursor.visible = false; // Hides the cursor from view
      
    }

   
}
