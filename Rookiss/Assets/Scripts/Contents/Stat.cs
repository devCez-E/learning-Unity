using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField] protected int level;
    [SerializeField] protected int hp;
    [SerializeField] protected int maxHp;

    [SerializeField] protected int attack;
    [SerializeField] protected int defense;

    [SerializeField] protected float moveSpeed;

    public int Level { get { return level; } set { level = value; } }
    public int HP { get { return hp; } set { hp = value; } }
    public int MaxHp { get { return maxHp; } set { maxHp = value; } }
    public int Attack { get { return attack; } set { attack = value; } }
    public int Defense { get { return defense; } set { defense = value; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
}
