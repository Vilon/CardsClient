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

        private Dictionary<string, LuaFunction> buttons = new Dictionary<string, LuaFunction>();

        protected virtual void Awake()
        {
            name = name.Replace("(Clone)", string.Empty);
            Util.CallMethod(name, "Awake", gameObject);
        }

        protected virtual void Start()
        {
            Util.CallMethod(name, "Start");
        }

        protected virtual void OnEnable()
        {
            Util.CallMethod(name, "OnEnable");
        }

        protected virtual void OnDisable()
        {
            Util.CallMethod(name, "OnDisable");
        }

        protected virtual void OnClick()
        {
            Util.CallMethod(name, "OnClick");
        }

        protected virtual void OnClickEvent(GameObject go)
        {
            Util.CallMethod(name, "OnClick", go);
        }

        /// <summary>
        /// 添加单击事件
        /// </summary>
        public void AddClick(GameObject go, LuaFunction luafunc)
        {
            if (go == null || luafunc == null) return;
            buttons.Add(go.name, luafunc);
            go.GetComponent<Button>().onClick.AddListener(
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
        public void RemoveClick(GameObject go)
        {
            if (go == null) return;
            LuaFunction luafunc = null;
            if (buttons.TryGetValue(go.name, out luafunc))
            {
                luafunc.Dispose();
                luafunc = null;
                buttons.Remove(go.name);
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
            string abName = name.ToLower();
            ResManager.UnloadAssetBundle(abName);
#endif
            Util.ClearMemory();
            Debug.Log("~" + name + " was destroy!");
        }
    }
}