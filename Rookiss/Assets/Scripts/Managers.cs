using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers Instance;
    static Managers GetInstance() { Init(); return Instance; }

    private void Start()
    {
        Init();
    }

    static void Init()
    {
        GameObject go = GameObject.Find("@Managers");
        if (Instance == null)
        {
            go = new GameObject { name = "@Managers" };
            go.AddComponent<Managers>();
        }

        DontDestroyOnLoad(go);
        Instance = go.GetComponent<Managers>();
    }
}
