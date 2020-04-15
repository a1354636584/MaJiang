using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LYFrame;
using Photon.Realtime;
using PhotonMessage;
using System.Linq;

public class MyClient : NetBase, IPhotonPeerListener
{
    private string address;
    private string Server;
    private PhotonPeer peer;

    private ClientState state;

    public void DebugReturn(DebugLevel level, string message)
    {

    }

    public void OnEvent(EventData eventData)
    {

    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        switch (operationResponse.ReturnCode)
        {
            case (byte)OpCodeEnum.LoginSuccess:
                state = ClientState.ConnectedToNameServer;
                SendMsg(new LoginSuccessMsg((ushort)Enum_UI.LoginSuccess, (string)operationResponse.Parameters[(byte)OpKeyEnum.UserName]));
                break;
            case (byte)OpCodeEnum.LoginFailed:
                state = ClientState.Disconnected;
                SendMsg(new MsgBase((ushort)Enum_UI.LoginFaild));
                break;
            default:
                break;
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        switch (statusCode)
        {
            case StatusCode.Connect:
                Debug.Log("Connect");
                break;
            case StatusCode.Disconnect:
                Debug.Log("Disconnect");
                break;
            default:
                break;
        }
    }

    private void Awake()
    {
        address = "127.0.0.1:5055";
        Server = "MyGameServer";
        peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        peer.Connect(address, Server);
    }

    private void Start()
    {
        msgIds = new ushort[] { (ushort)Enum_Net.Login };
        RegistSelf(this, msgIds);
    }


    private void Update()
    {
        peer.Service();
    }

    public override void ProcessEvent(MsgBase msg)
    {
        base.ProcessEvent(msg);
        switch (msg.msgId)
        {
            case (ushort)Enum_Net.Login:
                SendLogin(msg);
                break;
            default:
                break;
        }
    }


    private void SendLogin(MsgBase msg)
    {
        LoginMsg tmpMsg = (LoginMsg)msg;
        peer.SendOperation((byte)OpCodeEnum.Login, new Dictionary<byte, object> { { (byte)OpKeyEnum.UserName, tmpMsg.username }, { (byte)OpKeyEnum.PassWord, tmpMsg.password } }, new SendOptions());
    }
}
