using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LYFrame
{
    public class IPathTools
    {

        public static string GetPlatformFolderName(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.OSXEditor:

                case RuntimePlatform.OSXPlayer:
                    return "OSX";

                case RuntimePlatform.WindowsPlayer:

                case RuntimePlatform.WindowsEditor:
                    return "Windows";

                case RuntimePlatform.IPhonePlayer:
                    return "IOS";

                case RuntimePlatform.Android:
                    return "Android";

                default:
                    return null;
            }
        }



        public static string GetAppFilePath()
        {
            string tempPath = "";
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            {
                tempPath = Application.streamingAssetsPath;
            }
            else
            {
                tempPath = Application.persistentDataPath;
            }
            return tempPath;
        }

        public static string GetAssetBundlePath()
        {
            string platFolder = GetPlatformFolderName(Application.platform);

            // string allPath = Path.Combine(GetAppFilePath(), platFolder);

            string allPath = GetAppFilePath() + "/" + platFolder;

            return allPath;
        }

        public static string GetWWWAssetBundlePath()
        {
            string tmpStr = "";
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            {
                tmpStr = "file:///" + GetAssetBundlePath();
            }
            else
            {
                string tmpPath = GetAssetBundlePath();
#if UNITY_ANDROID
            tmpStr="jar:file://"+tmpPath;
#elif UNITY_STANDALONE_WIN
                tmpStr = "file:///" + tmpPath;
#else
            tmpStr = "file://" + tmpPath;
#endif

            }

            return tmpStr;
        }

    }

}