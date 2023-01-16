using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++) Push(Create());
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null) return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.isUsing = false;

            poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if (poolStack.Count > 0) poolable = poolStack.Pop();
            else poolable = Create();

            poolable.gameObject.SetActive(true);

            if (parent == null) poolable.transform.parent = Managers.Game.CurrentScene.transform;
            
            poolable.transform.parent = parent;
            poolable.isUsing = true;

            return poolable;
        }
    }

    Dictionary<string, Pool> dicPool = new Dictionary<string, Pool>();

    Transform root;

    public void Init()
    {
        if (root == null)
        {
            root = new GameObject { name = "@PoolRoot" }.transform;
            Object.DontDestroyOnLoad(root);
        }
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = root.transform;

        dicPool.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;

        if(dicPool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        dicPool[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (dicPool.ContainsKey(original.name) == false) CreatePool(original);

        return dicPool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (dicPool.ContainsKey(name) == false) return null;

        return dicPool[name].Original;
    }

    public void Clear()
    {
        foreach (Transform child in root) GameObject.Destroy(child.gameObject);

        dicPool.Clear();
    }
}
