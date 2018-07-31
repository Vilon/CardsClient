using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace LuaFramework
{
    public class LuaBehaviour : View
    {
        string panel;
        string control;
        private Dictionary<Button, LuaFunction> buttons = new Dictionary<Button, LuaFunction>();

        protected virtual void Awake()
        {
            panel = name.Replace("(Clone)", string.Empty);
            if (name.Contains("Page"))
                control = name.Replace("Page(Clone)", "Ctrl");
            if (name.Contains("Dialog"))
                control = name.Replace("Dialog(Clone)", "Ctrl");
            if (name.Contains("Msg"))
                control = name.Replace("Msg(Clone)", "Ctrl");
            Util.CallMethod(panel, "Awake", gameObject);
            Util.CallMethod(control, "Awake", gameObject);
        }

        protected virtual void Start()
        {
            Util.CallMethod(control, "Start");
        }

        protected virtual void OnEnable()
        {
            Util.CallMethod(control, "OnEnable");
        }

        protected virtual void OnDisable()
        {
            Util.CallMethod(control, "OnDisable");
        }

        protected virtual void OnClick()
        {
            Util.CallMethod(control, "OnClick");
        }

        protected virtual void OnClickEvent(GameObject go)
        {
            Util.CallMethod(control, "OnClick", go);
        }

        /// <summary>
        /// 添加单击事件
        /// </summary>
        public void AddClick(Button go, LuaFunction luafunc)
        {
            if (go == null || luafunc == null) return;
            buttons.Add(go, luafunc);
            go.onClick.AddListener(
                delegate ()
                {
                    luafunc.Call(go);
                }
            );
        }

        /// <summary>
        /// 删除单击事件
        /// </summary>
        /// <param name="go"></param>
        public void RemoveClick(Button go)
        {
            if (go == null) return;
            LuaFunction luafunc = null;
            if (buttons.TryGetValue(go, out luafunc))
            {
                luafunc.Dispose();
                luafunc = null;
                buttons.Remove(go);
            }
        }

        /// <summary>
        /// 清除单击事件
        /// </summary>
        public void ClearClick()
        {
            foreach (var de in buttons)
            {
                if (de.Value != null)
                {
                    de.Value.Dispose();
                }
            }
            buttons.Clear();
        }

        //-----------------------------------------------------------------
        protected void OnDestroy()
        {
            ClearClick();
#if ASYNC_MODE
            if (Application.isPlaying)
            {
                string abName = panel.ToLower();
                ResManager.UnloadAssetBundle(abName);
            }
#endif
            Util.ClearMemory();
            Debug.Log("~" + panel + " was destroy!");
        }
    }
}