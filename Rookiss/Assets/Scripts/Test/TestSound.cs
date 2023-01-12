using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : MonoBehaviour
{
    public AudioClip ac1;
    public AudioClip ac2;

    bool isSwitch = true;

    private void Start()
    {
        //Managers.Sound.Play("UnityChan/univ1343", Define.Sound.Bgm);
    }

    private void OnTriggerEnter(Collider other)
    {
        /*AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(ac1);
        audio.PlayOneShot(ac2);
        float lifeTime = Mathf.Max(ac1.length, ac2.length);
        GameObject.Destroy(gameObject, lifeTime);*/

        if(isSwitch) Managers.Sound.Play("UnityChan/univ0001", Define.Sound.Effect);
        else Managers.Sound.Play("UnityChan/univ0002", Define.Sound.Effect);

        isSwitch = !isSwitch;
    }
}
