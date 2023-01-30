using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : UIBase
{
    enum GameObjects
    {
        HP_Bar,
    }

    Stat stat;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        stat = transform.parent.GetComponent<Stat>();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Transform parent = gameObject.transform.parent;
        transform.position = parent.position + Vector3.up * 1.2f *(parent.GetComponent<Collider>().bounds.size.y);
        transform.LookAt(Camera.main.transform);
        transform.rotation = Camera.main.transform.rotation;

        float ratio = stat.HP / (float)stat.MaxHp;
        SetHpRadio(ratio);
    }

    public void SetHpRadio(float ratio)
    {
        GetObject((int)GameObjects.HP_Bar).GetComponent<Slider>().value = ratio;
    }
}
