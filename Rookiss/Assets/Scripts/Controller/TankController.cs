using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Tank
{
    Player player;
    public float speed = 10.0f;
}

class FastTank : Tank
{

}

class Player
{

}

public class TankController : MonoBehaviour
{
    void Start()
    {
        Tank tank1 = new Tank();
    }
}
