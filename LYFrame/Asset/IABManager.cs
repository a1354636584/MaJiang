using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;

namespace LYFrame
{
    public delegate void LoadAssetBundleCallBack(string sceneName, string bundleName);

    /// <summary>
    ///  单个obj 里面可能有多个
    /// </summary>
    public class AssetObj
    {

        public List<UnityEngine.Object> objs;

        public AssetObj(params UnityEngine.Object[] tmpObj)
        {
            objs = new List<UnityEngine.Object>();
            objs.AddRange(tmpObj);
        }

        public void ReleaseObj()
        {
            Debug.Log("count--" + objs.Count);
            for (int i = 0; i < objs.Count; i++)
            {
                Debug.Log("release-- " + i);
                Debug.Log(objs[i]);
                //GameObject.Destroy(objs[i]);
                Resources.UnloadAsset(objs[i]);
            }
        }
    }

    /// <summary>
    /// 存的是一个bundle包里obj
    /// </summary>
    public class AssetResObj
    {
        /// <summary>
        /// 缓存一个bundle包里所有的资源对象
        /// </summary>
        public Dictionary<string, AssetObj> resObj = new Dictionary<string, AssetObj>();

        public AssetResObj(string name, AssetObj obj)
        {
            AddResObj(name, obj);
        }

        /// <summary>
        /// 缓存资源
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        public void AddResObj(string name, AssetObj obj)
        {
            resObj.Add(name, obj);
        }

        /// <summary>
        /// 释放全部资源
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tmpObj"></param>
        public void ReleaseAllResObj()
        {
            List<string> keys = new List<string>();
            keys.AddRange(resObj.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                ReleaseResObj(keys[i]);
            }
        }

        /// <summary>
        /// 释放单个资源
        /// </summary>
        /// <param name="name"></param>
        public void ReleaseResObj(string name)
        {
            if (resObj.ContainsKey(name))
            {
                AssetObj tmpObj = resObj[name];
                tmpObj.ReleaseObj();
            }
            else
            {
                Debug.Log(" release object name is not exist ==" + name);
            }
        }

        public List<UnityEngine.Object> GetResObj(string name)
        {
            if (resObj.ContainsKey(name))
            {
                AssetObj tmpObj = resObj[name];
                return tmpObj.objs;
            }
            else
            {
                Debug.Log("  object name is not exist ==" + name);
                return null;
            }
        }

    }

    public class IABManager
    {

        Dictionary<string, IABRelationManager> loadHelper = new Dictionary<string, IABRelationManager>();

        /// <summary>
        /// 缓存bundle包
        /// </summary>
        Dictionary<string, AssetResObj> loadObj = new Dictionary<string, AssetResObj>();

        string sceneName;

        public IABManager(string sceneName)
        {
            this.sceneName = sceneName;
        }


        #region 由下层提供


