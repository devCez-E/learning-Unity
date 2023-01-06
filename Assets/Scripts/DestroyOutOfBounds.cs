using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private float topBound = 30f;
    private float lowBound = -10f;

    private void Update()
    {
        if (transform.position.z > topBound) Destroy(this.gameObject);
        else if (transform.position.z < lowBound)
        {
            Debug.Log("###### GAME OVER! ######");
            Destroy(this.gameObject);
        }
    }
}
