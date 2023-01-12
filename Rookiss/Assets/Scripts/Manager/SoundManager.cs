using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource[] audioSource = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if(root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
            for(int i = 0; i <soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                audioSource[i] = go.AddComponent<AudioSource>();
                go.transform.SetParent(root.transform);
            }
        }
    }

    public void Play(string path, Define.Sound type, float pitch = 1.0f)
    {
        if (path.Contains("Sounds/") == false) path = $"Sounds/{path}";

        AudioClip ac = GetOrAddAudioClip(path);

        if (ac == null)
        {
            Debug.LogError($"AudioClip Missing !! {path}");
            return;
        }

        switch (type)
        {
            case Define.Sound.Bgm:
                AudioSource audio1 = audioSource[(int)Define.Sound.Bgm];
                if (audio1.isPlaying) audio1.Stop();

                audio1.pitch = pitch;
                audio1.clip = ac;
                audio1.Play();
                audio1.loop = true;
                break;
            case Define.Sound.Effect:
                AudioSource audio2 = audioSource[(int)Define.Sound.Effect];

                audio2.loop = false;
                audio2.PlayOneShot(ac);
                break;
        }
    }

    AudioClip GetOrAddAudioClip(string path)
    {
        AudioClip ac = null;
        if(audioClips.TryGetValue(path, out ac) == false)
        {
            ac = Managers.Resource.Load<AudioClip>(path);
            audioClips.Add(path, ac);
        }
        return ac;
    }

    public void Clear()
    {
        foreach(AudioSource audio in audioSource)
        {
            audio.clip = null;
            audio.Stop();
        }
        audioClips.Clear();
    }
}
