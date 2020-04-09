using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace LYFrame
{

    public delegate void CallBackNormal(bool success, ErrorSocket error, string exception);
    public delegate void CallBackRecv(bool success, ErrorSocket error, string exception, byte[] byteMessage, string strMessage);

    public enum ErrorSocket
    {
        Success = 0,
        TimeOut,
        SocketNull,
        SocketUnConnect,
        ConnectSuccess,
        ConnectUnSuccessUnKnow,
        ConnectError,
        SendUnSuccessUnKnow,
        SendSuccess,
        RecvUnSuccessUnKnow,
        DiscConnectUnKnow,
        DisconnectSuccess
    }

    public class NetSocket : MonoBehaviour
    {
        private CallBackNormal callBackConnect;
        private CallBackNormal callBackSend;
        private CallBackNormal callBackDisConnect;

        private CallBackRecv callBackRecv;

        private Socket clientSocket;

        private string addressIp;

        private ushort port;

        private ErrorSocket errorSocket;

        private SocketBuffer recvBuffer;


        private byte[] recvBuf;


        public NetSocket()
        {
            recvBuffer = new SocketBuffer(4, RecvMsgOver);
            recvBuf = new byte[2048];
        }

        /// <summary>
        /// 是否处于连接状态
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            if (clientSocket != null && clientSocket.Connected)
                return true;
            else
                return false;
        }

        #region Connect

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="callBackConnect">发起连接的回调</param>
        /// <param name="callBackRecv">接收消息回调</param>
        public void AsyncConnect(string ip, ushort port, CallBackNormal callBackConnect, CallBackRecv callBackRecv)
        {

            errorSocket = ErrorSocket.Success;
            this.callBackConnect = callBackConnect;
            this.callBackRecv = callBackRecv;


            if (clientSocket != null && clientSocket.Connected)
            {
                this.callBackConnect(false, ErrorSocket.ConnectError, "connect repeat");
            }
            else if (clientSocket == null || !clientSocket.Connected)
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPAddress address = IPAddress.Parse(ip);

                IPEndPoint ipep = new IPEndPoint(address, port);

                IAsyncResult ar = clientSocket.BeginConnect(ipep, ConnectCallBack, clientSocket);

                if (!WriteDot(ar))
                {
                    this.callBackConnect(false, errorSocket, "连接超时");
                }
            }
        }

        Thread recThread;

        private void ConnectCallBack(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndConnect(ar);
                if (clientSocket.Connected == false)
                {
                    errorSocket = ErrorSocket.ConnectUnSuccessUnKnow;
                    this.callBackConnect(false, errorSocket, "connect fail");
                    return;
                }
                else
                {
                    //接收消息
                   // Receive();
                    recThread = new Thread(Receive);
                    recThread.Start();
                    errorSocket = ErrorSocket.ConnectSuccess;
                    this.callBackConnect(true, errorSocket, "connect success");
                    return;
                }
            }
            catch (Exception e)
            {
                this.callBackConnect(false, errorSocket, e.ToString());
            }
        }


        #endregion



        #region Receive

        public void Receive()
        {
            while (true)
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    IAsyncResult asyRecv = clientSocket.BeginReceive(recvBuf, 0, recvBuf.Length, SocketFlags.None, ReceiveCallBack, clientSocket);


                    if (!WriteDot(asyRecv))
                    {
                        this.callBackRecv(false, ErrorSocket.RecvUnSuccessUnKnow, "receive failed", null, "Receive");
                    }
                }
            }
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                if (clientSocket.Connected == false)
                {
                    errorSocket = ErrorSocket.RecvUnSuccessUnKnow;
                    this.callBackRecv(false, errorSocket, "connect fail", null, "ReceiveCallBack");
                    return;
                }

                int length = clientSocket.EndReceive(ar);
               
                if (length == 0)
                    return;

                Debug.Log(string.Format("接收到数据 数据长度 {0}，数据真实长度 {1}", length, BitConverter.ToInt32(recvBuf, 0)));

                byte[] tmp = new byte[length];
                Buffer.BlockCopy(recvBuf, 0, tmp, 0, length);
                //recvBuffer.RecvByte(tmp, length);
                recvBuffer.ReceiveByte(tmp);
            }
            catch (Exception e)
            {
                this.callBackRecv(false, ErrorSocket.RecvUnSuccessUnKnow, e.ToString(), null, "e");
            }

          // gai  Receive();
        }

        #endregion



        #region RecvMsgOver
        private void RecvMsgOver(byte[] allData)
        {
            this.callBackRecv(true, ErrorSocket.Success, "", allData, "receive back success");
        }
        #endregion



        #region Send


        public void AsynSend(List<byte[]> sendBufferList, CallBackNormal tmpSendBack)
        {
            errorSocket = ErrorSocket.Success;
            this.callBackSend = tmpSendBack;

            if (clientSocket == null)
            {
                this.callBackSend(false, ErrorSocket.SocketNull, "");
            }
            else if (!clientSocket.Connected)
            {
                this.callBackSend(false, ErrorSocket.SendUnSuccessUnKnow, "");
            }
            else
            {
                for (int i = 0; i < sendBufferList.Count; i++)
                {
                    byte[] sendBuffer = sendBufferList[i];
                    IAsyncResult asySend = clientSocket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, SendCallBack, clientSocket);
                    if (!WriteDot(asySend))
                    {
                        this.callBackSend(false, ErrorSocket.SendUnSuccessUnKnow, "send failed");
                    }
                }
            }
        }

        private void SendCallBack(IAsyncResult ar)
        {
            try
            {
                if (clientSocket.Connected == false)
                {
                    errorSocket = ErrorSocket.SendUnSuccessUnKnow;
                    this.callBackSend(false, errorSocket, "send failed");
                    return;
                }

                int length = clientSocket.EndSend(ar);
                if (length > 0)
                {
                    this.callBackSend(true, ErrorSocket.SendSuccess, "send success");
                }
                else
                {
                    this.callBackSend(false, ErrorSocket.SendUnSuccessUnKnow, "send failed");
                }

            }
            catch (Exception e)
            {
                this.callBackSend(false, ErrorSocket.SendUnSuccessUnKnow, e.ToString());
            }

        }

        #endregion



        #region Disconnect

        public void AsyncDisconnect(CallBackNormal tmpDisconnectBack)
        {

            try
            {
                errorSocket = ErrorSocket.Success;

                this.callBackDisConnect = tmpDisconnectBack;

                if (clientSocket == null)
                {
                    errorSocket = ErrorSocket.DiscConnectUnKnow;
                    this.callBackDisConnect(false, errorSocket, "client is null");
                }
                else if (clientSocket.Connected == false)
                {
                    errorSocket = ErrorSocket.DiscConnectUnKnow;
                    this.callBackDisConnect(false, errorSocket, "client is null");
                }
                else
                {
                    IAsyncResult asySend = clientSocket.BeginDisconnect(false, DisconnectCallBack, clientSocket);

                    if (!WriteDot(asySend))
                    {
                        this.callBackDisConnect(false, ErrorSocket.DiscConnectUnKnow, "超时");
                    }
                }

            }
            catch (Exception e)
            {
                this.callBackDisConnect(false, ErrorSocket.DiscConnectUnKnow, e.ToString());
            }
        }

        private void DisconnectCallBack(IAsyncResult ar)
        {
            try
            {
                if (clientSocket.Connected == false)
                {
                    errorSocket = ErrorSocket.DiscConnectUnKnow;
                    this.callBackSend(false, errorSocket, "");
                    return;
                }

                clientSocket.EndDisconnect(ar);

                clientSocket.Close();
                clientSocket = null;

                this.callBackDisConnect(true, ErrorSocket.DisconnectSuccess, "");

            }
            catch (Exception e)
            {
                this.callBackDisConnect(false, ErrorSocket.DiscConnectUnKnow, e.ToString());
            }

        }

        #endregion



        #region TimeOut chech

        private bool WriteDot(IAsyncResult ar)
        {
            int i = 0;
            while (ar.IsCompleted == false)
            {
                i++;
                if (i > 20)
                {
                    errorSocket = ErrorSocket.TimeOut;
                    return false;
                }
                Thread.Sleep(100);
            }
            return true;
        }

        #endregion


    }


}

