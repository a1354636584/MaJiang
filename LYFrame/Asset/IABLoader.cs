using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;


namespace LYFrame
{
    public delegate void LoadProgrecess(string bundleName, float progrecess);
    public delegate void loadFinished(string bundleName);

    /// <summary>
    /// 单个资源包加载管理
    /// </summary>
    public class IABLoader
    {
        private string bundleName;

        private string commonBundlePath;

        private WWW www;

        private float progress;

        private LoadProgrecess loadProgress;

        private loadFinished loadFinished;

        private IABResLoader abResLoader;

        public IABLoader(LoadProgrecess loadProgress, loadFinished loadFinished)
        {
            bundleName = "";
            commonBundlePath = "";
            progress = 0;
            this.loadProgress = null;
            this.loadFinished = null;
            www = null;

            this.loadProgress = loadProgress;
            this.loadFinished = loadFinished;
        }

        /// <summary>
        /// 设置包名  sceneone/prefab
        /// </summary>
        /// <param name="bundleName"></param>
        public void SetBundleName(string bundleName)
        {
            this.bundleName = bundleName;
        }

        /// <summary>
        /// 上层传入完整路径
        /// </summary>
        /// <param name="path"></param>
        public void LoadResources(string path)
        {
            commonBundlePath = path;

            //StartCoroutine(CommonLoad);
        }

        public IEnumerator CommonLoad()
        {
            Debug.Log(commonBundlePath);
            www = new WWW(commonBundlePath);
            while (!www.isDone)
            {
                progress = www.progress;
                if (loadProgress != null)
                {
                    loadProgress(bundleName, progress);
                }

                yield return www.progress;
                progress = www.progress;
            }

            if (progress >= 1.0f)
            {
                abResLoader = new IABResLoader(www.assetBundle);

                if (loadProgress != null)
                {
                    loadProgress(bundleName, progress);
                }

                if (loadFinished != null)
                {
                    loadFinished(bundleName);
                }

                // abResLoader = new IABResLoader(www.assetBundle);
            }
            else
            {
                Debug.LogError("load bundle error ==" + bundleName);
            }

            www = null;
        }




        #region 由下层提供功能

        /// <summary>
        /// 卸载单个资源
        /// </summary>
        /// <param name="obj"></param>
        public void UnLoadAssetRes(UnityEngine.Object obj)
        {
            if (abResLoader != null)
                abResLoader.UnLoadRes(obj);
        }

        /// <summary>
        /// 注销
        /// </summary>
        public void Dispose()
        {
            if (abResLoader != null)
                abResLoader.Dispose(); abResLoader = null;
        }

        /// <summary>
        /// 打印所有资源
        /// </summary>
        public void DebugerLoader()
        {
            if (abResLoader != null)
                abResLoader.DebugAllRes();
        }

        /// <summary>
        /// 获取单个资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UnityEngine.Object GetSingleResources(string name)
        {
            if (abResLoader != null)
                return abResLoader[name];
            else
                return null;
        }

        /// <summary>
        /// 获取多个资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UnityEngine.Object[] GetMutResources(string name)
        {
            if (abResLoader != null)
                return abResLoader.LoadResources(name);
            else
                return null;
        }



        #endregion


    }
}