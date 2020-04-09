using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


namespace LYFrame
{
    public class NetMsg : MsgBase
    {

        public NetModel model;

        public NetMsg(NetModel model)
            : base(model.MsgID)
        {
            this.model = model;
        }

        public List<byte[]> GetNetBytes()
        {
            byte[] buffer = ProtoSerialize.Serialize(model);
            return NetEncode.Encode(buffer);
        }
    }

}