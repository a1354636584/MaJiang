using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LYFrame;

namespace LYFrame
{
    public class MsgCenter : MonoBehaviour
    {
        public static MsgCenter Instance = null;

        private void Awake()
        {
            Instance = this;

            //#if USE_MutiMSGQueue
            //        msgQueue=new Queue<MsgBase>();
            //#endif

           
         
            gameObject.AddComponent<UIManager>();
            gameObject.AddComponent<ModelManager>();

            //gameObject.AddComponent<CharaterManager>();
            //gameObject.AddComponent<NetManager>();
            //gameObject.AddComponent<AssetManager>();
            //gameObject.AddComponent<NaticeResLoader>();
            //gameObject.AddComponent<ILoadManager>();PartManager
            gameObject.AddComponent<CameraManager>();

            DontDestroyOnLoad(gameObject);
        }

        public void SendToMsg(MsgBase msg)
        {
            ManagerID id = msg.GetManager();
            switch (id)
            {
                case ManagerID.GameManager:
                    break;
                case ManagerID.UIManager:
                    UIManager.Instance.SendMsg(msg);
                    break;
                case ManagerID.AudioManager:
                    break;
                case ManagerID.CharaterManager:
                    CharaterManager.Instance.SendMsg(msg);
                    break;
                case ManagerID.NetManager:
                    NetManager.Instance.SendMsg(msg);
                    break;
                case ManagerID.AssetManager:
                    AssetManager.Instance.SendMsg(msg);
                    break;
                case ManagerID.ModelManager:
                    ModelManager.Instance.SendMsg(msg);
                    break;
                case ManagerID.CameraManager:
                    CameraManager.Instance.SendMsg(msg);
                    break;
                default:
                    break;
            }
        }
    }

}