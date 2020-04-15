using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LYFrame;
using System;
using UnityEngine.UI;


public class LoginPanel : BaseUIForm
{
    InputField userInput;
    InputField psdInput;
    void Awake()
    {
        msgIds = new ushort[] { (ushort)Enum_UI.LoginSuccess, (ushort)Enum_UI.LoginFaild };
        RegistSelf(this, msgIds);
    }
    // Start is called before the first frame update
    void Start()
    {
        userInput = GetComponentByGameObjectName<InputField>("InputField_UserName");
        psdInput = GetComponentByGameObjectName<InputField>("InputField_Password");
        //RegisterButtonObjectEvent("Button_LoginWX", LoginWX);
        //RegisterButtonObjectEvent("Button_LoginQQ", LoginQQ);
        RegisterButtonObjectEvent("Button_Login", Login);
        RegisterButtonObjectEvent("Button_Exit", Exit);
    }

    private void Exit(GameObject go)
    {
        Application.Quit();
    }

    private void Login(GameObject go)
    {
        SendMsg(new LoginMsg((ushort)Enum_Net.Login, userInput.text, psdInput.text));
    }

    private void LoginQQ(GameObject go)
    {
        Debug.Log("LoginQQ");
    }

    private void LoginWX(GameObject go)
    {
        Debug.Log("LoginWX");
    }

    public override void ProcessEvent(MsgBase msg)
    {
        base.ProcessEvent(msg);
        switch (msg.msgId)
        {
            case (ushort)Enum_UI.LoginSuccess:
                Debug.Log("LoginSuccess");
                break;
            case (ushort)Enum_UI.LoginFaild:
                Debug.Log("LoginFaild");
                break;
            default:
                break;
        }
    }

}
