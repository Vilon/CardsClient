require "Common/define"

require "3rd/pblua/login_pb"
require "3rd/pbc/protobuf"

local sproto = require "3rd/sproto/sproto"
local core = require "sproto.core"
local print_r = require "3rd/sproto/print_r"

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
    local count = 100;
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
    this.TestSendPblua();
    logWarn("OnClick---->>>" .. go.name);
end

--测试发送SPROTO--
function PromptCtrl.TestSendSproto()
    local sp = sproto.parse [[
    .Person {
        name 0 : string
        id 1 : integer
        email 2 : string

        .PhoneNumber {
            number 0 : string
            type 1 : integer
        }

        phone 3 : *PhoneNumber
    }

    .AddressBook {
        person 0 : *Person(id)
        others 1 : *Person
    }
    ]]

    local ab = {
        person = {
            [10000] = {
                name = "Alice",
                id = 10000,
                phone = {
                    { number = "123456789", type = 1 },
                    { number = "87654321", type = 2 },
                }
            },
            [20000] = {
                name = "Bob",
                id = 20000,
                phone = {
                    { number = "01234567890", type = 3 },
                }
            }
        },
        others = {
            {
                name = "Carol",
                id = 30000,
                phone = {
                    { number = "9876543210" },
                }
            },
        }
    }
    local code = sp:encode("AddressBook", ab)
    ----------------------------------------------------------------
    local buffer = ByteBuffer.New();
    buffer:WriteShort(Protocal.Message);
    buffer:WriteByte(ProtocalType.SPROTO);
    buffer:WriteBuffer(code);
    networkMgr:SendMessage(buffer);
end

--测试发送PBC--
function PromptCtrl.TestSendPbc()
    local path = Util.DataPath .. "lua/3rd/pbc/login.pb";
    local addr = io.open(path, "rb")
    local buffer = addr:read "*a"
    addr:close()
    protobuf.register(buffer)

    local TosChat = {
        name = "Alice",
        content = "12345",
    }
    local code = protobuf.encode("msg.TosChat", TosChat)
    ----------------------------------------------------------------
    local buffer = ByteBuffer.New();
    buffer:WriteShort(Protocal.TosChat);
    buffer:WriteBuffer(code);
    networkMgr:SendMessage(buffer);
end

--测试发送PBLUA--
function PromptCtrl.TestSendPblua()
    local tosChat = login_pb.TosChat();
    tosChat.name = "Alice";
    tosChat.content = "12345";
    local msg = tosChat:SerializeToString();
    ----------------------------------------------------------------
    local buffer = ByteBuffer.New();
    buffer:WriteShort(Protocal.TosChat);
    buffer:WriteBuffer(msg);
    networkMgr:SendMessage(buffer);
end

--测试发送二进制--
function PromptCtrl.TestSendBinary()
    local buffer = ByteBuffer.New();
    buffer:WriteShort(Protocal.Message);
    buffer:WriteByte(ProtocalType.BINARY);
    buffer:WriteString("ffff我的ffffQ靈uuu");
    buffer:WriteInt(200);
    networkMgr:SendMessage(buffer);
end

--关闭事件--
function PromptCtrl.Close()
    panelMgr:ClosePanel(CtrlNames.Prompt);
end