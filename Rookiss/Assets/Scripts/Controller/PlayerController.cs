using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

public enum PlayerState
{
    Die,
    Move,
    Idle,
    Skill,
    Channeling,
    Jump,
    Fall,
}

public class PlayerController : MonoBehaviour
{
    enum CursorType
    {
        None,
        Hand,
        Attack,
    }

    PlayerStat stat;

    GameObject lockTarget;

    Texture2D attackCur;
    Texture2D handCur;

    int mask = 1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Monster;

    float yAngle = 0.0f;
    float wait_run_ratio = 0.0f;

    bool moveToDest;
    Vector3 destination;

    PlayerState state = PlayerState.Idle;
    CursorType type = CursorType.None;

    private void Start()
    {
        stat = gameObject.GetOrAddComponent<PlayerStat>();
        attackCur = Managers.Resource.Load<Texture2D>("Textures/Cursor/cur0004");
        handCur = Managers.Resource.Load<Texture2D>("Textures/Cursor/cur0001");

        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
    }

    private void Update()
    {
        UpdateMouseCursor();

        switch (state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Move:
                UpdateMoving();
                break;
            case PlayerState.Die:
                UpdateDie();
                break;
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

    void UpdateMouseCursor()
    {
        if (Input.GetMouseButton(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100.0f, mask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                if (type != CursorType.Attack)
                {
                    Cursor.SetCursor(attackCur, new Vector2(attackCur.width / 5, 0), CursorMode.Auto);
                    type = CursorType.Attack;
                }
            }
            else
            {
                if(type != CursorType.Hand)
                {
                    Cursor.SetCursor(handCur, new Vector2(handCur.width / 3, 0), CursorMode.Auto);
                    type = CursorType.Hand;
                }
            }
        }
    }

    void OnMouseEvent(Define.MouseEvent evt)
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
                    state = PlayerState.Move;

                    if (hit.collider.gameObject.layer == (int)Define.Layer.Monster) lockTarget = hit.collider.gameObject;
                    else lockTarget = null;
                }
                break;
            case Define.MouseEvent.PointerUp:
                lockTarget = null;
                break;
            case Define.MouseEvent.Press:
                if (lockTarget != null) destination = lockTarget.transform.position;
                else if (raycastHit) destination = hit.point;
                break;
        }

        moveToDest = true;
    }

#region STATEMENT FUNCTION
    void UpdateIdle()
    {
        Animator anim = GetComponent<Animator>();
        wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10.0f * Time.deltaTime);
        anim.SetFloat("speed", 0f);
        //anim.SetFloat("wait_run_ratio", wait_run_ratio);
        //anim.Play("WAIT_RUN");
    }

    void UpdateMoving()
    {
        Vector3 dir = destination - transform.position;
        if (dir.magnitude < 0.1f)
        {
            moveToDest = false;
            state = PlayerState.Idle;
        }
        else
        {
            float moveDist = Mathf.Clamp(stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);

            NavMeshAgent nma = gameObject.GetComponent<NavMeshAgent>();
            nma.Move(dir.normalized * moveDist);
            //transform.position += dir.normalized * moveDist;            

            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, mask))
            {
                if(Input.GetMouseButton(0) == false) state = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        Animator anim = GetComponent<Animator>();
        wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10.0f * Time.deltaTime);
        anim.SetFloat("speed", 1.0f);
        //anim.SetFloat("wait_run_ratio", wait_run_ratio);
        //anim.Play("WAIT_RUN");
    }

    void OnRunEvent(int num)
    {
        Debug.Log($"¶Ñ¹÷ -- {num}");
    }

    void UpdateDie()
    {
        Debug.Log(" === DIE ===");
    }
#endregion

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
}
