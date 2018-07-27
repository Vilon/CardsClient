using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[ExecuteInEditMode]
public class UIDialogBase : LuaFramework.LuaBehaviour {

    [HideInInspector]
    public bool UseBoxCollider = true;
    [HideInInspector]
    public bool UseBlackMask = true;
    [HideInInspector]
    public bool UseBackButton = true;

    public bool IsScreenActivated = false;

    protected GameObject BlackMask;
    protected GameObject BoxCollider;
    //private Action<bool> callback;

    public UtilBase EnterAnimatioin;
    public UtilBase ExitAnimatioin;

    protected override void Start()
    {
        base.Start();
#if UNITY_EDITOR
        SetParent();
#endif
    }

    void SetParent()
    {
        var root = uiManager.DialogRoot;
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
    public virtual void InitializeScene()
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
    }

    /* Function called every time this scene becomes the active scene */
    public virtual void OnSceneActivated()
    {
        IsScreenActivated = true;
        UIUtils.SetActive(this.gameObject, true);

        PlayEnterAnimation();
    }

    /* Function called every time this scene becomes deactivated (no longer the active scene) */
    public virtual void OnSceneDeactivated()
    {
        IsScreenActivated = false;
        UIUtils.SetActive(this.gameObject, false);
    }

    public virtual void OnBackPressed(bool isOK)
    {
        PlayExitAnimation(isOK);
    }

    public virtual void OnBackPressed()
    {
        PlayExitAnimation(false);
    }

    public virtual void SetAllMemberValue()
    {

    }

    public virtual void PlayEnterAnimation()
    {
        if (null != EnterAnimatioin)
        {
            EnterAnimatioin.Play();
        }
    }

    public virtual void PlayExitAnimation(bool isOk = false)
    {
        if (null != ExitAnimatioin)
        {
            //ExitAnimatioin.callBack.AddListener(() => {
            //    OnExitFinish(isOk);
            //});
            ExitAnimatioin.Play();
        }
        else
        {
            OnExitFinish(isOk);
        }
    }

    public void OnExitFinish(bool isOk)
    {
        OnSceneDeactivated();
        uiManager.Cache(this, isOk);
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
