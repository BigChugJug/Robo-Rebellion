using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndStage : MonoBehaviour
{
   public GameManager gameManager;
    public PlayGame PlayGame;

    public void Start()
    {
        Invoke("initialize", 1f); 
    }


    void OnTriggerEnter(Collider Other)
    {
        SceneLoader loader = gameObject.GetComponent<SceneLoader>();
        if (loader != null && Other.transform.tag == "Player")
        {
            gameManager.checkpointNo = 0;
            PlayGame.GameStart();
            loader.NextScene();
            
        }        
        
    }

    public void initialize() 
    {
        gameManager = FindAnyObjectByType<GameManager>();
        PlayGame = FindAnyObjectByType<PlayGame>();
    }
}
