using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LYFrame
{
    public class UIBase : MonoBase
    {
        [HideInInspector]
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
            UIManager.Instance.RegistMsg(mono, msgs);
        }
        public void UnRegistSelf(MonoBase mono, params ushort[] msgs)
        {
            UIManager.Instance.UnRegistMsg(mono, msgs);
        }

        //public void RegistPart(string id, BasePart part)
        //{
        //    UIManager.Instance.RegistPart(id, part);
        //}
        //public void UnRegistPart(string id)
        //{
        //    UIManager.Instance.UnRegistPart(id);
        //}
        public void SendMsg(MsgBase msg)
        {
            UIManager.Instance.SendMsg(msg);
        }

        private void OnDestroy()
        {
            if (msgIds != null)
                UnRegistSelf(this, msgIds);
        }
    }

}