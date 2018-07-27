using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[ExecuteInEditMode]
public class UIPageBase : LuaFramework.LuaBehaviour
{
    [HideInInspector]
    public bool UseBoxCollider = true;
    [HideInInspector]
    public bool UseBlackMask = false;
    /* Whether or not to show the scene below this one in the stack */
    [HideInInspector]
    public bool HideOldPages = true;
    [HideInInspector]
    public bool BackPopPrePages = false;

    [HideInInspector]
    public bool IsScreenActivated = false;

    public UtilComponent EnterAnimation;

    protected GameObject BlackMask;
    protected GameObject BoxCollider;
    protected override void Start()
    {
        base.Start();
#if UNITY_EDITOR
        SetParent();
#endif
    }

    void SetParent()
    {
        var root = uiManager.PageRoot;
        if (null != root)
        {
            Transform parent = this.transform.parent;
            if (parent != root.transform)
            {
                this.transform.SetParent(root.transform);
                RectTransform rect = this.GetComponent<RectTransform>();
                rect.offsetMax = Vector2.zero;
                rect.offsetMin = Vector2.zero;
                rect.localScale = Vector3.one;
                rect.localPosition = Vector3.zero;
            }
        }
    }
    /* Initialization function that is called immediately after this scene is created */
    public virtual void InitializePage()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.offsetMax = Vector2.zero;
        rect.offsetMin = Vector2.zero; ;

        if (UseBlackMask)
        {
            BlackMask = uiManager.SetUseBlackMask(gameObject);
        }

        if (UseBoxCollider)
        {
            BoxCollider = uiManager.SetUseBoxCollider(gameObject);
        }

        Button[] allBtns = GetComponentsInChildren<Button>();
        for (int i = 0; i < allBtns.Length; i++)
        {
            Button button = allBtns[i];
            button.transition = Selectable.Transition.None;

            GameObject btn = button.gameObject;
            EventTriggerListener.Get(btn).onDown = (bt) =>
            {
                bt.transform.DOKill();
                bt.transform.DOScale(Vector3.one * 1.1f, 0.3f);
            };

            EventTriggerListener.Get(btn).onUp = (bt) =>
            {
                bt.transform.DOKill();
                bt.transform.DOScale(Vector3.one, 0.3f);
            };
        }
    }

    /* Function called when this scene is to be destroyed (allows for deallocation of memory or whatnot) */
    public virtual void DestroyPage()
    {
    }

    /* Function called every time this scene is opened */
    public virtual void OnPageOpened()
    {
        ShowPage();
    }

    /* Function called every time this scene is closed */
    public virtual void OnPageClosed()
    {
        HidePage();
    }

    /* Function called every time this scene becomes the active scene */
    public virtual void OnPageActivated()
    {
        IsScreenActivated = true;

        ShowPage();

        if (EnterAnimation != null)
        {
            EnterAnimation.Play();
        }
    }

    /* Function called every time this scene becomes deactivated (no longer the active scene) */
    public virtual void OnPageDeactivated(bool hidePage)
    {
        IsScreenActivated = false;

        if (hidePage == true)
        {
            HidePage();
        }
    }

    /* Function that hides this scene */
    public virtual void HidePage()
    {
        gameObject.SetActive(false);
    }

    /* Function that unhides this scene */
    public virtual void ShowPage()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnBackPressed()
    {
        uiManager.PopPage();
    }

    public void Clear(MonoBehaviour mono)
    {
        UIUtils.ClearChild(mono);
    }

    public GameObject AddChild(MonoBehaviour parent, MonoBehaviour prefab)
    {
        return UIUtils.AddChild(parent, prefab);
    }
}
