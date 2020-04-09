using ProtoBuf;

namespace LYFrame
{
    [ProtoContract]
    public class NetModel
    {
        [ProtoMember(1)]
        public ushort MsgID;
        [ProtoMember(2)]
        public string Message;


    }
}