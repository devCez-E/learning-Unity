using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance;
    private InputManager input = new InputManager();
    private ResourceManager resource = new ResourceManager();
    private UIManager ui = new UIManager();
    private GameManager game = new GameManager();

    public static Managers Instance { get { Init(); return instance; } }
    public static InputManager Input { get { return Instance.input; } }
    public static ResourceManager Resource { get { return Instance.resource; } }
    public static UIManager UI { get { return Instance.ui; } }
    public static GameManager Game { get { return Instance.game; } }

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
