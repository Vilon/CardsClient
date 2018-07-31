
local lpeg = require "lpeg"

local json = require "cjson"
local util = require "3rd/cjson/util"

require "Logic/LuaClass"
require "Logic/CtrlManager"
require "Common/functions"

--管理器--
Game = {};
local this = Game;

local game; 
local transform;
local gameObject;
local WWW = UnityEngine.WWW;

function Game.InitViewPanels()
	for i = 1, #PanelNames do
        require ("View/"..tostring(PanelNames[i]))
	end
end

--初始化完成，发送链接服务器信息--
function Game.OnInitOK()
    AppConst.SocketPort = 20000;
    AppConst.SocketAddress = "192.168.0.5";
    networkMgr:SendConnect();
    
    --注册LuaView--
    this.InitViewPanels();
    CtrlManager.Init();
    local ctrl = CtrlManager.GetCtrl(CtrlNames.Prompt);
    if ctrl ~= nil then
        ctrl:Show();
    end
       
    logWarn('LuaFramework InitOK--->>>');
end

--测试协同--
function Game.test_coroutine()    
    logWarn("1111");
    coroutine.wait(0.1);	
    logWarn("2222");
    local www = WWW("http://bbs.ulua.org");
    coroutine.www(www);
    log(www.text);     
end

--测试lpeg--
function Game.test_lpeg_func()
	logWarn("test_lpeg_func-------->>");
	-- matches a word followed by end-of-string
	local p = lpeg.R"az"^1 * -1

	print(p:match("hello"))        --> 6
	print(lpeg.match(p, "hello"))  --> 6
	print(p:match("1 hello"))      --> nil
end

--测试lua类--
function Game.test_class_func()
    LuaClass:New(10, 20):test();
end

--测试cjson--
function Game.test_cjson_func()
    local path = Util.DataPath.."lua/3rd/cjson/example2.json";
    local text = util.file_load(path);
    LuaHelper.OnJsonCallFunc(text, this.OnJsonCall);
end

--cjson callback--
function Game.OnJsonCall(data)
    local obj = json.decode(data);
    print(obj['menu']['id']);
end

--销毁--
function Game.OnDestroy()
	--logWarn('OnDestroy--->>>');
end
