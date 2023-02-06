using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
#region ---------- Scene ----------
    public BaseScene CurrentScene 
    {
        get { return GameObject.FindObjectOfType<BaseScene>(); }
    }

    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void ClearScene()
    {
        CurrentScene.Clear();
    }
    #endregion

#region ---------- Contents ----------

    GameObject player;
    HashSet<GameObject> monsters = new HashSet<GameObject>();

    public Action<int> OnSpawnEvent;

    public GameObject GetPlayer() { return player; }

    public GameObject Spawn(Define.GameObj type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch(type){
            case Define.GameObj.Unknown:
                break;
            case Define.GameObj.Player:
                player = go;
                break;
            case Define.GameObj.Monster:
                monsters.Add(go);
                if (OnSpawnEvent != null) OnSpawnEvent.Invoke(1);
                break;
        }

        return go;
    }

    public void Despawn(GameObject go)
    {
        Define.GameObj type = GetObjType(go);

        switch (type)
        {
            case Define.GameObj.Unknown:
                break;
            case Define.GameObj.Player:
                if (player == go) player = null;
                break;
            case Define.GameObj.Monster:
                if (monsters.Contains(go))
                {
                    monsters.Remove(go);
                    if (OnSpawnEvent != null) OnSpawnEvent.Invoke(-1);
                }
                break;
        }

        Managers.Resource.Destroy(go);
    }

    public Define.GameObj GetObjType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null) return Define.GameObj.Unknown;
        
        return bc.objType;
    }

#endregion
}
