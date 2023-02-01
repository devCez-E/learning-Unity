using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    Stat stat;

    [SerializeField]
    float scanRange = 5;

    [SerializeField]
    float attackRange = 1.2f;

    bool stopSkill = false;

    public override void Init()
    {
        stat = gameObject.GetOrAddComponent<Stat>();
        objType = Define.GameObj.Monster;
        if (gameObject.GetComponentInChildren<HPBar>() == null) Managers.UI.CreateWorldspaceUI<HPBar>(transform);
    }

    protected override void UpdateIdle()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float distance = (player.transform.position - this.transform.position).magnitude;
        if (distance <= scanRange && player.transform.GetComponent<Stat>().HP > 0)
        {
            lockTarget = player;
            State = Define.State.Move;
            return;
        }
    }

    protected override void UpdateMoving()
    {        
        if (lockTarget != null)
        {
            destination = lockTarget.transform.position;
            float distance = (destination - transform.position).magnitude;
            
            if (distance <= attackRange)
            {
                NavMeshAgent nma = gameObject.GetComponent<NavMeshAgent>();
                nma.SetDestination(transform.position);
                State = Define.State.Skill;
                return;
            }
        }

        Vector3 dir = destination - transform.position;
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetComponent<NavMeshAgent>();
            nma.SetDestination(destination);
            nma.speed = stat.MoveSpeed;            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }

    void OnRunEvent(int num)
    {

    }

    void OnHitEvent()
    {
        if (lockTarget != null)
        {
            Stat targetStat = lockTarget.GetComponent<Stat>();
            Stat myStat = gameObject.GetComponent<Stat>();

            int damage = Mathf.Max(0, myStat.Attack - targetStat.Defense);
            targetStat.HP -= damage;

            if (targetStat.HP <= 0) Managers.Game.Despawn(targetStat.gameObject);

            if(targetStat.HP > 0)
            {
                float distance = (lockTarget.transform.position - this.transform.position).magnitude;
                if (distance <= attackRange) State = Define.State.Skill;
                else State = Define.State.Move;
            }
            else
            {
                State = Define.State.Idle;
                lockTarget = null;
            }
        }
        else
        {
            State = Define.State.Idle;
        }
    }

    protected override void UpdateSkill()
    {
        if (lockTarget != null)
        {
            Vector3 dir = lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    protected override void UpdateDie()
    {
        Debug.Log("Monster UpdateDie");
    }
}
