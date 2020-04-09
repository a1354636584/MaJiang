using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LYFrame;

public class NetBase : MonoBase {

	    public ushort[] msgIds;

        public override void ProcessEvent(MsgBase msg)
        {

        }

        public void RegistSelf(MonoBase mono, params ushort[] msgs)
        {
            NetManager.Instance.RegistMsg(mono, msgs);
        }
        public void UnRegistSelf(MonoBase mono, params ushort[] msgs)
        {
            NetManager.Instance.UnRegistMsg(mono, msgs);
        }

        public void SendMsg(MsgBase msg)
        {
            NetManager.Instance.SendMsg(msg);
        }

        private void OnDestroy()
        {
            if (msgIds != null)
            {
                UnRegistSelf(this, msgIds);
            }
        }
    }

