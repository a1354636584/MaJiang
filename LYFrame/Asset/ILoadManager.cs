using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LYFrame
{
    public class ILoadManager : MonoBase
    {

        public static ILoadManager Instance = null;

        private Dictionary<string, IABSceneManager> dicLoadManager = new Dictionary<string, IABSceneManager>();

        public override void ProcessEvent(MsgBase msg)
        {

        }

        private void Awake()
        {
            Instance = this;

            //第一步 加载 IABManifest
            StartCoroutine(IABManifestLoader.Instance.LoadManifest());

        }
        /// <summary>
        /// 第二步 读取配置文件
        /// </summary>
        /// <param name="sceneName"></param>
        public void ReadConfiger(string sceneName)
        {

            if (!dicLoadManager.ContainsKey(sceneName))
            {
                IABSceneManager tmpManager = new IABSceneManager(sceneName);
                tmpManager.ReadConfiger(sceneName);
                dicLoadManager.Add(sceneName, tmpManager);
            }
        }

        private void LoadCallBack(string sceneName, string bundleName)
        {
            if (dicLoadManager.ContainsKey(sceneName))
            {
                IABSceneManager tmpManager = dicLoadManager[sceneName];
                StartCoroutine(tmpManager.LoadAssetSys(bundleName));
            }
            else
            {
                Debug.Log("bundle name is not  contain ==" + sceneName);
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="bundleName"></param>
        /// <param name="progress"></param>
        public void LoadAsset(string sceneName, string bundleName, LoadProgrecess progress)
        {

            if (!dicLoadManager.ContainsKey(sceneName))
            {
                ReadConfiger(sceneName);

            }

            IABSceneManager tmpManager = dicLoadManager[sceneName];
            tmpManager.LoadAsset(bundleName, progress, LoadCallBack);
        }

        #region 由下层提供API

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="bundleName"></param>
        /// <param name="resName">资源的名称</param>
        /// <returns></returns>
        public UnityEngine.Object GetSingleResources(string sceneName, string bundleName, string resName)
        {
            if (dicLoadManager.ContainsKey(sceneName))
            {
                IABSceneManager tmpManager = dicLoadManager[sceneName];
                return tmpManager.GetSingleResources(bundleName, resName);
            }
            else
            {
                Debug.Log("Is not  load ==" + sceneName + "/" + bundleName);
                return null;
            }
        }

        public UnityEngine.Object[] GetMutResources(string sceneName, string bundleName, string resName)
        {
            if (dicLoadManager.ContainsKey(sceneName))
            {
                IABSceneManager tmpManager = dicLoadManager[sceneName];
                return tmpManager.GetMutResources(bundleName, resName);
            }
            else
            {
                Debug.Log("Is not  load ==" + sceneName + "/" + bundleName);
                return null;
            }

        }

        /// <summary>
        /// 释放某一个资源obj
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="bundleName"></param>
        /// <param name="resName"></param>
        public void UnLoadResObj(string sceneName, string bundleName, string resName)
        {

            if (dicLoadManager.ContainsKey(sceneName))
            {
                IABSceneManager tmpManager = dicLoadManager[sceneName];
                tmpManager.DisposeResObj(bundleName, resName);
            }
        }

        /// <summary>
        /// 释放整个包obj
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="bundleName"></param>
        /// <param name="resName"></param>
        public void UnLoadBundleResObjs(string sceneName, string bundleName, string resName)
        {
            if (dicLoadManager.ContainsKey(sceneName))
            {
                IABSceneManager tmpManager = dicLoadManager[sceneName];
                tmpManager.DisposeBundleRes(bundleName);
            }
        }

        /// <summary>
        /// 释放整个场景的obj
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="bundleName"></param>
        /// <param name="resName"></param>
        public void UnLoadAllObjs(string sceneName, string bundleName, string resName)
        {
            if (dicLoadManager.ContainsKey(sceneName))
            {
                IABSceneManager tmpManager = dicLoadManager[sceneName];
                tmpManager.DisposeAllObj();
            }
        }

        /// <summary>
        /// 释放一个bundle
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="bundleName"></param>
        /// <param name="resName"></param>
        public void UnLoadAssetBundle(string sceneName, string bundleName, string resName)
        {
            Debug.Log(sceneName);
            if (dicLoadManager.ContainsKey(sceneName))
            {
                IABSceneManager tmpManager = dicLoadManager[sceneName];
                tmpManager.DisposeBundle(bundleName);
            }
        }

        /// <summary>
        /// 释放一个场景所有的bundle包
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="bundleName"></param>
        /// <param name="resName"></param>
        public void UnLoadAllAssetBundle(string sceneName, string bundleName, string resName)
        {
            if (dicLoadManager.ContainsKey(sceneName))
            {
                IABSceneManager tmpManager = dicLoadManager[sceneName];
                tmpManager.DisposeAllBundle();

                System.GC.Collect();
            }
        }

        /// <summary>
        /// 释放一个场景所有的bundle包和obj
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="bundleName"></param>
        /// <param name="resName"></param>
        public void UnLoadAllBundleAndResObjs(string sceneName, string bundleName, string resName)
        {
            if (dicLoadManager.ContainsKey(sceneName))
            {
                IABSceneManager tmpManager = dicLoadManager[sceneName];
                tmpManager.DisposeAllBundleAndRes();
                System.GC.Collect();
            }
        }

        public void DebugAllAssetBundle(string sceneName)
        {
            if (dicLoadManager.ContainsKey(sceneName))
            {
                IABSceneManager tmpManager = dicLoadManager[sceneName];
                tmpManager.DebugAllAsset();
            }
        }


        #endregion

        /// <summary>
        /// 是否完成加载
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public bool IsLoadingBundleFinish(string sceneName, string bundleName)
        {
            bool tmpBool = dicLoadManager.ContainsKey(sceneName);
            if (tmpBool)
            {
                IABSceneManager tmpManager = dicLoadManager[sceneName];

                return tmpManager.IsLoadingFinish(bundleName);
            }

            return false;
        }

        /// <summary>
        /// 是否已经加载
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public bool IsLoadingBundle(string sceneName, string bundleName)
        {
            bool tmpBool = dicLoadManager.ContainsKey(sceneName);
            if (tmpBool)
            {
                IABSceneManager tmpManager = dicLoadManager[sceneName];

                return tmpManager.IsLoadingAssetBundle(bundleName);
            }

            return false;
        }

        public string GetBundleReateName(string sceneName, string bundleName)
        {
            if (dicLoadManager.ContainsKey(sceneName))
            {
                return dicLoadManager[sceneName].GetBundleReateName(bundleName);
            }
            else
            {
                return null;
            }
        }

        private void OnDestroy()
        {
            dicLoadManager.Clear();
            System.GC.Collect();
        }
    }

}