
require "3rd/pbc/protobuf"

function encode(pb,type,data)
    local path = Util.DataPath.."lua/3rd/pbc/"..pb..".pb"
    protobuf.register_file(path)
    local code = protobuf.encode("msg."..type,data)
    local buffer = ByteBuffer.New();
    buffer:WriteShort(Protocal[type]);
    buffer:WriteBuffer(code);
    return  buffer
end

function decode(pb,type,bytes)
    local path = Util.DataPath.."lua/3rd/pbc/"..pb..".pb"
    protobuf.register_file(path)
    local msg = protobuf.decode("msg."..type,bytes)
    return msg
end