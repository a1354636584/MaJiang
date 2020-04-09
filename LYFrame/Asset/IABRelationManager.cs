using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LYFrame
{
    /// <summary>
    /// 单个资源bundle包依赖关系管理
    /// </summary>
    public class IABRelationManager
    {

        /// <summary>所依赖的assetbundle集合</summary>
        List<string> listDependenceBundle;

        /// <summary>被依赖的assetbundle集合</summary>
        List<string> listReferBundle;

        IABLoader assetLoader;

        bool IsLoadFinish;

        string bundleName;

        LoadProgrecess progrecess;

        public void Init(string bundleName, LoadProgrecess loadProgress)
        {
            IsLoadFinish = false;
            this.progrecess = loadProgress;
            this.bundleName = bundleName;
            assetLoader = new IABLoader(loadProgress, LoadBundleFinish);

            assetLoader.SetBundleName(bundleName);

            Debug.Log(bundleName);
            string bundlePath = IPathTools.GetWWWAssetBundlePath() + "/" + bundleName;


          //  string bundlePath = "file:///E:/Windows" + "/" + bundleName;


            assetLoader.LoadResources(bundlePath);
        }

        public string GetBundleName()
        {
            return bundleName;
        }

        private void LoadBundleFinish(string bundleName)
        {
            IsLoadFinish = true;
        }

        /// <summary>
        /// 是否加载完成
        /// </summary>
        /// <returns></returns>
        public bool IsBundleLoadFinish()
        {
            return IsLoadFinish;
        }

        public IABRelationManager()
        {
            listDependenceBundle = new List<string>();
            listReferBundle = new List<string>();
        }

        /// <summary>
        /// 添加被依赖的资源assetbundle名
        /// </summary>
        /// <param name="bundleName"></param>
        public void AddReference(string bundleName)
        {
            listReferBundle.Add(bundleName);
        }

        /// <summary>
        /// 获取被依赖的资源assetbundle名集合
        /// </summary>
        /// <returns></returns>
        public List<string> GetReference()
        {
            return listReferBundle;
        }

        /// <summary>
        /// 移除被依赖关系
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns>表示是否释放自己,即没有被其他资源依赖</returns>
        public bool RemoveReference(string bundleName)
        {
            for (int i = 0; i < listReferBundle.Count; i++)
            {
                if (bundleName.Equals(listReferBundle[i]))
                {
                    listReferBundle.RemoveAt(i);
                }
            }

            if (listReferBundle.Count <= 0)
            {
                Dispose();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 设置依赖关系
        /// </summary>
        /// <param name="dependences"></param>
        public void SetDependence(string[] dependences)
        {
            if (dependences.Length > 0)
            {
                listDependenceBundle.AddRange(dependences);
            }
        }

        /// <summary>
        /// 获取依赖关系
        /// </summary>
        /// <returns></returns>
        public List<string> GetDependence()
        {
            return listDependenceBundle;
        }

        /// <summary>
        /// 移除依赖关系
        /// </summary>
        /// <param name="bundleName"></param>
        public void RemoveDependence(string bundleName)
        {
            for (int i = 0; i < listDependenceBundle.Count; i++)
            {
                if (bundleName.Equals(listDependenceBundle[i]))
                {
                    listDependenceBundle.RemoveAt(i);
                }
            }
        }

        #region 由下层提供功能

        public IEnumerator LoadAssetBundle()
        {
            yield return assetLoader.CommonLoad();
        }


        /// <summary>
        /// 卸载单个资源
        /// </summary>
        /// <param name="obj"></param>
        public void UnLoadAssetRes(UnityEngine.Object obj)
        {
            if (assetLoader != null)
                assetLoader.UnLoadAssetRes(obj);
        }

        /// <summary>
        /// 注销
        /// </summary>
        public void Dispose()
        {
            if (assetLoader != null)
                assetLoader.Dispose(); assetLoader = null;
        }

        /// <summary>
        /// 打印所有资源
        /// </summary>
        public void DebugerLoader()
        {
            if (assetLoader != null)
                assetLoader.DebugerLoader();
            else
                Debug.Log("asset load is null");
        }

        /// <summary>
        /// 获取单个资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UnityEngine.Object GetSingleResources(string name)
        {
            if (assetLoader != null)
                return assetLoader.GetSingleResources(name);
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
            if (assetLoader != null)
                return assetLoader.GetMutResources(name);
            else
                return null;
        }

        public LoadProgrecess GetProgrecess()
        {
            return progrecess;
        }

        #endregion
    }


}