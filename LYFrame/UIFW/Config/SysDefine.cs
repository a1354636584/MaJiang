/***
 *           1： 系统常量
 *           2： 全局性方法。
 *           3： 系统枚举类型
 *           4： 委托定义 
 *   
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LYFrame
{
    #region 系统枚举类型

    /// <summary>
    /// UI窗体（位置）类型
    /// </summary>
    public enum UIFormType
    {
        //普通窗体
        Normal,   
        //固定窗体                              
        Fixed,
        //弹出窗体
        PopUp
    }

    /// <summary>
    /// UI窗体的显示类型
    /// </summary>
    public enum UIFormShowMode
    {
        //普通
        Normal,
        //反向切换
        ReverseChange,
        //隐藏其他
        HideOther
    }

    /// <summary>
    /// UI窗体透明度类型
    /// </summary>
    public enum UIFormLucenyType
    {
        //完全透明，不能穿透
        Lucency,
        //半透明，不能穿透
        Translucence,
        //低透明度，不能穿透
        ImPenetrable,
        //可以穿透
        Pentrate    
    }

    #endregion

    public class SysDefine : MonoBehaviour {
        /* 路径常量 */
        public const string SYS_PATH_CANVAS = "Prefab/UI/Canvas";
        public const string SYS_PATH_UIFORMS_CONFIG_INFO = "Config/UIFormsConfigInfo";
        public const string SYS_PATH_CONFIG_INFO = "Config/SysConfigInfo";

        /* 标签常量 */
        public const string SYS_TAG_CANVAS = "_TagCanvas";
        public const string SYS_NAME_DEFAULTCANVAS = "Default";
        public const string SYS_NAME_CANVAS = "Canvas(Clone)";
        public const string SYS_NAME_MONCANVAS = "MonCanvas";
        /* 节点常量 */
        public const string SYS_NORMAL_NODE = "Normal";
        public const string SYS_FIXED_NODE = "Fixed";
        public const string SYS_POPUP_NODE = "PopUp";
        public const string SYS_SCRIPTMANAGER_NODE = "_ScriptMgr";
        /* 遮罩管理器中，透明度常量 */
        public const float SYS_UIMASK_LUCENCY_COLOR_RGB = 255 / 255F;
        public const float SYS_UIMASK_LUCENCY_COLOR_RGB_A = 0F / 255F;

        public const float SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB = 220 / 255F;
        public const float SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB_A = 50F / 255F;

        public const float SYS_UIMASK_IMPENETRABLE_COLOR_RGB = 50 / 255F;
        public const float SYS_UIMASK_IMPENETRABLE_COLOR_RGB_A = 200F / 255F;

        /* 摄像机层深的常量 */

        /* 全局性的方法 */
        
        /* 委托的定义 */

    }

    public struct MyCanvas
    {
        //缓存所有UI窗体
        public Dictionary<string, BaseUIForm> _DicALLUIForms;
        //当前显示的UI窗体
        public Dictionary<string, BaseUIForm> _DicCurrentShowUIForms;
        //定义“栈”集合,存储显示当前所有[反向切换]的窗体类型
        public Stack<BaseUIForm> _StaCurrentUIForms;
        //UI根节点
        public Transform _TraCanvasTransfrom;
        //全屏幕显示的节点
        public Transform _TraNormal;
        //固定显示的节点
        public Transform _TraFixed;
        //弹出节点
        public Transform _TraPopUp;
        //UI管理脚本的节点
        public Transform _TraUIScripts;

        public MyCanvas(Transform canvasTF)
        {
            _DicALLUIForms = new Dictionary<string, BaseUIForm>();
            _DicCurrentShowUIForms = new Dictionary<string, BaseUIForm>();
            _StaCurrentUIForms = new Stack<BaseUIForm>();
            _TraCanvasTransfrom = canvasTF;
            _TraNormal = UnityHelper.FindTheChildNode(_TraCanvasTransfrom.gameObject, SysDefine.SYS_NORMAL_NODE);
            _TraFixed = UnityHelper.FindTheChildNode(_TraCanvasTransfrom.gameObject, SysDefine.SYS_FIXED_NODE);
            _TraPopUp = UnityHelper.FindTheChildNode(_TraCanvasTransfrom.gameObject, SysDefine.SYS_POPUP_NODE);
            _TraUIScripts = UnityHelper.FindTheChildNode(_TraCanvasTransfrom.gameObject, SysDefine.SYS_SCRIPTMANAGER_NODE);
        }
    }
}