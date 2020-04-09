using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace LYFrame
{
    /// <summary>
    /// ProtoBuffer 序列化 反序列化
    /// </summary>
    public class ProtoSerialize
    {
        public static byte[] Serialize(NetModel model)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize<NetModel>(ms, model);
                    byte[] result = new byte[ms.Length];
                    ms.Position = 0;

                    ms.Read(result, 0, result.Length);
                    return result;
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("Serialize Failed: " + e.ToString());
                return null;
            }
        }

        public static NetModel DeSerialize(byte[] msg)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(msg, 0, msg.Length);
                    ms.Position = 0;
                    NetModel model = ProtoBuf.Serializer.Deserialize<NetModel>(ms);
                    return model;
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("DeSerialize Failed: " + e.ToString());
                return null;
            }
        }

    }
}