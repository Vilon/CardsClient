--Generate by UITools, Do not Edit!

local transform

PromptPage = {
    openImage = nil,
    openButton = nil,
    opennameText = nil,
    scrollviewImage = nil,
    gridImage = nil,
    gridGridLayoutGroup = nil
}
local this = PromptPage

function this.InitPanel()
    this.opennameText = transform:Find("Open/OpenName").gameObject:GetComponent("Text")
    this.openImage = transform:Find("Open").gameObject:GetComponent("Image")
    this.openButton = transform:Find("Open").gameObject:GetComponent("Button")
    this.gridImage = transform:Find("ScrollView/Grid").gameObject:GetComponent("Image")
    this.gridGridLayoutGroup = transform:Find("ScrollView/Grid").gameObject:GetComponent("GridLayoutGroup")
    this.scrollviewImage = transform:Find("ScrollView").gameObject:GetComponent("Image")
end

function this.Awake(obj)
    transform = obj.transform
    this.InitPanel()
end
