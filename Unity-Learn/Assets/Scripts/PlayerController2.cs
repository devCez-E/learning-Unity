using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public GameObject cookie;

    float horizontalInput;
    float speed = 20.0f;
    float xRange = 20f;

    void Update()
    {
        if (transform.position.x < -xRange) transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        else if(transform.position.x > xRange) transform.position = new Vector3(xRange, transform.position.y, transform.position.z);

        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space)) Instantiate(cookie, transform.position, cookie.transform.rotation);
    }
}

