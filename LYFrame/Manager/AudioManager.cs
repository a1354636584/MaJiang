using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LYFrame
{
    public class AudioManager : ManagerBase
    {

        public static AudioManager Instance = null;

        private Dictionary<string, GameObject> sonMembers = new Dictionary<string, GameObject>();

        private void Awake()
        {
            Instance = this;
        }

        public void SendMsg(MsgBase msg)
        {
            if (msg.GetManager() == ManagerID.AudioManager)
                ProcessEvent(msg);//本模块 自己处理
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