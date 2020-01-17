using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera followCamera;
    public Camera staticCamera;

    private void Start()
    {
        followCamera.enabled = false;
        staticCamera.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            staticCamera.enabled = !staticCamera.enabled;
            followCamera.enabled = !followCamera.enabled;
        }
    }
}
