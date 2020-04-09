using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LYFrame
{
    public class SocketBuffer
    {
        //消息头
        private byte[] headByte;

        private byte headLength;

        //数据池
        private byte[] allRecvData;

        //当前已经接收到的数据长度
        private int curRecvLength;

        //表示这条数据数据的总长度
        private int allDataLength;

        //2.0
        //接收数据池
        private List<byte> receiveCache;
        private bool isReceiving;

        public SocketBuffer(byte tmpHeadLength, CallBackRecvOver tmpOver)
        {
            headLength = tmpHeadLength;
            headByte = new byte[headLength];
            callBackRecvOver = tmpOver;

            receiveCache = new List<byte>();
        }


        #region 弃用

        public void RecvByte(byte[] recvByte, int realLength)
        {

            if (realLength == 0)
                return;

            //当前已经接收的数据小于头的长度
            if (curRecvLength < headByte.Length)
            {
                RecvHead(recvByte, realLength);
            }
            else
            {
                int tmpLength = curRecvLength + realLength;

                if (tmpLength == allDataLength)//刚好相等
                {
                    RecvOneAll(recvByte, realLength);
                }
                else if (tmpLength > allDataLength)//有多余的数据
                {
                    RecvLarger(recvByte, realLength);
                }
                else//不够 小于
                {
                    RecvSmall(recvByte, realLength);
                }
            }

        }

        private void RecvLarger(byte[] recvByte, int realLength)
        {
            //表示这条数据allDataLength里面还差好长的数据
            int tmpLength = allDataLength - curRecvLength;
            //就拷贝进去
            Buffer.BlockCopy(recvByte, 0, allRecvData, curRecvLength, tmpLength);
            curRecvLength += tmpLength;

            RecvOneMsgOver();

            int remainLength = realLength - tmpLength;
            byte[] remainByte = new byte[remainLength];
            Buffer.BlockCopy(recvByte, tmpLength, remainByte, 0, remainLength);
            //多余的数据继续放入处理
            RecvByte(remainByte, remainLength);
        }

        private void RecvSmall(byte[] recvByte, int realLength)
        {
            Buffer.BlockCopy(recvByte, 0, allRecvData, curRecvLength, realLength);//把剩下的byte拷贝进tmpByte
            curRecvLength += realLength;
        }

        private void RecvOneAll(byte[] recvByte, int realLength)
        {

            Buffer.BlockCopy(recvByte, 0, allRecvData, curRecvLength, realLength);//把剩下的byte拷贝进allRecvData
            curRecvLength += realLength;
            RecvOneMsgOver();
        }

        private void RecvHead(byte[] recvByte, int realLength)
        {
            //还差多少个字节 才能组成一个消息头 
            int tmpReal = headByte.Length - curRecvLength;
            //已经接收的和现在接收的总长度
            int tmpLength = curRecvLength + realLength;

            //总长度还小于头
            if (tmpLength < headByte.Length)
            {
                //表示把recvByte从0开始到realLength结束的字节 拷贝到headByte中，从curRecvLength开始
                Buffer.BlockCopy(recvByte, 0, headByte, curRecvLength, realLength);

                curRecvLength += realLength;
            }
            else//大于等于消息头长度
            {
                Buffer.BlockCopy(recvByte, 0, headByte, curRecvLength, tmpReal);
                curRecvLength += tmpReal;//头部已经凑齐

                //取出四个字节转换成int32（int32占四个字节） 得到数据的长度
                allDataLength = BitConverter.ToInt32(headByte, 0) + headLength;

                //表示一整条数据 head+body
                allRecvData = new byte[allDataLength];

                Buffer.BlockCopy(headByte, 0, allRecvData, 0, headLength);//把head拷贝进allRecvData

                int tmpRemin = realLength - tmpReal;

                if (tmpRemin > 0)//表示recvByte除去消息头还有数据
                {
                    byte[] tmpByte = new byte[tmpRemin];
                    Buffer.BlockCopy(recvByte, tmpReal, tmpByte, 0, tmpRemin);//把剩下的byte拷贝进tmpByte
                    Debug.Log(Encoding.UTF8.GetString(tmpByte, 2, tmpByte.Length - 2));
                    //返回去处理
                    RecvByte(tmpByte, tmpRemin);
                }
                else
                {
                    //表示一个消息接受完了 只有消息头
                    RecvOneMsgOver();
                }
            }
        }
        #region recv over back to

        public delegate void CallBackRecvOver(byte[] allData);

        CallBackRecvOver callBackRecvOver;

        //表示一个消息接受完了 只有消息头
        private void RecvOneMsgOver()
        {
            if (callBackRecvOver != null)
            {
                callBackRecvOver(allRecvData);
            }
            curRecvLength = 0;
            allDataLength = 0;
            allRecvData = null;
        }
        #endregion

        
        #endregion

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="data"></param>
        public void ReceiveByte(byte[] data)
        {
            //将接收到的数据放入数据池中
            receiveCache.AddRange(data);

            //如果没在读数据
            if (!isReceiving)
            {
                isReceiving = true;
                ReadData();
            }
         }

        private void ReadData()
        {
            byte[] data = NetEncode.Decode(ref receiveCache);

            if (data!=null)//说明数据保存成功
            {
                if (callBackRecvOver!=null)
                {
                    callBackRecvOver(data);
                }

                ReadData();
            }
            else
            {
                isReceiving = false;
            }
        }

    }

}