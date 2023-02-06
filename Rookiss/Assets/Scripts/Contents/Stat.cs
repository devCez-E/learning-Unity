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

    private void Start()
    {
        level = 1;
        hp = 100;
        maxHp = 100;
        attack = 10;
        defense = 5;
        moveSpeed = 5.0f;
    }

    public virtual void OnAttacked(Stat opponent)
    {
        int damage = Mathf.Max(0, opponent.Attack - Defense);
        HP -= damage;

        if (HP < 0)
        {
            HP = 0;
            OnDead(opponent);
        }
    }

    protected virtual void OnDead(Stat opponent)
    {
        PlayerStat playerStat = opponent as PlayerStat;
        if (playerStat != null) playerStat.Exp += 15;
        
        Managers.Game.Despawn(gameObject);
    }
}
