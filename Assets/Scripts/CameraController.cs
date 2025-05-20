using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Camera Settings
    [Header("Camera Settings")]
    public float XSensitivity = 400f;
    public float YSensitivity = 400f;
    [Space]
    [Header("Connections")]
    public Transform Player;
    float XRotaion;
    float YRotaion;
    
    //Cursor Management
   private void Start() 
   {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
   }

    //Get Mouse Input
   private void Update() 
   {
        float MouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * XSensitivity;
        float MouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * YSensitivity;

        YRotaion += MouseX;
        XRotaion -= MouseY;

        //Rotation Limit
        XRotaion = Mathf.Clamp(XRotaion, -90f, 90f);

        //Camera Rotation And Orientation
        transform.rotation = Quaternion.Euler(XRotaion, YRotaion, 0);
        // Smoothly rotate the player towards the target rotation
        Player.rotation = Quaternion.Euler(0, YRotaion, 0);
    }
}
