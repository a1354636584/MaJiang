using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LYFrame
{

    public abstract class MonoBase : MonoBehaviour
    {
        public abstract void ProcessEvent(MsgBase msg);

        #region 封装子类常用的方法
        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="objName">组件名</param>
        /// <returns></returns>
        protected T GetComponentByGameObjectName<T>(string objName) where T : Component
        {
            Transform go = UnityHelper.FindTheChildNode(this.gameObject, objName);
            if (go != null)
                return go.GetComponent<T>();
            else
                return null;
        }
        #endregion
    }

}