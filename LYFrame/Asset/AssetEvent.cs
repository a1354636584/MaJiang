using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LYFrame
{
    public enum AssetEvent
    {
        Inital = ManagerID.AssetManager + 1,
        HunkRes,
        ReleaseSingleObj,
        //...
        LoadCube,
        LoadCubeBack,
        LoadSphereBack,
        TestRes,
        MaxValu
    }

    public class HunkAssetRes : MsgBase
    {
        public string sceneName;
        public string bundleName;
        public string resName;
        public ushort backMsgId;
        public bool isSingle;


        public HunkAssetRes(bool tmpSingle, ushort msgId, string tmpSceneName, string tmpBundleName, string tmpResName, ushort tmpBackId)
            : base(msgId)
        {
            this.isSingle = tmpSingle;
            this.sceneName = tmpSceneName;
            this.bundleName = tmpBundleName;
            this.resName = tmpResName;
            this.backMsgId = tmpBackId;
        }
    }


    /// <summary>
    /// 返回给上层的消息
    /// </summary>
    public class HunkAssetResBack : MsgBase
    {
        public Object[] value;

        public HunkAssetResBack(ushort msgId, params Object[] tmpValue)
            : base(msgId)
        {
            this.msgId = msgId;
            this.value = tmpValue;
        }

        public void Changer(ushort msgId, params Object[] tmpValu)
        {
            this.msgId = msgId;
            this.value = tmpValu;
        }

        public void Changer(ushort msgId)
        {
            this.msgId = msgId;
        }


        public void Changer(params Object[] tmpValue)
        {
            this.value = tmpValue;
        }

    }
}