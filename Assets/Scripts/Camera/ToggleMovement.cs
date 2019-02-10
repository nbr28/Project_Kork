using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMovement : MonoBehaviour {

    public void disableMovement()
    {
        gameObject.GetComponent<CameraController>().MovementLocked = true;
    }

    public void enableMovement()
    {
        gameObject.GetComponent<CameraController>().MovementLocked = false;
    }
}
