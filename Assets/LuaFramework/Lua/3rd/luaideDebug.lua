--luaide debug
local breakSocketHandle, debugXpCall = require("LuaDebug")("localhost", 7003)
local timer =
    Timer.New(
    function()
        breakSocketHandle()
    end,
    1,
    -1,
    false
)
timer:Start()
--luaide end
