using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LYFrame
{
    public class IABManifestLoader
    {

        private static IABManifestLoader instance = null;

        public static IABManifestLoader Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IABManifestLoader();
                }
                return instance;
            }
        }

        public AssetBundleManifest assetManifest;

        public string manifestPath;

        public bool isLoadFinish;

        public AssetBundle manifestLoder;

        public IABManifestLoader()
        {
            assetManifest = null;
            manifestLoder = null;
            isLoadFinish = false;

            manifestPath = IPathTools.GetWWWAssetBundlePath() + "/" + IPathTools.GetPlatformFolderName(Application.platform);
        }

        public IEnumerator LoadManifest()
        {
            WWW www = new WWW(manifestPath);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log("error:" + www.error);
            }
            else
            {
                if (www.progress >= 1.0f)
                {
                    manifestLoder = www.assetBundle;
                    assetManifest = manifestLoder.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                    isLoadFinish = true;
                }
            }
        }

        public void SetManifestPath(string path)
        {
            manifestPath = path;
        }

        public bool IsLoadFinish()
        {
            return isLoadFinish;
        }

        public string[] GetDependence(string name)
        {
            return assetManifest.GetDirectDependencies(name);
        }

        public void UnLoadManifest()
        {
            manifestLoder.Unload(true);
        }

    }

}