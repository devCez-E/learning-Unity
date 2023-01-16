using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolScene : BaseScene
{

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Login;


        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < 5; i++) list.Add(Managers.Resource.Instantiate("Prefabs/UnityPool"));

        foreach(GameObject obj in list)
        {
            Managers.Resource.Destroy(obj);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Managers.Game.LoadScene(Define.Scene.Game);
        }
    }

    public override void Clear()
    {
        Debug.Log("PoolScene Clear!");
    }
}
