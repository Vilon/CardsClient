local transform;
local gameObject;

PromptPage = {};
local this = PromptPage;

--启动事件--
function PromptPage.Awake(obj)
	gameObject = obj;
	transform = obj.transform;

	this.InitPanel();
	logWarn("Awake lua--->>"..gameObject.name);
end

--初始化面板--
function PromptPage.InitPanel()
	this.btnOpen = transform:Find("Open").gameObject;
	print(this.btnOpen.name)
	this.gridParent = transform:Find('ScrollView/Grid');
	print(this.gridParent.name)
end

function PromptPage.Start()
	-- body
end

--单击事件--
function PromptPage.OnDestroy()
	logWarn("OnDestroy---->>>");
end