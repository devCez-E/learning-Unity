using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        Managers.UI.ShowScene<UIInven>();

        //for (int i = 0; i < 5; i++) Managers.Resource.Instantiate("Prefabs/UnityPool");
    }

    public override void Clear()
    {

    }
}
