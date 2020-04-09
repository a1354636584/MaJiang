using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LYFrame
{
    public class AudioBase : MonoBase
    {

        public ushort[] msgIds;

        public override void ProcessEvent(MsgBase msg)
        {

        }

        public void RegistSelf(MonoBase mono, params ushort[] msgs)
        {
            AudioManager.Instance.RegistMsg(mono, msgs);
        }
        public void UnRegistSelf(MonoBase mono, params ushort[] msgs)
        {
            AudioManager.Instance.UnRegistMsg(mono, msgs);
        }

        public void SendMsg(MsgBase msg)
        {
            AudioManager.Instance.SendMsg(msg);
        }

        private void OnDestroy()
        {
            if (msgIds != null)
            {
                UnRegistSelf(this, msgIds);
            }
        }
    }
}
