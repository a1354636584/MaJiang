using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LYFrame;

public class LoginMsg : MsgBase
{
    public string username;
    public string password;

    public LoginMsg(ushort msgid, string username, string password) : base(msgid)
    {
        this.username = username;
        this.password = password;
    }
}

public class LoginSuccessMsg : MsgBase
{
    public string username;

    public LoginSuccessMsg(ushort msgid, string username) : base(msgid)
    {
        this.username = username;
    }
}
