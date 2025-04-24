using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Splines;

public class TrainCart : MonoBehaviour, IInteract
{
    public SplineContainer track;
    public float trainSpeed;
    private float t = 0f;
    public bool clockwise;
    public bool active;
    public float directionOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            float direction = clockwise ? 1 : -1;
            t += trainSpeed * Time.deltaTime * direction;

            if (t > 1f) { t = 0f; }
            if (t < 0f) { t = 1f; }
            
            SplineUtility.Evaluate(track.Spline, t, out var position, out var tangent, out var upVector);

            // Convert to world space
            Vector3 worldPos = track.transform.TransformPoint(position);
            Vector3 worldForward = track.transform.TransformDirection(tangent);
            Vector3 worldUp = track.transform.TransformDirection(upVector);

            if (!clockwise) { worldForward *= -1; }


            transform.position = worldPos;
            Quaternion traindirection = Quaternion.LookRotation(worldForward, worldUp);
            transform.rotation = traindirection * Quaternion.Euler(0,directionOffset,0);


        }
    }

    public void InteractAction()
    {
        active = !active;
    }
}