        /// <summary>
        /// 是否加载了bundle
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public bool IsLoadingAssetBundle(string bundleName)
        {
            if (loadHelper.ContainsKey(bundleName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否已经加载完成
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public bool IsLoadingFinish(string bundleName)
        {
            if (loadObj.ContainsKey(bundleName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 打印该资源包中所有资源bundle名
        /// </summary>
        /// <param name="bundleName"></param>
        public void DebugAssetBundle(string bundleName)
        {
            if (loadHelper.ContainsKey(bundleName))
            {
                IABRelationManager loader = loadHelper[bundleName];
                loader.DebugerLoader();
            }
            else
            {
                Debug.Log("not contain assetbundle ==" + bundleName);
            }

        }

        public UnityEngine.Object GetSingleResourcess(string bundleName, string resName)
        {
            if (loadObj.ContainsKey(bundleName))
            {
                List<UnityEngine.Object> listObj = loadObj[bundleName].GetResObj(resName);
                if (listObj != null)
                {
                    return listObj[0];
                }
            }

            if (loadHelper.ContainsKey(bundleName))//表示已经加载过bundle
            {
                IABRelationManager loader = loadHelper[bundleName];

                // lhy change bundleName --> resName
                UnityEngine.Object tmpObj = loader.GetSingleResources(resName);

                AssetObj tmpAssetObj = new AssetObj(tmpObj);

                //缓存里面是否有这个bundle包
                if (loadObj.ContainsKey(bundleName))
                {
                    AssetResObj tmpAssetResObj = loadObj[bundleName];
                    tmpAssetResObj.AddResObj(resName, tmpAssetObj);
                }
                else
                {
                    AssetResObj tmpRes = new AssetResObj(resName, tmpAssetObj);
                    loadObj.Add(bundleName, tmpRes);
                }

                return tmpObj;
            }
            else
            {//没有加载过
                return null;
            }
        }

        public UnityEngine.Object[] GetMutiResourcess(string bundleName, string resName)
        {
            if (loadObj.ContainsKey(bundleName))
            {
                List<UnityEngine.Object> listObj = loadObj[bundleName].GetResObj(resName);
                if (listObj != null)
                {
                    return listObj.ToArray();
                }
            }

            if (loadHelper.ContainsKey(bundleName))//表示已经加载过bundle
            {
                IABRelationManager loader = loadHelper[bundleName];
                UnityEngine.Object[] tmpObj = loader.GetMutResources(bundleName);

                AssetObj tmpAssetObj = new AssetObj(tmpObj);

                //缓存里面是否有这个bundle包
                if (loadObj.ContainsKey(bundleName))
                {
                    AssetResObj tmpAssetResObj = loadObj[bundleName];
                    tmpAssetResObj.AddResObj(resName, tmpAssetObj);
                }
                else
                {
                    AssetResObj tmpRes = new AssetResObj(resName, tmpAssetObj);
                    loadObj.Add(bundleName, tmpRes);
                }

                return tmpObj;
            }
            else
            {//没有加载过
                return null;
            }
        }



        #endregion


        #region 释放缓存物体
        /// <summary>
        /// 释放一个资源obj
        /// </summary>
        /// <param name="bundleNmae"></param>
        /// <param name="resName"></param>
        public void DisposeResObj(string bundleNmae, string resName)
        {
            if (loadObj.ContainsKey(bundleNmae))
            {
                AssetResObj tmp = loadObj[bundleNmae];
                tmp.ReleaseResObj(resName);
            }
            else
            {
                Debug.Log("bundle is not exist ==" + bundleNmae);
            }
        }

        /// <summary>
        /// 释放一个bundle包里所有资源obj
        /// </summary>
        /// <param name="bundleNmae"></param>
        /// <param name="resName"></param>
        public void DisposeResObj(string bundleNmae)
        {
            if (loadObj.ContainsKey(bundleNmae))
            {
                AssetResObj tmp = loadObj[bundleNmae];
                tmp.ReleaseAllResObj();
                //
                loadObj.Remove(bundleNmae);
            }
            else
            {
                Debug.Log("bundle is not exist ==" + bundleNmae);
            }

            Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 循环处理依赖关系
        /// </summary>
        /// <param name="bundleNmae"></param>
        public void DisposeBundle(string bundleNmae)
        {
            Debug.Log(bundleNmae);

            if (loadHelper.ContainsKey(bundleNmae))
            {
                IABRelationManager loader = loadHelper[bundleNmae];

                List<string> depences = loader.GetDependence();
                for (int i = 0; i < depences.Count; i++)
                {
                    if (loadHelper.ContainsKey(depences[i]))
                    {
                        IABRelationManager dependce = loadHelper[depences[i]];
                        if (dependce.RemoveReference(bundleNmae))
                        {
                            DisposeBundle(dependce.GetBundleName());
                        }
                    }
                }

                if (loader.GetReference().Count <= 0)
                {
                    loader.Dispose();
                    loadHelper.Remove(bundleNmae);
                }
            }
            else
            {
                Debug.Log("bundle is not exist ==" + bundleNmae);
            }

        }

        public void DisposeAllBundle()
        {

            List<string> keys = new List<string>();
            keys.AddRange(loadHelper.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                IABRelationManager loader = loadHelper[keys[i]];
                loader.Dispose();
            }

            loadHelper.Clear();
        }

        public void DisposeAllBundleAndRes()
        {

            DisposeAllObj();

            DisposeAllBundle();
        }

        /// <summary>
        /// 释放所有bundle包
        /// </summary>
        public void DisposeAllObj()
        {
            List<string> keys = new List<string>();
            keys.AddRange(loadObj.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                DisposeResObj(keys[i]);
            }

            loadObj.Clear();
        }

        string[] GetDependences(string bundleName)
        {
            return IABManifestLoader.Instance.GetDependence(bundleName);
        }

        /// <summary>
        /// (外部调用)
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="progrecess"></param>
        /// <param name="callBack"></param>
        public void LoadAssetBundle(string bundleName, LoadProgrecess progrecess, LoadAssetBundleCallBack callBack)
        {
            if (!loadHelper.ContainsKey(bundleName))
            {
                IABRelationManager loader = new IABRelationManager();

                loader.Init(bundleName, progrecess);

                loadHelper.Add(bundleName, loader);

                callBack(sceneName, bundleName);
            }
            else
            {
                Debug.Log("IABManager have contain bundle name ==" + bundleName);
            }

        }

        public IEnumerator LoadAssetBundleDependences(string bundleName, string refName, LoadProgrecess progrecess)
        {

            if (!loadHelper.ContainsKey(bundleName))
            {
                IABRelationManager loader = new IABRelationManager();

                loader.Init(bundleName, progrecess);


                if (refName != null)
                {
                    loader.AddReference(refName);
                }

                loadHelper.Add(bundleName, loader);

                yield return LoadAssetBundles(bundleName);

            }
            else
            {

                if (refName != null)
                {
                    IABRelationManager loader = loadHelper[bundleName];
                    loader.AddReference(bundleName);
                }
            }
        }

        public IEnumerator LoadAssetBundles(string bundleName)
        {
            while (!IABManifestLoader.Instance.IsLoadFinish())
            {
                yield return null;
            }

            IABRelationManager loader = loadHelper[bundleName];

            string[] dependences = GetDependences(bundleName);

            loader.SetDependence(dependences);

            for (int i = 0; i < dependences.Length; i++)
            {
                yield return LoadAssetBundleDependences(dependences[i], bundleName, loader.GetProgrecess());
            }
            yield return loader.LoadAssetBundle();
        }



        #endregion
    }

}