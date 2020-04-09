using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

namespace LYFrame
{
    public class NetWorkToServer
    {
        private Queue<NetMsg> recvMsgPool = null;

        private Queue<NetMsg> sendMsgPool = null;

        private NetSocket clientSocket;

        Thread sendThread;

        public byte[] message = null;

        public NetWorkToServer(string ip, ushort port)
        {
            recvMsgPool = new Queue<NetMsg>();
            sendMsgPool = new Queue<NetMsg>();
            clientSocket = new NetSocket();
            clientSocket.AsyncConnect(ip, port, ConnectCallBack, RecvCallBack);
        }

        private void RecvCallBack(bool success, ErrorSocket error, string exception, byte[] byteMessage, string strMessage)
        {
            if (success)
            {
                PutReceiveMsgToPool(byteMessage);
            }
            else
            {
                //处理错误信息
               // Debug.Log(strMessage);
            }
        }

        private void ConnectCallBack(bool success, ErrorSocket error, string exception)
        {
            if (success)
            {
                sendThread = new Thread(LoopSendMsg);
                sendThread.Start();
            }
            else {
                Debug.Log(exception);
            }
        }



        #region Send

        public void PutSendMsgToPool(NetMsg msg)
        {
            lock (sendMsgPool)
            {
                sendMsgPool.Enqueue(msg);
            }
        }

        private void LoopSendMsg()
        {
            //clientSocket != null &&
            while (clientSocket.IsConnected())
            {
                lock (sendMsgPool)
                {
                    while (sendMsgPool.Count > 0)
                    {
                        NetMsg msg = sendMsgPool.Dequeue();

                        clientSocket.AsynSend(msg.GetNetBytes(), SendCallBack);
                    }
                }

                Thread.Sleep(100);
            }
        }

        private void SendCallBack(bool success, ErrorSocket error, string exception)
        {
            if (success)
            {
                // Debug.Log("发送成功");
            }
            else
            {
                //  Debug.Log("发送失败");
                //如果发送失败 应该返回给队列中
            }
        }

        #endregion


        #region Receive

        public void PutReceiveMsgToPool(byte[] recvMsg)
        {
            //Debug.Log(Encoding.UTF8.GetString(recvMsg));

            //ushort id = BitConverter.ToUInt16(recvMsg, 0);
            //Debug.Log(id);
            //byte[] result = new byte[recvMsg.Length - 2];
            //Buffer.BlockCopy(recvMsg, 2, result, 0, recvMsg.Length-2);
            //Debug.Log(Encoding.UTF8.GetString(result));

            //NetMsgBase tmpNet = new NetMsgBase(id, result);
            //Debug.Log(string.Format("收到消息 msgid{0}  消息：{1}", id, Encoding.UTF8.GetString(tmpNet.buffer)));

            NetModel model = ProtoSerialize.DeSerialize(recvMsg);
            recvMsgPool.Enqueue(new NetMsg(model));
        }

        public void Update()
        {

            if (recvMsgPool != null)
            {
                while (recvMsgPool.Count > 0)
                {
                    NetMsg msg = recvMsgPool.Dequeue();
                    AnalyseData(msg);
                }
            }
        }

        private void AnalyseData(NetMsg msg)
        {
            MsgCenter.Instance.SendToMsg(msg);
        }

        #endregion



        #region Disconnect

        private void CallBackDisconnect(bool success, ErrorSocket error, string exception)
        {
            if (success)
            {
                sendThread.Abort();
            }
            else
            {
                //关闭错误的处理
            }
        }

        public void Disconnect()
        {
            if (clientSocket != null && clientSocket.IsConnected())
            {
                clientSocket.AsyncDisconnect(CallBackDisconnect);
            }
        }

        #endregion
    }

}