using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInven : UIBase
{
    enum GameObjects
    {
        ItemThumbnail,
        ItemName
    }

    string name;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        GetObject((int)GameObjects.ItemThumbnail).BindEvent((PointerEventData) => { Debug.Log($"아이템 클릭! {name}"); });
        GetObject((int)GameObjects.ItemName).GetComponent<Text>().text = name;
    }

    public void SetInfo(string _name) { name = _name; }
}
