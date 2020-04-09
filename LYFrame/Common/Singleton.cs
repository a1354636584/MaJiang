
using UnityEngine;
using System.Collections;

namespace LYFrame
{
    public abstract class Singleton<T> where T : new()
    {
        private static T instance;

        private bool isInit = false;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();

                }

                return instance;
            }
        }

        protected Singleton()
        {
            if (instance != null) throw new System.Exception("This " + (typeof(T)).ToString() + " Singleton Instance is not null !!!");
            SingletonInit();
        }

        /// <summary>
        /// 初始化.
        /// </summary>
        private void SingletonInit()
        {
            if (isInit) return;
            OnSingletonInit();
            isInit = true;
        }

        /// <summary>
        /// Singleton初始化.
        /// </summary>
        public virtual void OnSingletonInit()
        {
            if (isInit) return;
        }

    }
}