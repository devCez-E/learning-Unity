using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 20.0f;
    private float turnSpeed = 45.0f;
    private float forwardInput;
    private float horizontalInput;

    private void Update()
    {
        // Move the vehicle forward
        /*transform.Translate(0, 0, 1);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);*/

        forwardInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        // Moves the vehicle forward based on vertical input
        transform.Translate(Vector3.forward * forwardInput * speed * Time.deltaTime);

        // Rotates the vehicle based on horizontal input
        transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);
    }
}
