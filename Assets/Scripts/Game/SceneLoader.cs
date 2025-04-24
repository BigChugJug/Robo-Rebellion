using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    // this method is called by any object or button, etc....  
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    // ends the game
    public void ExitGame()
    {
        Application.Quit();
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    { 
        Application.Quit();
    }

}
