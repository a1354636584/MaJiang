
using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LYFrame
{
    public class UIBehaviour : MonoBase
    {

        public override void ProcessEvent(MsgBase msg)
        {

        }

        private void Start()
        {
            UIManager.Instance.RegistGameObject(name, gameObject);
        }


        public void AddButtonListener(UnityAction action)
        {
            if (action != null)
            {
                Button btn = transform.GetComponent<Button>();
                btn.onClick.AddListener(action);
            }
        }

        public void RemoveButtonListener(UnityAction action)
        {
            if (action != null)
            {
                Button btn = transform.GetComponent<Button>();
                btn.onClick.RemoveListener(action);
            }
        }

        public void AddToggleListener(UnityAction<bool> action)
        {
            if (action != null)
            {
                Toggle btn = transform.GetComponent<Toggle>();
                btn.onValueChanged.AddListener(action);
            }
        }

        public void RemoveToggleListener(UnityAction<bool> action)
        {
            if (action != null)
            {
                Toggle btn = transform.GetComponent<Toggle>();
                btn.onValueChanged.RemoveListener(action);
            }
        }

        public void AddSliderListener(UnityAction<float> action)
        {
            if (action != null)
            {
                Slider btn = transform.GetComponent<Slider>();
                btn.onValueChanged.AddListener(action);
            }
        }

        public void RemoveSliderListener(UnityAction<float> action)
        {
            if (action != null)
            {
                Slider btn = transform.GetComponent<Slider>();
                btn.onValueChanged.RemoveListener(action);
            }
        }

        public void AddScrollbarListener(UnityAction<float> action)
        {
            if (action != null)
            {
                Scrollbar btn = transform.GetComponent<Scrollbar>();
                btn.onValueChanged.AddListener(action);
            }
        }

        public void RemoveScrollbarListener(UnityAction<float> action)
        {
            if (action != null)
            {
                Scrollbar btn = transform.GetComponent<Scrollbar>();
                btn.onValueChanged.RemoveListener(action);
            }
        }

        public void AddDropdownListener(UnityAction<int> action)
        {
            if (action != null)
            {
                Dropdown btn = transform.GetComponent<Dropdown>();
                btn.onValueChanged.AddListener(action);
            }
        }

        public void RemoveDropdownListener(UnityAction<int> action)
        {
            if (action != null)
            {
                Dropdown btn = transform.GetComponent<Dropdown>();
                btn.onValueChanged.RemoveListener(action);
            }
        }

        /// <summary>
        /// onEndEdit
        /// </summary>
        /// <param name="action"></param>
        public void AddInputFieldListener(UnityAction<string> action)
        {
            if (action != null)
            {
                InputField btn = transform.GetComponent<InputField>();
                btn.onEndEdit.AddListener(action);
                //btn.onValueChanged.AddListener(action);
            }
        }
        /// <summary>
        /// onEndEdit
        /// </summary>
        /// <param name="action"></param>
        public void RemoveInputFieldListener(UnityAction<string> action)
        {
            if (action != null)
            {
                InputField btn = transform.GetComponent<InputField>();
                btn.onEndEdit.RemoveListener(action);
                //btn.onValueChanged.RemoveListener(action);
            }
        }



    }
}