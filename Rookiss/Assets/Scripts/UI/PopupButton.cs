using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupButton : UIBase
{
    enum Buttons
    {
        PointBtn,
    }

    enum Texts
    {
        PointBtnTxt,
        ScoreTxt
    }

    enum GameObjects
    {
        TestObj,
    }

    enum Images
    {
        Pad,
    }

    int score = 0;

    void Start()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.PointBtn).gameObject.AddUIEvent(OnClickPointButton);

        GameObject go = GetImage((int)Images.Pad).gameObject;

        AddUIEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);
    }

    public void OnClickPointButton(PointerEventData data)
    {
        score++;

        GetText((int)Texts.ScoreTxt).text = $"SCORE : {score}";
    }
}
