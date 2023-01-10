using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int order = 0;

    Stack<UIPopup> popupStack = new Stack<UIPopup>();
    UIScene sceneStack = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UIRoot");
            if (root == null) root = new GameObject { name = "@UIRoot" };
            return root;
        }
    }

    public T ShowPopup<T>(string name = null) where T : UIPopup
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;
    }

    public T ShowScene<T>(string name = null) where T : UIScene
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T scene = Util.GetOrAddComponent<T>(go);
        sceneStack = scene;

        go.transform.SetParent(Root.transform);

        return scene;
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        if (sort)
        {
            canvas.sortingOrder = order;
            order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public void ClosePopup()
    {
        if (popupStack.Count == 0) return;

        UIPopup popup = popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);

        popup = null;

        order--;
    }

    public void ClosePopup(UIPopup popup)
    {
        if (popupStack.Count == 0) return;
        if (popupStack.Peek() != popup) return;

        ClosePopup();
    }

    public void CloseAllPopup()
    {
        while(popupStack.Count > 0)
        {
            ClosePopup();
        }
    }

    public void CloseSceneUI()
    {
        if (sceneStack == null) return;

        UIScene scene = sceneStack;
        Managers.Resource.Destroy(scene.gameObject);

        scene = null;

        order--;
    }

    public void CloseSceneUI(UIScene scene)
    {
        if (sceneStack == null) return;
        if (sceneStack != scene) return;

        CloseSceneUI();
    }
}
