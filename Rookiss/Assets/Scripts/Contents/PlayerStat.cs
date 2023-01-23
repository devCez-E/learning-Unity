using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField] protected int exp;
    [SerializeField] protected int gold;

    public int Exp { get { return exp; } set { exp = value; } }
    public int Gold { get { return gold; } set { gold = value; } }

    private void Awake()
    {
        level = 1;
        hp = 100;
        maxHp = 100;
        attack = 10;
        defense = 5;
        moveSpeed = 5.0f;
        exp = 0;
        gold = 0;
    }
}
