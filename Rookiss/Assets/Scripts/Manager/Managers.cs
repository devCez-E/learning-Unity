using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance;
    public static Managers Instance { get { Init(); return instance; } }


    private InputManager input = new InputManager();
    public static InputManager Input { get { return Instance.input; } }


    private ResourceManager resource = new ResourceManager();
    public static ResourceManager Resource { get { return Instance.resource; } }

    private UIManager ui = new UIManager();
    public static UIManager UI { get { return Instance.ui; } }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        input.OnUpdate();
    }

    static void Init()
    {
        GameObject go = GameObject.Find("@Managers");
        if (instance == null)
        {
            go = new GameObject { name = "@Managers" };
            go.AddComponent<Managers>();
        }

        DontDestroyOnLoad(go);
        instance = go.GetComponent<Managers>();
    }
}
