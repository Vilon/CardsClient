using UnityEngine;
using LuaFramework;
using System.Collections.Generic;
using UnityEngine.UI;

public class AppView : View
{
    private ProgressData data;
    private Text text;
    private Slider slider;
    ///<summary>
    /// 监听的消息
    ///</summary>
    List<string> MessageList
    {
        get
        {
            return new List<string>()
            {
                NotiConst.UPDATE_MESSAGE,
                NotiConst.UPDATE_PROGRESS,
            };
        }
    }

    void Awake()
    {
        text = GetComponentInChildren<Text>();
        slider = GetComponentInChildren<Slider>();
        RemoveMessage(this, MessageList);
        RegisterMessage(this, MessageList);
    }

    private void Update()
    {
        if (data != null)
        {
            text.text = data.message;
            slider.value = data.value;
        }
    }

    /// <summary>
    /// 处理View消息
    /// </summary>
    /// <param name="message"></param>
    public override void OnMessage(IMessage message)
    {
        string name = message.Name;
        object body = message.Body;
        switch (name)
        {
            case NotiConst.UPDATE_MESSAGE:      //更新消息
                UpdateMessage(body as ProgressData);
                break;
            case NotiConst.UPDATE_PROGRESS:     //更新下载进度
                UpdateProgress(body.ToString());
                break;
        }
    }

    public void UpdateMessage(ProgressData data)
    {
        this.data = data;

    }

    public void UpdateProgress(string data)
    {

    }
}
