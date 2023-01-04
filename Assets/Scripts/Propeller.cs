using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    float z = 0.0f;

    void Update()
    {
        z += 1f * Time.deltaTime;
        transform.Rotate(0, 0, z);        
    }
}
