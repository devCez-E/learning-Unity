using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. Position Vector
// 2. Direction Vector
struct MyVector
{
    public float x;
    public float y;
    public float z;

    public float magnitude{get {return Mathf.Sqrt(x*x + y*y + z*z);}}
    public MyVector normalized { get { return new MyVector(x / magnitude, y / magnitude, z / magnitude); } }

    public MyVector(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }

    public static MyVector operator +(MyVector a, MyVector b)
    {
        return new MyVector(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static MyVector operator -(MyVector a, MyVector b)
    {
        return new MyVector(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static MyVector operator *(MyVector a, float d)
    {
        return new MyVector(a.x * d, a.y * d, a.z * d);
    }
}

/*
MyVector to = new MyVector(10.0f, 0.0f, 0.0f);
MyVector from = new MyVector(5.0f, 0.0f, 0.0f);

// Direction Vector : Distance(Value) & Direction
MyVector dir = to - from; // (5.0f, 0.0f, 0.0f)
dir = dir.normalized; // (1.0f, 0.0f, 0.0f)

MyVector newPos = from + dir * speed;
*/

public class PlayerController : BaseController
{
    PlayerStat stat;

    int mask = 1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Monster;

    float yAngle = 0.0f;
    float wait_run_ratio = 0.0f;

    bool moveToDest;
    bool stopSkill = false;

    public override void Init()
    {
        stat = gameObject.GetOrAddComponent<PlayerStat>();
        objType = Define.GameObj.Player;

        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        if (gameObject.GetComponentsInChildren<HPBar>() == null) Managers.UI.CreateWorldspaceUI<HPBar>(transform);
    }

    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case Define.State.Idle:
            case Define.State.Move:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
                if (evt == Define.MouseEvent.PointerUp) stopSkill = true;
                break;
        }
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        //if (evt != Define.MouseEvent.Click) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, mask);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                if (raycastHit)
                {
                    destination = hit.point;
                    State = Define.State.Move;
                    stopSkill = false;

                    if (hit.collider.gameObject.layer == (int)Define.Layer.Monster) lockTarget = hit.collider.gameObject;
                    else lockTarget = null;
                }
                break;
            case Define.MouseEvent.Press:
                if (lockTarget == null && raycastHit) destination = hit.point;
                break;
            case Define.MouseEvent.PointerUp:
                stopSkill = true;
                break;
        }

        moveToDest = true;
    }

#region STATEMENT FUNCTION
    protected override void UpdateIdle()
    {
        wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10.0f * Time.deltaTime);
        //anim.SetFloat("wait_run_ratio", wait_run_ratio);
        //anim.Play("WAIT_RUN");
    }

    protected override void UpdateMoving()
    {
        if(lockTarget != null)
        {
            float distance = (destination - transform.position).magnitude;
            if (distance <= 2f)
            {
                State = Define.State.Skill;
                return;
            }
        }

        Vector3 dir = destination - transform.position;
        dir.y = 0;

        if (dir.magnitude < 0.1f)
        {
            moveToDest = false;
            State = Define.State.Idle;
        }
        else
        {
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, mask))
            {
                if(Input.GetMouseButton(0) == false) State = Define.State.Idle;
                return;
            }

            float moveDist = Mathf.Clamp(stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10.0f * Time.deltaTime);
        //anim.SetFloat("wait_run_ratio", wait_run_ratio);
        //anim.Play("WAIT_RUN");
    }

    void OnRunEvent(int num)
    {
        Debug.Log($"�ѹ� -- {num}");
    }

    void OnHitEvent()
    {
        if(lockTarget != null)
        {
            Stat targetStat = lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(stat);
        }

        if (stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Move;
        }
    }

    protected override void UpdateSkill()
    {
        if(lockTarget != null)
        {
            Vector3 dir = lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    protected override void UpdateDie()
    {
        Debug.Log(" === DIE ===");
    }
    #endregion

#region UN_USE
    void Raycasting()
    {
        Vector3 look = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        Debug.DrawRay(transform.position + Vector3.up, look, Color.red);

        if (Physics.Raycast(transform.position + Vector3.up, look, out hit, 1))
        {
            Debug.Log($"Raycast {hit.collider.gameObject.name}!");
        }
    }

    void RaycastingForCam()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            //Vector3 dir = mousePos - Camera.main.transform.position;
            //dir = dir.normalized;

            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1);
            //Debug.DrawRay(Camera.main.transform.position, dir * 100f, Color.red, 1);

            LayerMask mask = LayerMask.GetMask("Monster") | LayerMask.GetMask("Wall");
            // int mask = (1 << 8) | (1 << 9);

            RaycastHit hit;
            //if(Physics.Raycast(Camera.main.transform.position, dir, out hit, 100.0f))
            if(Physics.Raycast(ray, out hit, 100.0f, mask))
            {
                Debug.Log($"Raycast {hit.collider.gameObject.name}!");
            }
        }
    }

    void OnKeyboard()
    {
        // Local -> World : TransformDirection
        // World -> Local : InverseTransformDirection

        //yAngle += Time.deltaTime;
        //transform.eulerAngles += new Vector3(0f, yAngle, 0f);
        //transform.Rotate(new Vector3(0f, yAngle, 0f));
        //transform.rotation = Quaternion.Euler(new Vector3(0.0f, yAngle * 20f, 0.0f));

        if (Input.GetKey(KeyCode.W))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            //transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.position += Vector3.forward * stat.MoveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.back);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            //transform.Translate(Vector3.back * speed * Time.deltaTime);
            transform.position += Vector3.back * stat.MoveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.left);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            //transform.Translate(Vector3.left * speed * Time.deltaTime);
            transform.position += Vector3.left * stat.MoveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.right);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            //transform.Translate(Vector3.right * speed * Time.deltaTime);
            transform.position += Vector3.right * stat.MoveSpeed * Time.deltaTime;
        }
        //transform.position += transform.TransformDirection(Vector3.forward * speed * Time.deltaTime);

        moveToDest = false;
    }
#endregion
}
