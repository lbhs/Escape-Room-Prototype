﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatScreenCameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    //The transform of the player so the entire player will rotate with the camera
    public Transform playerTransform;

    float xRotation = 0f;

    void Start()
    {
        //Hide/lock mouse in the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        //calculates the angles
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;

        //clamp rotation so it has limits
        xRotation = Mathf.Clamp(xRotation, -90f, 90);

        //apples rotations
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerTransform.Rotate(Vector3.up * mouseX);
    }
}
