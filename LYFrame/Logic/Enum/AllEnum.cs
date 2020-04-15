using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LYFrame;

public enum Enum_Net
{
    Min = ManagerID.NetManager,
    Login,
    Max = Min + FrameTools.MsgSpan - 1
}

public enum Enum_UI
{
    Min = ManagerID.UIManager,
    LoginSuccess,
    LoginFaild,
    Max = Min + FrameTools.MsgSpan - 1
}
