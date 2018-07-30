local transform;
local gameObject;

PromptPage = {
	btnOpen = nil,
	gridParent = nil,
};
local this = PromptPage;

--初始化面板--
function PromptPage.InitPanel()
	this.btnOpen = transform:Find("Open").gameObject;
	this.gridParent = transform:Find('ScrollView/Grid');
end

--启动事件--
function PromptPage.Awake(obj)
	gameObject = obj;
	transform = obj.transform;

	this.InitPanel();
	logWarn("Awake lua--->>"..gameObject.name);
end

function PromptPage.Start()
	-- body
end

--单击事件--
function PromptPage.OnDestroy()
	logWarn("OnDestroy---->>>");
end