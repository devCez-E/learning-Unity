using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] animals;

    private float spawnRangeX = 20;
    private float spawnPosZ = 20;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            int rand = Random.Range(0, animals.Length);
            Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ);

            Instantiate(animals[rand], spawnPos, animals[rand].transform.rotation);
        }
    }
}
