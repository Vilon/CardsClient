using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LuaInterface;

namespace LuaFramework
{
    public class UIManager : Manager
    {
        private Transform pageRoot;
        private Transform dialogRoot;
        private Transform msgRoot;

        public Transform PageRoot
        {
            get
            {
                if (pageRoot == null)
                {
                    GameObject go = GameObject.Find("UIPage");
                    if (go != null) pageRoot = go.transform;
                }
                return pageRoot;
            }
        }

        public Transform DialogRoot
        {
            get
            {
                if (dialogRoot == null)
                {
                    GameObject go = GameObject.Find("UIDialog");
                    if (go != null) dialogRoot = go.transform;
                }
                return dialogRoot;
            }
        }

        public Transform MsgRoot
        {
            get
            {
                if (msgRoot == null)
                {
                    GameObject go = GameObject.Find("UIMsg");
                    if (go != null) msgRoot = go.transform;
                }
                return msgRoot;
            }
        }

        public bool IsDialogActive
        {
            get
            {
                return (openedDialogList.Count > 0);
            }
        }

        private List<UIDialogBase> cacheDialogList = new List<UIDialogBase>();
        private List<UIDialogBase> openedDialogList = new List<UIDialogBase>();
        protected List<UIPageBase> OpenPages = new List<UIPageBase>();
        protected List<UIPageBase> LoadedPages = new List<UIPageBase>();
        private Queue<string> dialogQueue = new Queue<string>();
        private List<UIMsgBase> cacheMsgList = new List<UIMsgBase>();

        [HideInInspector]
        public UIPageBase ActivePage = null;

        public GameObject SetUseBoxCollider(GameObject target)
        {
            GameObject go = UIUtils.AddChild(target, "Prefabs/UI/UseBoxCollider");
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = Vector2.zero;
            rect.SetAsFirstSibling();
            return go;
        }

        public GameObject SetUseBlackMask(GameObject target)
        {
            GameObject go = UIUtils.AddChild(target, "Prefabs/UI/UseBlackMask");
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = Vector2.zero;
            rect.SetAsFirstSibling();
            return go;
        }

        public void CacheDialog(UIDialogBase dialog, bool isOk)
        {
            if (null != dialog)
            {
                openedDialogList.Remove(dialog);
                cacheDialogList.Add(dialog);
            }
        }

        public void CacheMsg(UIMsgBase dialog, bool isOk)
        {
            if (null != dialog && !cacheMsgList.Contains(dialog))
            {
                cacheMsgList.Add(dialog);
            }
        }

        public virtual void PopPage()
        {
            if (OpenPages.Count > 0)
            {
                var poppedPage = OpenPages[OpenPages.Count - 1];
                if (poppedPage != null) ClosePage(poppedPage);
            }

            if (OpenPages.Count > 0)
            {
                var nextPage = OpenPages[OpenPages.Count - 1];
                if (nextPage != null)
                {
                    SetActivePage(nextPage);
                }
                else
                {
                    ActivePage = null;
                }
            }
            else
            {
                ActivePage = null;
            }
        }

        protected void SetActivePage(UIPageBase newActivePage)
        {
            if (ActivePage != null)
            {
                ActivePage.OnPageDeactivated(newActivePage.HideOldPages);
            }
            ActivePage = newActivePage;
            RectTransform rect = ActivePage.GetComponent<RectTransform>();
            rect.SetAsLastSibling();
            ActivePage.OnPageActivated();
        }

        public virtual void PushPage(string page, LuaFunction callback = null)
        {
            OpenPage(page, (openedPage) =>
            {
                if (openedPage != null &&
                    (ActivePage == null ||
                    openedPage.name != ActivePage.name))
                {
                    SetActivePage(openedPage);
                }
                if (callback != null) callback.Call(openedPage);
            });
        }

        public void OpenPage(string page, System.Action<UIPageBase> callback)
        {
            for (int idx = 0; idx < OpenPages.Count; idx++)
            {
                if (OpenPages[idx] != null &&
                    OpenPages[idx].name == page)
                {
                    UIPageBase Page = OpenPages[idx];
                    OpenPages.RemoveAt(idx);
                    OpenPages.Add(Page);
                    callback(Page);
                }
            }

            LoadPage(page, (openPage) =>
             {
                 if (openPage != null)
                 {
                     OpenPages.Add(openPage);
                     openPage.OnPageOpened();
                 }
                 callback(openPage);
             });
        }

        public void LoadPage(string page, System.Action<UIPageBase> func, bool hidePage = false)
        {
            for (int idx = 0; idx < LoadedPages.Count; idx++)
            {
                if (LoadedPages[idx] != null &&
                    LoadedPages[idx].name == page)
                {
                    func(LoadedPages[idx]);
                }
            }

            if (!string.IsNullOrEmpty(page))
            {
                string assetName = page + "Page";
                string abName = AppConst.ResDir + "Prefabs/UI/Page/" + assetName + ".prefab";
                ResManager.LoadPrefab(abName, delegate (UnityEngine.Object[] objs)
             {
                 if (objs.Length == 0) return;
                 GameObject newPageGameObject = objs[0] as GameObject;
                 if (newPageGameObject != null)
                 {
                     newPageGameObject = UIUtils.AddChild(PageRoot, newPageGameObject);
                     if (newPageGameObject.activeSelf == true)
                     {
                         Debug.Log("newPageGameObject activity true:" + newPageGameObject.name);
                     }
                     else
                     {
                         Debug.Log("newPageGameObject activity false:" + newPageGameObject.name);
                     }
                     if (newPageGameObject != null)
                     {
                         UIPageBase newPage = newPageGameObject.GetComponent<UIPageBase>();

                         if (newPage != null)
                         {
                             LoadedPages.Add(newPage);
                             // Allow the Page to initialize itself
                             newPage.InitializePage();

                             // Hide the Page if requested
                             if (hidePage == true)
                             {
                                 newPage.HidePage();
                             }

                             func(newPage);
                         }
                         else
                         {
                             Debug.LogWarning("UISystem::LoadPage() Failed to instance new Page with name: " + page);
                         }
                     }
                     else
                     {
                         Debug.LogWarning("UISystem::LoadPage() Failed to add new Page to parent UISystem with name: " + page);
                     }
                 }
                 else
                 {
                     Debug.LogWarning("UISystem::LoadPage() Failed to load new Page with name: " + page);
                 }
             });
            }
            else
            {
                Debug.LogWarning("UISystem::LoadPage() No Page Data for Page with name: " + page);
            }
        }

