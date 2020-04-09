using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;


namespace LYFrame
{
    public class IABSceneManager
    {

        IABManager abManager;

        Dictionary<string, string> dicAllAsset;



        public IABSceneManager(string sceneName)
        {
            abManager = new IABManager(sceneName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">场景名字 sceneone</param>
        public void ReadConfiger(string sceneName)
        {
            string textGileName = "Record.txt";
            string path = IPathTools.GetAssetBundlePath() + "/" + sceneName + textGileName;
            dicAllAsset = new Dictionary<string, string>();

            //abManager = new IABManager(sceneName);

            ReadConfig(path);
        }


        //        foreach (string key in readDic.Keys)
        //        {
        //            sw.Write(key);
        //            sw.Write(" ");
        //            sw.Write(readDic[key]);
        //            sw.Write("\n");
        //        }

        //sw.Close();
        //        fs.Close();


        private void ReadConfig(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader br = new StreamReader(fs);

            int allCount = br.Read();

            for (int i = 0; i < allCount; i++)
            {
                string temStr = br.ReadLine();

                if (string.IsNullOrEmpty(temStr))
                {
                    break;
                }
                string[] array = temStr.Split(' ');
                dicAllAsset.Add(array[0], array[1]);
            }
            br.Close();
            fs.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bundleName">key</param>
        /// <param name="progrecess"></param>
        /// <param name="callBack"></param>
        public void LoadAsset(string bundleName, LoadProgrecess progrecess, LoadAssetBundleCallBack callBack)
        {

            if (dicAllAsset.ContainsKey(bundleName))
            {
                string tmpValue = dicAllAsset[bundleName];
                Debug.Log(tmpValue);
                abManager.LoadAssetBundle(tmpValue, progrecess, callBack);
            }
            else
            {
                Debug.Log("Dnot contain the bundle ==" + bundleName);
            }
        }

        public IEnumerator LoadAssetSys(string bundleName)
        {

            yield return abManager.LoadAssetBundles(bundleName);
        }

        public UnityEngine.Object GetSingleResources(string bundleName, string resName)
        {

            if (dicAllAsset.ContainsKey(bundleName))
            {
                return abManager.GetSingleResourcess(dicAllAsset[bundleName], resName);
            }
            else
            {
                Debug.Log("Dnot contain the bundle ==" + bundleName);

                return null;
            }
        }

        public UnityEngine.Object[] GetMutResources(string bundleName, string resName)
        {
            if (dicAllAsset.ContainsKey(bundleName))
            {
                return abManager.GetMutiResourcess(dicAllAsset[bundleName], resName);
            }
            else
            {
                Debug.Log("Dnot contain the bundle ==" + bundleName);

                return null;
            }

        }

        public void DisposeResObj(string bundleName, string resName)
        {

            if (dicAllAsset.ContainsKey(bundleName))
            {
                abManager.DisposeResObj(dicAllAsset[bundleName], resName);
            }
            else
            {
                Debug.Log("Dnot contain the bundle ==" + bundleName);
            }
        }


        public void DisposeBundleRes(string bundleName)
        {

            if (dicAllAsset.ContainsKey(bundleName))
            {
                abManager.DisposeResObj(dicAllAsset[bundleName]);
            }
            else
            {
                Debug.Log("Dnot contain the bundle ==" + bundleName);
            }
        }

        public void DisposeBundle(string bundleName)
        {
            Debug.Log(bundleName);
            if (dicAllAsset.ContainsKey(bundleName))
            {
                Debug.Log(dicAllAsset[bundleName]);
                abManager.DisposeBundle(dicAllAsset[bundleName]);
            }
            else
            {
                Debug.Log("Dnot contain the bundle ==" + bundleName);
            }
        }

        public void DisposeAllBundle()
        {
            abManager.DisposeAllBundle();
            dicAllAsset.Clear();
        }

        public void DisposeAllBundleAndRes()
        {
            abManager.DisposeAllBundleAndRes();
            dicAllAsset.Clear();
        }

        public void DisposeAllObj()
        {
            abManager.DisposeAllObj();
        }

        public void DebugAllAsset()
        {
            List<string> keys = new List<string>();
            keys.AddRange(dicAllAsset.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                abManager.DebugAssetBundle(keys[i]);
            }
        }

        /// <summary>
        /// 是否加载完成
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public bool IsLoadingFinish(string bundleName)
        {
            if (dicAllAsset.ContainsKey(bundleName))
            {
                return abManager.IsLoadingFinish(dicAllAsset[bundleName]);
            }
            else
            {
                Debug.Log(" is not contain bundle ==" + bundleName);
                return false;
            }

        }

        /// <summary>
        /// 是否已经加载
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public bool IsLoadingAssetBundle(string bundleName)
        {
            if (dicAllAsset.ContainsKey(bundleName))
            {
                return abManager.IsLoadingAssetBundle(dicAllAsset[bundleName]);
            }
            else
            {
                Debug.Log(" is not contain bundle ==" + bundleName);
                return false;
            }
        }


        public string GetBundleReateName(string bundleName)
        {
            if (dicAllAsset.ContainsKey(bundleName))
            {
                return dicAllAsset[bundleName];
            }
            else
            {
                return null;
            }
        }

    }

}