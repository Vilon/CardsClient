require "Common/define"
personpb = require "Protol.person_pb"
loginpb = require "Protol.login_pb"

PromptCtrl = {}
local this = PromptCtrl

local panel
local prompt
local transform
local gameObject

--构建函数--
function PromptCtrl.New()
    logWarn("PromptCtrl.New--->>")
    return this
end

function PromptCtrl.Show()
    logWarn("PromptCtrl.Show--->>")
    uiMgr:PushPage("Prompt", this.OnCreate)
    this.TestPbc()
end

--启动事件--
function PromptCtrl.OnCreate(obj)
    gameObject = obj.gameObject
    transform = obj.transform
    prompt = obj
    obj:AddClick(PromptPage.openButton, this.OnClick)
    resMgr:LoadPrefab(AppConst.ResDir .. "prefabs/ui/page/promptitem.prefab", {"PromptItem"}, this.Init)
end

--初始化面板--
function PromptCtrl.Init(objs)
    print(objs[0].name)
    local count = 1000
    local parent = PromptPage.gridGridLayoutGroup.transform
    for i = 1, count do
        local go = newObject(objs[0])
        go.name = "Item" .. tostring(i)
        go.transform:SetParent(parent)
        go.transform.localScale = Vector3.one
        go.transform.localPosition = Vector3.zero
        prompt:AddClick(go:GetComponent("Button"), this.OnItemClick)

        local label = go.transform:Find("Text")
        label:GetComponent("Text").text = tostring(i)
    end
end

--滚动项单击--
function PromptCtrl.OnItemClick(go)
    log(go.name)
end

--单击事件--
function PromptCtrl.OnClick(go)
    Sound.PlayEffect("Click")
    this.TestSendPbc()
    logWarn("OnClick---->>>" .. go.name)
end

--测试发送PBC--
function PromptCtrl.TestSendPbc()
    local TosChat = {
        name = "Alice",
        content = "12345"
    }
    local chat = loginpb.TosChat()
    chat.name = "Alice"
    chat.content = "1235342"
    Network.SendMessage(ProtoType.TosChat, chat)
end

function PromptCtrl.TestPbc()
    local person = personpb.Person()
    person.id = 1000
    person.name = "Alice"
    person.home.address = 12
    person.email = "Alice@example.com"
    local home = person.Extensions[personpb.Phone.phones]:add()
    home.num = "2147483647"
    home.type = personpb.Phone.HOME

    local data = person:SerializeToString()

    local msg = personpb.Person()

    msg:ParseFromString(data)
    -- print(msg.id)
    -- print(msg.name)
    -- print(msg.email)
    print(tostring(msg))
    -- for i,v in ipairs(msg.Extensions[person_pb.Phone.phones]) do
    --     print(i,v.num)
    -- end
end
--关闭事件--
function PromptCtrl.Close()
    panelMgr:ClosePanel(CtrlNames.Prompt)
end