        public void ClosePage(UIPageBase closePage, bool activateNextPage = true)
        {
            if (OpenPages.Remove(closePage) == true)
            {
                if (ActivePage != null && ActivePage.IsScreenActivated == true)
                {
                    ActivePage.OnPageDeactivated(true);
                }

                closePage.OnPageClosed();
            }
        }

        public void PushDialog(string dialog)
        {
            dialogQueue.Enqueue(dialog);
        }

        public void LoadDialog(string key, System.Action<UIDialogBase> callback)
        {
            UIDialogBase dialog = FindDialogInCache(key);

            if (dialog == null)
            {
                string assetName = key + "Dialog";
                string abName = AppConst.ResDir + "Prefabs/UI/Dialog/" + assetName + ".prefab";
                ResManager.LoadPrefab(abName, delegate (UnityEngine.Object[] objs)
                {
                    if (objs.Length == 0) return;
                    GameObject newSceneGameObject = objs[0] as GameObject;
                    if (newSceneGameObject != null)
                    {
                        newSceneGameObject = UIUtils.AddChild(DialogRoot, newSceneGameObject);
                        if (newSceneGameObject.activeSelf == true)
                        {
                            Debug.Log("newSceneGameObject activity true:" + newSceneGameObject.name);
                        }
                        else
                        {
                            Debug.Log("newSceneGameObject activity false:" + newSceneGameObject.name);
                        }
                        if (newSceneGameObject != null)
                        {
                            dialog = newSceneGameObject.GetComponent<UIDialogBase>();
                            dialog.InitializeScene();
                            callback(dialog);
                        }
                        else
                        {
                            Debug.LogWarning("UISystem::LoadDialog() Failed to add new scene to parent UISystem with name: " + key);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("UISystem::LoadDialog() Failed to load new scene with name: " + key);
                    }
                });
            }
            else
            {
                callback(dialog);
            }
        }

        public void LoadMsg(string key, System.Action<UIMsgBase> callback)
        {
            UIMsgBase dialog = FindMsgInCache(key);

            if (dialog == null)
            {
                string assetName = key + "Msg";
                string abName = AppConst.ResDir + "Prefabs/UI/Msg/" + assetName + ".prefab";
                ResManager.LoadPrefab(abName, delegate (UnityEngine.Object[] objs)
                {
                    if (objs.Length == 0) return;
                    GameObject newSceneGameObject = objs[0] as GameObject;
                    if (newSceneGameObject != null)
                    {
                        newSceneGameObject = UIUtils.AddChild(MsgRoot, newSceneGameObject);
                        if (newSceneGameObject.activeSelf == true)
                        {
                            Debug.Log("newSceneGameObject activity true:" + newSceneGameObject.name);
                        }
                        else
                        {
                            Debug.Log("newSceneGameObject activity false:" + newSceneGameObject.name);
                        }
                        if (newSceneGameObject != null)
                        {
                            dialog = newSceneGameObject.GetComponent<UIMsgBase>();
                            dialog.InitializeScene();
                            callback(dialog);
                        }
                        else
                        {
                            Debug.LogWarning("UISystem::LoadDialog() Failed to add new scene to parent UISystem with name: " + key);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("UISystem::LoadDialog() Failed to load new scene with name: " + key);
                    }
                });
            }
            else
            {
                callback(dialog);
            }
        }

        UIDialogBase FindDialogInCache(string key)
        {
            foreach (UIDialogBase dialog in cacheDialogList)
            {
                if (dialog.name == key)
                {
                    cacheDialogList.Remove(dialog);
                    return dialog;
                }
            }
            return null;
        }

        UIMsgBase FindMsgInCache(string key)
        {
            foreach (UIMsgBase dialog in cacheMsgList)
            {
                if (dialog.name == key)
                {
                    return dialog;
                }
            }
            return null;
        }

        private void Update()
        {
            if (dialogQueue.Count != 0 && !IsDialogActive)
            {
                var dialog = dialogQueue.Dequeue();
                LoadDialog(dialog, (activeDialog) =>
                 {
                     openedDialogList.Add(activeDialog);
                     activeDialog.transform.SetAsLastSibling();
                     activeDialog.OnSceneActivated();
                 });
            }
        }

        public void PushMsg(string msg)
        {
            LoadMsg(msg, (activeDialog) =>
            {
                activeDialog.OnSceneActivated();
            });
        }

        public void PopMsg(string msg)
        {
            UIMsgBase dialog = FindMsgInCache(msg);
            dialog.OnSceneActivated();
        }
    }
}