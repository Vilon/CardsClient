require "Common/define"
require "Common.prototype"
require "Common/functions"
Event = require "events"
loginpb = require "Protol.login_pb"

Network = {}
local this = Network

local transform
local gameObject
local islogging = false

function Network.Start()
    logWarn("Network.Start!!")
    Event.AddListener(ProtoType.Connect, this.OnConnect)
    Event.AddListener(ProtoType.TocChat, this.OnMessage)
    Event.AddListener(ProtoType.Exception, this.OnException)
    Event.AddListener(ProtoType.Disconnect, this.OnDisconnect)
end

--发送消息
function Network.SendMessage(type, data)
    local msg = data:SerializeToString()
    local buffer = ByteBuffer.New()
    buffer:WriteShort(type)
    buffer:WriteBuffer(msg)
    networkMgr:SendMessage(buffer)
end

--Socket消息--
function Network.OnSocket(key, data)
    Event.Brocast(tostring(key), data)
end

--当连接建立时--
function Network.OnConnect()
    logWarn("Game Server connected!!")
end

--异常断线--
function Network.OnException()
    islogging = false
    NetManager:SendConnect()
    logError("OnException------->>>>")
end

--连接中断，或者被踢掉--
function Network.OnDisconnect()
    islogging = false
    logError("OnDisconnect------->>>>")
end

--登录返回--
function Network.OnMessage(buffer)
    ----------------------------------------------------
    local ctrl = CtrlManager.GetCtrl(CtrlNames.Message)
    if ctrl ~= nil then
        ctrl:Awake()
    end
    logWarn("OnMessage-------->>>")
    this.TestLoginPbc(buffer)
end

--PBC登录--
function Network.TestLoginPbc(bytes)
    print("TestLoginPbc")
    local TocChat = loginpb.TocChat()
    TocChat:ParseFromString(bytes)
    print(tostring(TocChat))
    print(TocChat.name)
    print(TocChat.content)
end

--卸载网络监听--
function Network.Unload()
    Event.RemoveListener(ProtoType.Connect)
    Event.RemoveListener(ProtoType.TocChat)
    Event.RemoveListener(ProtoType.Exception)
    Event.RemoveListener(ProtoType.Disconnect)
    logWarn("Unload Network...")
end
