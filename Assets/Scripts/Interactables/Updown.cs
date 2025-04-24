using UnityEngine;

public class Updown : MonoBehaviour, IInteract
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InteractAction()
    {
        this.transform.Translate(0f, 2f, 0f);
    }
}
