// ========================================================
// Module：
// Author：lihangyu
// CreateTime：2018/12/28 14:08:23
// ========================================================
using UnityEngine;

public static class GameObjectEx
{
    /// <summary>
    /// 获取组件，没有则添加
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this GameObject go) where T : MonoBehaviour
    {
        T t = go.GetComponent<T>();
        return t ? t : go.AddComponent<T>();
    }
}
