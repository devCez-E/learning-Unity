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
    private SoundManager sound = new SoundManager();
    private PoolManager pool = new PoolManager();
    public static Managers Instance { get { Init(); return instance; } }
    public static InputManager Input { get { return Instance.input; } }
    public static ResourceManager Resource { get { return Instance.resource; } }
    public static UIManager UI { get { return Instance.ui; } }
    public static GameManager Game { get { return Instance.game; } }
    public static SoundManager Sound { get { return Instance.sound; } }
    public static PoolManager Pool { get { return Instance.pool; } }

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

        instance.sound.Init();
        instance.pool.Init();
    }

    public static void Clear()
    {
        Sound.Clear();
        Input.Clear();
        Game.ClearScene();
        UI.Clear();
        Pool.Clear();
    }
}
