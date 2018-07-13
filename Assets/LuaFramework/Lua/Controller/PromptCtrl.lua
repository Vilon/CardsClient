require "Common/define"

PromptCtrl = {};
local this = PromptCtrl;

local panel;
local prompt;
local transform;
local gameObject;

--构建函数--
function PromptCtrl.New()
    logWarn("PromptCtrl.New--->>");
    return this;
end

function PromptCtrl.Awake()
    logWarn("PromptCtrl.Awake--->>");
    panelMgr:CreatePanel('Prompt', this.OnCreate);
end

--启动事件--
function PromptCtrl.OnCreate(obj)
    gameObject = obj;
    transform = obj.transform;

    panel = transform:GetComponent('UIPanel');
    prompt = transform:GetComponent('LuaBehaviour');
    logWarn("Start lua--->>" .. gameObject.name);

    prompt:AddClick(PromptPanel.btnOpen, this.OnClick);
    resMgr:LoadPrefab('prompt', { 'PromptItem' }, this.InitPanel);
end

--初始化面板--
function PromptCtrl.InitPanel(objs)
    local count = 1;
    local parent = PromptPanel.gridParent;
    for i = 1, count do
        local go = newObject(objs[0]);
        go.name = 'Item' .. tostring(i);
        go.transform:SetParent(parent);
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = Vector3.zero;
        prompt:AddClick(go, this.OnItemClick);

        local label = go.transform:Find('Text');
        label:GetComponent('Text').text = tostring(i);
    end
end

--滚动项单击--
function PromptCtrl.OnItemClick(go)
    log(go.name);
end

--单击事件--
function PromptCtrl.OnClick(go)
    this.TestSendPbc();
    logWarn("OnClick---->>>" .. go.name);
end

--测试发送PBC--
function PromptCtrl.TestSendPbc()
    local TosChat = {
        name = "Alice",
        content = "12345",
    }
    Network.SendMessage("login","TosChat",TosChat);
end

--关闭事件--
function PromptCtrl.Close()
    panelMgr:ClosePanel(CtrlNames.Prompt);
end