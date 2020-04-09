using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace LYFrame
{
    /// <summary>
    /// 单个资源包加载
    /// </summary>
    public class IABResLoader : IDisposable
    {
        private AssetBundle ABRes;

        public IABResLoader(AssetBundle tmpAB)
        {
            ABRes = tmpAB;
        }

        public UnityEngine.Object this[string resName]
        {
            get
            {
                if (this.ABRes == null || !this.ABRes.Contains(resName))
                {
                    Debug.LogError("res not contain");
                    return null;
                }

                return ABRes.LoadAsset(resName);

            }
        }

        public UnityEngine.Object[] LoadResources(string resName)
        {
            if (this.ABRes == null || !this.ABRes.Contains(resName))
            {
                Debug.LogError("res not contain");
                return null;
            }

            return this.ABRes.LoadAssetWithSubAssets(resName);

        }

        /// <summary>
        /// 卸载单个资源
        /// </summary>
        /// <param name="resObj"></param>
        public void UnLoadRes(UnityEngine.Object resObj)
        {
            Resources.UnloadAsset(resObj);
        }

        /// <summary>
        /// 释放assetbundle包
        /// </summary>
        public void Dispose()
        {
            if (ABRes == null)
                return;
            ABRes.Unload(false);
        }

        public void DebugAllRes()
        {

            string[] temAssetbundle = ABRes.GetAllAssetNames();
            for (int i = 0; i < temAssetbundle.Length; i++)
            {
                Debug.Log("ABRes contain asset name == " + temAssetbundle[i]);
            }
        }

    }

}