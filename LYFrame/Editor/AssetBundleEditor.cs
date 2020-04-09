using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LYFrame
{
    public class AssetBundleEditor
    {
        [MenuItem("Tools/BuildAssetBundle")]
        public static void BuildAssetBundle()
        {
            //string outPath = Application.streamingAssetsPath + "/AssetBundle";
            string outPath = IPathTools.GetAssetBundlePath();
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }
            BuildPipeline.BuildAssetBundles(outPath, 0, EditorUserBuildSettings.activeBuildTarget);

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/MarkAssetBundle")]
        public static void MarkAssetBundle()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();

            string path = Application.dataPath + "/Art/Scenes/";


            DirectoryInfo dir = new DirectoryInfo(path);
            // dir.Create();

            FileSystemInfo[] fileInfo = dir.GetFileSystemInfos();
            for (int i = 0; i < fileInfo.Length; i++)
            {
                FileSystemInfo tmpFile = fileInfo[i];
                if (tmpFile is DirectoryInfo)//操作场景文件夹
                {
                    string tmpPath = Path.Combine(path, tmpFile.Name);
                    Debug.Log(tmpPath);
                    SceneOverView(tmpPath);
                }
                //else {
                //    Debug.Log(tmpFile);
                //}
            }

            string outPath = IPathTools.GetAssetBundlePath();
            CopyRecord(path, outPath);


            AssetDatabase.Refresh();

        }

        private static void CopyRecord(string path, string outPath)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                Debug.Log("is not exists");
                return;
            }

            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }

            FileSystemInfo[] files = dir.GetFileSystemInfos();

            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i] as FileInfo;


                if (file != null && file.Extension == ".txt")
                {
                    string sourFile = path + file.Name;

                    string disFile = outPath + "/" + file.Name;

                    Debug.Log("sourFile ==" + sourFile);
                    Debug.Log("disFile ==" + disFile);

                    File.Copy(sourFile, disFile, true);
                }
            }

            AssetDatabase.Refresh();
        }

        //对所有场景遍历
        private static void SceneOverView(string scenePath)
        {
            string textFileName = "Record.txt";
            string tmpPath = scenePath + textFileName;
            Debug.Log(tmpPath);
            FileStream fs = new FileStream(tmpPath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);

            //存储对应关系
            Dictionary<string, string> readDic = new Dictionary<string, string>();

            ChangerHead(scenePath, readDic);

            foreach (string key in readDic.Keys)
            {
                //sw.Write(key);
                //sw.Write(" ");
                //sw.Write(readDic[key]);
                // sw.Write("\n");

                sw.WriteLine(key + " " + readDic[key]);
            }



            sw.Close();
            fs.Close();

        }

        //遍历场景中的功能文件夹
        private static void ChangerHead(string fullPath, Dictionary<string, string> dicWrite)
        {
            int tmpCount = fullPath.IndexOf("Assets");

            string replacePath = fullPath.Substring(tmpCount, fullPath.Length - tmpCount);

            DirectoryInfo dirInfo = new DirectoryInfo(fullPath);
            if (dirInfo != null)
            {
                Debug.Log("replacePath= " + replacePath);

                ListFiles(dirInfo, replacePath, dicWrite);
            }
            else
            {
                Debug.LogError("this path is not exist.");
            }

        }

        private static void ListFiles(FileSystemInfo info, string replacePath, Dictionary<string, string> dicWrite)
        {
            if (!info.Exists)
            {
                Debug.LogError("is not exist");
                return;
            }
            DirectoryInfo tmpInfo = info as DirectoryInfo;
            FileSystemInfo[] files = tmpInfo.GetFileSystemInfos();

            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i] as FileInfo;

                if (file != null)//对文件的操作
                {
                    ChangerMark(file, replacePath, dicWrite);
                }
                else//对目录的操作
                {
                    ListFiles(files[i], replacePath, dicWrite);
                }
            }
        }

        private static void ChangeAssetMark(FileInfo file, string markStr, Dictionary<string, string> dicWrite)
        {
            string fulPath = file.FullName;
            int assetCount = fulPath.IndexOf("Assets");

            //Assets/Scenes/SceneOne/Prefab/Cube.prefab  
            AssetImporter importer = AssetImporter.GetAtPath(fulPath.Substring(assetCount, fulPath.Length - assetCount));
            importer.assetBundleName = markStr;
            //改变后缀
            if (file.Extension == ".unity")
            {
                importer.assetBundleVariant = "u3d";
            }
            else
            {
                importer.assetBundleVariant = "ld";
            }


            string modleName = "";
            string[] array = markStr.Split('/');
            if (array.Length > 1)
            {
                modleName = array[1];
            }
            else
            {
                modleName = markStr;
            }

            string modlePath = markStr.ToLower() + "." + importer.assetBundleVariant;

            if (!dicWrite.ContainsKey(modleName))
            {
                dicWrite.Add(modleName, modlePath);
            }


        }

        private static void ChangerMark(FileInfo tmpInfo, string repalcePath, Dictionary<string, string> dicWrite)
        {
            if (tmpInfo.Extension == ".meta")
            {
                return;
            }

            string markStr = GetBundlePath(tmpInfo, repalcePath);

            Debug.Log("markStr= " + markStr);

            ChangeAssetMark(tmpInfo, markStr, dicWrite);

        }

        private static string GetBundlePath(FileInfo file, string replacePath)
        {
            string tmpPath = file.FullName;
            //E:\Job\TestProjects\SuperFrame\Assets\Art\Scenes\SceneOne\Prefab\Cube.prefab
            Debug.Log("tmpPath= " + tmpPath);

            //replacePath= Assets/Art/Scenes/SceneOne
            Debug.Log("replacePath= " + replacePath);

            tmpPath = tmpPath.Replace("\\".ToCharArray()[0], '/');

            Debug.Log("tmpPath= " + tmpPath.Replace("\\", "/"));

            int assetCount = tmpPath.IndexOf(replacePath);

            Debug.Log("assetCount=" + assetCount);

            assetCount += replacePath.Length + 1;

            //file name= Cube.prefab
            Debug.Log("file name= " + file.Name);


            int nameCount = tmpPath.LastIndexOf(file.Name);


            int tmpCount = replacePath.LastIndexOf("/");

            string sceneHead = replacePath.Substring(tmpCount + 1, replacePath.Length - tmpCount - 1);

            Debug.Log("sceneHead= " + sceneHead);

            int tmpLength = nameCount - assetCount;


            //replacePath= Assets/Art/Scenes/SceneOne
            //tmpPath = E:\Job\TestProjects\SuperFrame\Assets\Art\Scenes\SceneOne\ Prefab\ Cube.prefab

            if (tmpLength > 0)
            {
                string subStr = tmpPath.Substring(assetCount, tmpPath.Length - assetCount);
                Debug.Log("subStr= " + subStr);
                string[] result = subStr.Split("/".ToCharArray());
                Debug.Log("result= " + result[0]);
                return sceneHead + "/" + result[0];
            }
            else
            {
                return sceneHead;//场景文件标记
            }

        }
    }

}