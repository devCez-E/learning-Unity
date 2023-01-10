using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScene : UIBase
{
    public virtual void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);
    }
}
