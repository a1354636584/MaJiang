// ========================================================
// Module：
// Author：lihangyu
// CreateTime：2018/12/28 14:05:06
// ========================================================
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class ListEx {
    /// <summary>
    /// List克隆
    /// </summary>
    /// <typeparam name="T">需序列化</typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<T> Clone<T>(this List<T> list) where T : class
    {
        MemoryStream stream = new MemoryStream();
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, list);
        byte[] data = stream.ToArray();
        stream.Close();

        stream = new MemoryStream();
        stream.Write(data, 0, data.Length);
        stream.Position = 0;
        formatter = new BinaryFormatter();
        object obj = formatter.Deserialize(stream);
        stream.Close();

        return obj as List<T>;
    }
}
