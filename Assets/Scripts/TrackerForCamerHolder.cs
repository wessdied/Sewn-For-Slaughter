using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerForCamerHolder : MonoBehaviour
{
    //Camera Tracking
    [Header("Camera Position Connection")]
    public Transform CameraPosition;
    

   private void Update()
    {
        transform.position = CameraPosition.position;
    }
}
