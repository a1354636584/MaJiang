using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LYFrame
{
    public class CameraBase : MonoBase
    {
        [HideInInspector]
        public ushort[] msgIds;

        public override void ProcessEvent(MsgBase msg)
        {

        }

        public void RegistSelf(MonoBase mono, params ushort[] msgs)
        {
            CameraManager.Instance.RegistMsg(mono, msgs);
        }
        public void UnRegistSelf(MonoBase mono, params ushort[] msgs)
        {
            CameraManager.Instance.UnRegistMsg(mono, msgs);
        }

        public void SendMsg(MsgBase msg)
        {
            CameraManager.Instance.SendMsg(msg);
        }

        private void OnDestroy()
        {
            if (msgIds != null && msgIds.Length != 0)
            {
                UnRegistSelf(this, msgIds);
            }
        }
    }

}