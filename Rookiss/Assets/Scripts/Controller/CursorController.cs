using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    enum CursorType
    {
        None,
        Hand,
        Attack,
    }
    CursorType type = CursorType.None;

    Texture2D attackCur;
    Texture2D handCur;

    int mask = 1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Monster;

    private void Start()
    {
        attackCur = Managers.Resource.Load<Texture2D>("Textures/Cursor/cur0004");
        handCur = Managers.Resource.Load<Texture2D>("Textures/Cursor/cur0001");
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, mask))
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
                if (type != CursorType.Hand)
                {
                    Cursor.SetCursor(handCur, new Vector2(handCur.width / 3, 0), CursorMode.Auto);
                    type = CursorType.Hand;
                }
            }
        }
    }
}
