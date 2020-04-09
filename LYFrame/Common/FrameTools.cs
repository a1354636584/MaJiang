using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LYFrame
{
    public enum ManagerID
    {
        GameManager = 0,
        UIManager = FrameTools.MsgSpan,
        AudioManager = FrameTools.MsgSpan * 2,
        CharaterManager = FrameTools.MsgSpan * 3,
        NetManager = FrameTools.MsgSpan * 4,
        AssetManager = FrameTools.MsgSpan * 5,
        CameraManager = FrameTools.MsgSpan * 6,
        ModelManager = FrameTools.MsgSpan * 7,
        PartManager = FrameTools.MsgSpan * 8,
    }

    public class FrameTools
    {
        public const int MsgSpan = 3000;
    }

}