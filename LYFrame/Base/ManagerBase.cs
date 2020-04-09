using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LYFrame
{
    public class EventNode
    {
        public MonoBase data;
        public EventNode next;

        public EventNode(MonoBase mono)
        {
            data = mono;
            next = null;
        }
    }



    public class ManagerBase : MonoBase
    {
        //注册的消息
        protected Dictionary<ushort, EventNode> dicEventMsg = new Dictionary<ushort, EventNode>();

        /// <summary>
        /// 给脚本注册多个消息
        /// </summary>
        /// <param name="mono"></param>
        /// <param name="msgs"></param>
        public void RegistMsg(MonoBase mono, params ushort[] msgs)
        {
            for (int i = 0; i < msgs.Length; i++)
            {
                ushort id = msgs[i];
                EventNode node = new EventNode(mono);
                RegistMsg(id, node);
            }
        }

        /// <summary>
        /// 给一个消息添加一个node（mono）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="node"></param>
        public void RegistMsg(ushort id, EventNode node)
        {
            if (dicEventMsg.ContainsKey(id))
            {
                EventNode tmpNode = dicEventMsg[id];

                while (tmpNode.next != null && tmpNode.data != node.data)//找到最后一个node,并且这个node没有注册这个消息
                {
                    tmpNode = tmpNode.next;
                }

                if (tmpNode.data != node.data)
                {
                    tmpNode.next = node;
                }
                else
                {
                    Debug.LogWarning("The msg regist this mono. mono:" + node.data);
                }
            }
            else
            {
                dicEventMsg.Add(id, node);
            }
        }

        /// <summary>
        /// 注销一个脚本的一个消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mono"></param>
        public void UnRegistMsg(ushort id, MonoBase mono)
        {
            if (!dicEventMsg.ContainsKey(id))
            {
                Debug.LogWarning("Not contait id:" + id);
                return;
            }

            EventNode node = dicEventMsg[id];

            if (node.data == mono)
            {
                if (node.next == null)
                {
                    dicEventMsg.Remove(id);
                }
                else
                {
                    node.data = node.next.data;
                    node.next = node.next.next;
                }
            }
            else
            {
                while (node.next != null && node.next.data != mono)
                {
                    node = node.next;
                }

                if (node.next == null)
                {
                    Debug.Log(string.Format("The mono=={0} not regist the msgId=={1},or UnRegiste", mono.name, id));
                }
                else
                {
                    if (node.next.next != null)
                    {
                        node.next = node.next.next;
                    }
                    else
                    {
                        node.next = null;
                    }
                }
            }
        }

        /// <summary>
        /// 注销一个脚本的若干个消息
        /// </summary>
        /// <param name="mono"></param>
        /// <param name="msgs"></param>
        public void UnRegistMsg(MonoBase mono, params ushort[] msgs)
        {
            for (int i = 0; i < msgs.Length; i++)
            {
                UnRegistMsg(msgs[i], mono);
            }
        }

        public override void ProcessEvent(MsgBase msg)
        {
            if (!dicEventMsg.ContainsKey(msg.msgId))
            {
                Debug.LogWarning("Not containt id:" + msg.msgId);
                return;
            }
            else
            {
                EventNode node = dicEventMsg[msg.msgId];

                do
                {
                    node.data.ProcessEvent(msg);
                    node = node.next;
                } while (node != null);
            }
        }
    }

}