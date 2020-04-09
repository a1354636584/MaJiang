// ========================================================
// Module：
// Author：lihangyu
// CreateTime：2018/12/28 14:06:00
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayEx {
    /// <summary>
    /// 数组转List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static List<T> ToList<T>(this T[] array)
    {
        if (array != null || array.Length > 0)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < array.Length; i++)
            {
                result.Add(array[i]);
            }
            return result;
        }
        return null;
    }
}
