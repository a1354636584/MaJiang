using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;


namespace LYFrame
{
    /// <summary>
    /// 编码和解码
    /// </summary>
    public class NetEncode
    {
        //消息头长度
        private static int headLength = 4;
        //每条消息最大长度
        private static int size = 1024;


        /// <summary>
        /// 将数据编码 ptotobuff
        /// </summary>
        /// <param name="data">内容</param>
        public static List<byte[]> Encode(byte[] data)
        {
            if (data == null || data.Length <= 0)
                return null;

            List<byte[]> resList = new List<byte[]>();

            //使用流将编码写二进制
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            br.Write(data.Length);//数据长度 headlength
            br.Write(data);//数据body
            //将流中的内容复制到数组中

            byte[] array = ms.ToArray();
            int num = array.Length / size + 1;
            if (num == 1)// <1024
            {
                resList.Add(array);
            }
            else  // >1024
            {
                for (int i = 1; i < num + 1; i++)
                {
                    byte[] tmp = new byte[size];
                    if (i == num)
                    {
                        int count = array.Length - size * (num - 1);
                        tmp = new byte[count];
                        System.Buffer.BlockCopy(array, size * (i - 1), tmp, 0, count);
                    }
                    else
                    {
                        System.Buffer.BlockCopy(array, size * (i - 1), tmp, 0, size);
                    }
                    resList.Add(tmp);
                }
            }

            return resList;
        }

        /// <summary>
        /// 将数据解码
        /// </summary>
        /// <param name="cache">消息队列</param>
        public static byte[] Decode(ref List<byte> cache)
        {
            //首先要获取长度，整形4个字节，如果字节数不足4个字节
            if (cache.Count < headLength)
            {
                return null;
            }
            byte[] array = cache.ToArray();
            int dataLength = BitConverter.ToInt32(array, 0);

            if (dataLength > array.Length - headLength)
            {
                return null;
            }

            byte[] resData = new byte[dataLength];
            Buffer.BlockCopy(array, headLength, resData, 0, dataLength);
            cache.RemoveRange(0, dataLength + headLength);
            return resData;

            //读取数据
            //MemoryStream ms = new MemoryStream(cache.ToArray());
            //BinaryReader br = new BinaryReader(ms);
            //int len = br.ReadInt32();

            ////根据长度，判断内容是否传递完毕
            //if (len > ms.Length - ms.Position)
            //{
            //    return null;
            //}
            ////获取数据
            //byte[] result = br.ReadBytes(len);
            ////清空消息池
            //cache.Clear();
            ////讲剩余没处理的消息存入消息池
            //cache.AddRange(br.ReadBytes((int)ms.Length - (int)ms.Position));
            //return result;
        }
    }
}