/***
 *       UI窗体的父类
 *    
 *           功能：定义所有UI窗体的父类。
 *           定义四个生命周期
 *           
 *           1：Display 显示状态。
 *           2：Hiding 隐藏状态
 *           3：ReDisplay 再显示状态。
 *           4：Freeze 冻结状态。
 *           
 *   
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

namespace LYFrame
{
    public class BaseUIForm : UIBase
    {
        /*字段*/
        private UIType _CurrentUIType = new UIType();

        /* 属性*/
        //当前UI窗体类型
        public UIType CurrentUIType
        {
            get { return _CurrentUIType; }
            set { _CurrentUIType = value; }
        }


        public object[] para = null;

        public override void ProcessEvent(MsgBase msg)
        {

        }



        #region  窗体的四种(生命周期)状态

        /// <summary>
        /// 重新显示状态
        /// </summary>
        public virtual void Display()
        {
            this.gameObject.SetActive(true);
            //设置模态窗体调用(必须是弹出窗体)
            if (_CurrentUIType.UIForms_Type == UIFormType.PopUp)
            {
                UIMaskMgr.GetInstance().SetMaskWindow(this.gameObject, _CurrentUIType.UIForm_LucencyType);
            }
        }

        /// <summary>
        /// 隐藏状态
        /// </summary>
        public virtual void Hiding()
        {
            this.gameObject.SetActive(false);
            //取消模态窗体调用
            if (_CurrentUIType.UIForms_Type == UIFormType.PopUp)
            {
                UIMaskMgr.GetInstance().CancelMaskWindow(_CurrentUIType.UIForm_LucencyType);
            }
            InitPanel();
        }
        #region 当前项目需求更改
        protected virtual void InitPanel()
        {

        }

        #endregion
        /// <summary>
        /// 显示状态
        /// </summary>
        public virtual void Redisplay()
        {

            this.gameObject.SetActive(true);
            //设置模态窗体调用(必须是弹出窗体)
            if (_CurrentUIType.UIForms_Type == UIFormType.PopUp)
            {
                UIMaskMgr.GetInstance().SetMaskWindow(this.gameObject, _CurrentUIType.UIForm_LucencyType);
            }
        }

        /// <summary>
        /// 冻结状态
        /// </summary>
        public virtual void Freeze()
        {
            this.gameObject.SetActive(true);
        }


        #endregion

        #region 封装子类常用的方法

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="objName">组件名</param>
        /// <returns></returns>
        protected T GetComponentByGameObjectName<T>(string objName) where T : Component
        {
            GameObject go = UnityHelper.FindTheChildNode(this.gameObject, objName).gameObject;
            if (go != null)
                return go.GetComponent<T>();
            else
                return null;
        }

        /// <summary>
        /// 注册按钮事件
        /// </summary>
        /// <param name="buttonName">按钮节点名称</param>
        /// <param name="delHandle">委托：需要注册的方法</param>
        protected void RegisterButtonObjectEvent(string buttonName, EventTriggerListener.VoidDelegate delHandle)
        {
            GameObject goButton = UnityHelper.FindTheChildNode(this.gameObject, buttonName).gameObject;
            //给按钮注册事件方法
            if (goButton != null)
            {
                EventTriggerListener.Get(goButton).onClick = delHandle;
            }
        }

        /// <summary>
        /// 注册按钮双击事件
        /// </summary>
        /// <param name="buttonName">按钮节点名称</param>
        /// <param name="delHandle">委托：需要注册的方法</param>
        protected void RegisterButtonObjectDoubleClickEvent(string buttonName, EventTriggerListener.VoidDelegate delHandle)
        {
            GameObject goButton = UnityHelper.FindTheChildNode(this.gameObject, buttonName).gameObject;
            //给按钮注册事件方法
            if (goButton != null)
            {
                EventTriggerListener.Get(goButton).onDoubleClick = delHandle;
            }
        }

        /// <summary>
        /// 打开UI窗体
        /// </summary>
        /// <param name="uiFormName"></param>
        protected void OpenUIForm(string uiFormName)
        {
            UIManager.Instance.ShowUIForms(uiFormName);
        }

        /// <summary>
        /// 关闭当前UI窗体
        /// </summary>
        protected void CloseUIForm()
        {
            string strUIFromName = string.Empty;            //处理后的UIFrom 名称
            int intPosition = -1;

            strUIFromName = GetType().ToString();             //命名空间+类名
            intPosition = strUIFromName.IndexOf('.');
            if (intPosition != -1)
            {
                //剪切字符串中“.”之间的部分
                strUIFromName = strUIFromName.Substring(intPosition + 1);
            }
            UIManager.Instance.CloseUIForms(strUIFromName);
        }

        /// <summary>
        /// 切换UI窗体
        /// </summary>
        /// <param name="uiFormName"></param>
        protected void AlterForm(string uiFormName)
        {
            OpenUIForm(uiFormName);
            CloseUIForm();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgType">消息的类型</param>
        /// <param name="msgName">消息名称</param>
        /// <param name="msgContent">消息内容</param>
        protected void SendMessage(string msgType, string msgName, object msgContent)
        {
            KeyValuesUpdate kvs = new KeyValuesUpdate(msgName, msgContent);
            MessageCenter.SendMessage(msgType, kvs);
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="messagType">消息分类</param>
        /// <param name="handler">消息委托</param>
        public void ReceiveMessage(string messagType, MessageCenter.DelMessageDelivery handler)
        {
            MessageCenter.AddMsgListener(messagType, handler);
        }

        /// <summary>
        /// 显示语言
        /// </summary>
        /// <param name="id"></param>
        public string Show(string id)
        {
            string strResult = string.Empty;

            strResult = LauguageMgr.GetInstance().ShowText(id);

            return strResult;
        }

        #endregion

    }
}