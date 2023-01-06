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

public class PlayerController : MonoBehaviour
{
    float speed = 10.0f;
    float yAngle = 0.0f;

    private void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
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
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.back);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            //transform.Translate(Vector3.back * speed * Time.deltaTime);
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.left);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            //transform.Translate(Vector3.left * speed * Time.deltaTime);
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.right);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            //transform.Translate(Vector3.right * speed * Time.deltaTime);
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        //transform.position += transform.TransformDirection(Vector3.forward * speed * Time.deltaTime);
    }
}
