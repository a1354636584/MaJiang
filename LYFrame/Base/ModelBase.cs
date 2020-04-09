using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LYFrame;

public class ModelBase : MonoBase
{

    public ushort[] msgIds;

    /// <summary>
    /// 添加消息
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="msgs"></param>
    protected void AddMsg(MonoBase mono, ushort[] msgs)
    {
        if (msgs == null || msgs.Length <= 0)
            return;
        if (msgIds == null || msgIds.Length <= 0)
        {
            msgIds = msgs;
        }
        else
        {
            ushort[] newMsg = new ushort[msgIds.Length + msgs.Length];
            msgIds.CopyTo(newMsg, 0);
            msgs.CopyTo(newMsg, msgIds.Length);
            msgIds = newMsg;
        }
        RegistSelf(mono, msgIds);
    }

    public override void ProcessEvent(MsgBase msg)
    {
        
    }

    public void RegistSelf(MonoBase mono, params ushort[] msgs)
    {
        ModelManager.Instance.RegistMsg(mono, msgs);
    }
    public void UnRegistSelf(MonoBase mono, params ushort[] msgs)
    {
        ModelManager.Instance.UnRegistMsg(mono, msgs);
    }

    public void SendMsg(MsgBase msg)
    {
        ModelManager.Instance.SendMsg(msg);
    }

    private void OnDestroy()
    {
        if (msgIds != null)
        {
            UnRegistSelf(this, msgIds);
        }
    }
}
