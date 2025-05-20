using System.Collections;
using UnityEngine;

public class VIEW : MonoBehaviour
{
    public Camera firstPersonCam;    // Reference to the first-person camera
    public Camera thirdPersonCam;    // Reference to the third-person camera
    public Transform thirdPersonCamPos;  // Position of the third-person camera
    
    private bool isFirstPerson = true; // Track current camera mode

    // Start is called before the first frame update
    void Start()
    {
        // Initially enable the first-person camera and disable the third-person camera
        SwitchToFirstPerson();
    }

    // Update is called once per frame
    void Update()
    {
        // Switch between first-person and third-person camera views when the player presses the "C" key
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isFirstPerson)
            {
                SwitchToThirdPerson();
            }
            else
            {
                SwitchToFirstPerson();
            }
        }
    }

    // Method to switch to first-person camera
    void SwitchToFirstPerson()
    {
        firstPersonCam.enabled = true;
        thirdPersonCam.enabled = false;
        isFirstPerson = true;
    }

    // Method to switch to third-person camera
    void SwitchToThirdPerson()
    {
        firstPersonCam.enabled = false;
        thirdPersonCam.enabled = true;
        thirdPersonCam.transform.position = thirdPersonCamPos.position; // Update position to third-person view
        thirdPersonCam.transform.rotation = thirdPersonCamPos.rotation; // Set the rotation as well
        isFirstPerson = false;
    }
}