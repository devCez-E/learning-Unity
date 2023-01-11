using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInven : UIScene
{
    enum GameObjects
    {
        ItemGrid,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));


        GameObject itemGrid = GetObject((int)GameObjects.ItemGrid);
        foreach (Transform child in itemGrid.transform) Managers.Resource.Destroy(child.gameObject);

        for(int i = 0; i < 8; i++)
        {
            ItemInven invenItem = Managers.UI.CreateItem<ItemInven>(itemGrid.transform);
            invenItem.SetInfo($"¿Œ∫• {i}");
        }
    }
}
