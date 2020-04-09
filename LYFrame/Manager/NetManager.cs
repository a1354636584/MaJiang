using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LYFrame
{
    public class NetManager : ManagerBase
    {

        public static NetManager Instance = null;

        private Dictionary<string, GameObject> sonMembers = new Dictionary<string, GameObject>();

        //public string ip;
        //public ushort port;

        //private NetWorkToServer server;

        private void Awake()
        {
            Instance = this;

        }

        void Start() {
            //server = new NetWorkToServer(ip, port);
        }

        public void PutSendMsgToPool(NetMsg msg)
        {
           // server.PutSendMsgToPool(msg);
        }
        void Update()
        {
           // server.Update();
        }

        public void SendMsg(MsgBase msg)
        {
            if (msg.GetManager() == ManagerID.NetManager)
            {
                ProcessEvent(msg);
            }//本模块 自己处理
            else
                MsgCenter.Instance.SendToMsg(msg);
        }

        public GameObject GetGameObject(string name)
        {
            if (sonMembers.ContainsKey(name))
            {
                return sonMembers[name];
            }
            return null;
        }

        public void RegistGameObject(string name, GameObject obj)
        {
            if (!sonMembers.ContainsKey(name)) 
            {
                sonMembers.Add(name, obj);
            }
        }

        public void UnRegistGameObject(string name)
        {
            if (sonMembers.ContainsKey(name))
            {
                sonMembers.Remove(name);
            }
        }
    }

}