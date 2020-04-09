using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LYFrame
{
    public class MsgBase
    {
        public ushort msgId;

        public ManagerID GetManager()
        {
            int tmpId = msgId / FrameTools.MsgSpan;
            return (ManagerID)(tmpId * FrameTools.MsgSpan);
        }


        public MsgBase(ushort tmpId)
        {
            msgId = tmpId;
        }

        public void ChangeEventId(ushort id)
        {
            msgId = id;
        }
    }

}