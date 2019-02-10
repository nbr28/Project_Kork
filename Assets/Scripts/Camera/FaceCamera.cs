using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    /// <summary>
    /// Update is called once per frame forsing all snippets to face the main camera
    /// </summary>
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
    }
}
