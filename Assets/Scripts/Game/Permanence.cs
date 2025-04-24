using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Permanence : MonoBehaviour
{
    void Awake()
    {
        GameObject obj = GameObject.Find("GameController");

        if (obj != null && obj != this.gameObject)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
