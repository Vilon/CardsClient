require "Common/define"

Sound = {}
local this = Sound

local soundPath = AppConst.ResDir .. "Sounds/"
local effectPath = soundPath .. "Effect/"

--发送消息
function Sound.PlayEffect(name)
    local fullPath = effectPath .. name .. ".mp3"
    soundMgr:Play(fullPath, Vector3.zero)
end
