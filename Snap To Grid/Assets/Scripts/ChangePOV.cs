using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePOV : MonoBehaviour
{

    public Camera FirstPersonCam, ThirdPersonCam;
    public KeyCode TKey;
    public GameObject FirstpersonControls;
    public bool camSwitch = false;

    void Update()
    {
        if (Input.GetKeyDown(TKey))
        {
            camSwitch = !camSwitch;
            FirstPersonCam.gameObject.SetActive(camSwitch);
            ThirdPersonCam.gameObject.SetActive(!camSwitch);
            FirstpersonControls.SetActive(camSwitch);
            if (!camSwitch)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else if(camSwitch)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}