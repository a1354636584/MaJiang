using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LYFrame
{
    public delegate void NativeResCallBack(NativeResCallBackNode tmpNode);

    public class NativeResCallBackNode
    {

        public string sceneName;
        public string bundleName;
        public string resName;
        public ushort backMsgId;
        public bool isSingle;

        public NativeResCallBackNode nextValue;

        public NativeResCallBack callBack;

        public NativeResCallBackNode(bool tmpSingle, string tmpSceneName, string tmpBundleName, string tmpResName, ushort tmpBackId, NativeResCallBack tmpCallBack, NativeResCallBackNode tmpNode)
        {
            this.isSingle = tmpSingle;
            this.sceneName = tmpSceneName;
            this.bundleName = tmpBundleName;
            this.resName = tmpResName;
            this.backMsgId = tmpBackId;

            this.nextValue = tmpNode;
            this.callBack = tmpCallBack;
        }

        public void Dispose()
        {
            callBack = null;
            nextValue = null;
        }
    }

    public class NativeResCallBackManager
    {

        Dictionary<string, NativeResCallBackNode> manager = null;

        public NativeResCallBackManager()
        {
            manager = new Dictionary<string, NativeResCallBackNode>();

        }

        public void AddBundle(string bundleName, NativeResCallBackNode tmpNode)
        {
            if (manager.ContainsKey(bundleName))
            {
                NativeResCallBackNode node = manager[bundleName];

                while (node.nextValue != null)
                {
                    node = node.nextValue;
                }

                node.nextValue = tmpNode;
            }
            else
            {
                manager.Add(bundleName, tmpNode);
            }
        }

        /// <summary>
        /// 加载完成后 消息向上层传递释放bundle
        /// </summary>
        /// <param name="bundleName"></param>
        public void Dispose(string bundleName)
        {
            if (manager.ContainsKey(bundleName))
            {
                NativeResCallBackNode curNode = manager[bundleName];
                while (curNode.nextValue != null)
                {
                    NativeResCallBackNode tmpNode = curNode;
                    curNode = curNode.nextValue;
                    tmpNode.Dispose();
                }

                curNode.Dispose();

                manager.Remove(bundleName);
            }
        }

        public void CallBackRes(string bundleName)
        {
            if (manager.ContainsKey(bundleName))
            {
                NativeResCallBackNode curNode = manager[bundleName];
                do
                {
                    curNode.callBack(curNode);
                    curNode = curNode.nextValue;
                } while (curNode != null);
            }
        }

    }

    public class NaticeResLoader : AssetBase
    {
        public override void ProcessEvent(MsgBase msg)
        {
            switch (msg.msgId)
            {
                case (ushort)AssetEvent.ReleaseSingleObj:
                    {
                        HunkAssetRes tmpMsg = (HunkAssetRes)msg;
                        Debug.Log(tmpMsg.sceneName + "  " + tmpMsg.bundleName);
                        ILoadManager.Instance.UnLoadBundleResObjs(tmpMsg.sceneName, tmpMsg.bundleName, tmpMsg.resName);
                    }
                    break;
                case (ushort)AssetEvent.HunkRes:
                    {
                        HunkAssetRes tmpMsg = (HunkAssetRes)msg;
                        Debug.Log("HunkRes");
                        GetResources(tmpMsg.sceneName, tmpMsg.bundleName, tmpMsg.resName, tmpMsg.isSingle, tmpMsg.backMsgId);
                    }
                    break;
                default:
                    break;
            }
        }

        HunkAssetResBack resBackMsg = null;

        NativeResCallBackManager callBack = null;
        NativeResCallBackManager CallBack
        {
            get
            {
                if (callBack == null)
                {
                    callBack = new NativeResCallBackManager();
                }

                return callBack;
            }
        }

        public HunkAssetResBack ReleaseBackMsg
        {
            get
            {
                if (resBackMsg == null)
                {
                    resBackMsg = new HunkAssetResBack(0, null);
                }
                return resBackMsg;
            }
        }


        private void Start()
        {
            msgIds = new ushort[] {
            (ushort)AssetEvent.ReleaseSingleObj,
            (ushort)AssetEvent.HunkRes,
        };

            RegistSelf(this, msgIds);
        }

        public void SendToBackMsg(NativeResCallBackNode node)
        {
            if (node.isSingle)
            {
                UnityEngine.Object obj = ILoadManager.Instance.GetSingleResources(node.sceneName, node.bundleName, node.resName);
                this.ReleaseBackMsg.Changer(node.backMsgId, obj);

                SendMsg(ReleaseBackMsg);
            }
            else
            {
                UnityEngine.Object[] objs = ILoadManager.Instance.GetMutResources(node.sceneName, node.bundleName, node.resName);
                this.ReleaseBackMsg.Changer(node.backMsgId, objs);

                SendMsg(ReleaseBackMsg);
            }
        }

        public void GetResources(string sceneName, string bundleName, string resName, bool isSingle, ushort backId)
        {

            if (!ILoadManager.Instance.IsLoadingBundle(sceneName, bundleName))
            {//没有加载
                ILoadManager.Instance.LoadAsset(sceneName, bundleName, LoadProgress);

                string bundleFullName = ILoadManager.Instance.GetBundleReateName(sceneName, bundleName);

                if (bundleFullName != null)
                {
                    NativeResCallBackNode tmpNode = new NativeResCallBackNode(isSingle, sceneName, bundleName, resName, backId, SendToBackMsg, null);

                    CallBack.AddBundle(bundleFullName, tmpNode);
                }
                else
                {
                    Debug.Log("Dont contain bundle ==" + bundleName);
                }
            }
            else if (ILoadManager.Instance.IsLoadingBundleFinish(sceneName, bundleName))
            {//已经加载
                if (isSingle)
                {
                    UnityEngine.Object obj = ILoadManager.Instance.GetSingleResources(sceneName, bundleName, resName);
                    this.ReleaseBackMsg.Changer(backId, obj);
                    SendMsg(ReleaseBackMsg);
                }
                else
                {
                    UnityEngine.Object[] objs = ILoadManager.Instance.GetMutResources(sceneName, bundleName, resName);
                    this.ReleaseBackMsg.Changer(backId, objs);
                    SendMsg(ReleaseBackMsg);
                }
            }
            else
            { //已经加载 但是没有完成
                string bundleFullName = ILoadManager.Instance.GetBundleReateName(sceneName, bundleName);
                if (bundleFullName != null)
                {
                    NativeResCallBackNode tmpNode = new NativeResCallBackNode(isSingle, sceneName, bundleName, resName, backId, SendToBackMsg, null);

                    CallBack.AddBundle(bundleFullName, tmpNode);
                }
                else
                {
                    Debug.Log("Dont contain bundle ==" + bundleName);
                }
            }

        }

        private void LoadProgress(string bundleName, float progrecess)
        {
            if (progrecess >= 1.0f)
            {
                CallBack.CallBackRes(bundleName);
                CallBack.Dispose(bundleName);
            }
        }


    }

}