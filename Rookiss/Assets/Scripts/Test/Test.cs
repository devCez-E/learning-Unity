using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    GameObject tank;

    void Start()
    {
        tank = Managers.Resource.Instantiate("Prefabs/Tank");

        Destroy(tank, 3.0f);
    }

}
