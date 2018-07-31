--Generate by UITools, Do not Edit!

local transfrom

MessagePage = {
    spriteImage,
    labelText,
    buttonImage,
    buttonButton,

}
local this = MessagePage

function this.InitPanel()
    spriteImage = transform:Find("Sprite").gameObject:GetComponent("Image")
    labelText = transform:Find("Label").gameObject:GetComponent("Text")
    buttonImage = transform:Find("Button").gameObject:GetComponent("Image")
    buttonButton = transform:Find("Button").gameObject:GetComponent("Button")

end

function this.Awake(obj)
    transfrom = obj.transfrom
    this.InitPanel()
end
