/***
 * 
 *    
 *            事件触发监听      
 *   
 *            实现对于任何对象的监听处理
 *    
 *   
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LYFrame
{
    public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger
    {
        public delegate void VoidDelegate(GameObject go);
        public delegate void VoidPositionDelegate(GameObject go, Vector2 pointerPos);
        public VoidDelegate onClick;
        public VoidPositionDelegate onRightClick;
        public VoidPositionDelegate onLeftClick;
        public VoidDelegate onDoubleClick;
        public VoidDelegate onDown;
        public VoidPositionDelegate onEnterPos;
        public VoidDelegate onEnter;
        public VoidDelegate onExit;
        public VoidDelegate onUp;
        public VoidDelegate onSelect;
        public VoidDelegate onUpdateSelect;
        public VoidDelegate onDeselect;
        public VoidDelegate onScroll;
        public VoidDelegate onDrag;
        public bool onClickPass;

        /// <summary>
        /// 得到“监听器”组件
        /// </summary>
        /// <param name="go">监听的游戏对象</param>
        /// <returns>
        /// 监听器
        /// </returns>
        public static EventTriggerListener Get(GameObject go)
        {
            EventTriggerListener lister = go.GetComponent<EventTriggerListener>();
            if (lister == null)
            {
                lister = go.AddComponent<EventTriggerListener>();
            }
            return lister;
        }

        public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);
            GameObject current = data.pointerCurrentRaycast.gameObject;
            int count = results.Count;
            for (int i = 0; i < count; i++)
            {
                if (current != results[i].gameObject)
                {
                    ExecuteEvents.Execute(results[i].gameObject, data, function);
                    break;
                }
            }
        }


        public override void OnPointerClick(PointerEventData eventData)
        {
            if (onClickPass)
            {
                PassEvent(eventData, ExecuteEvents.pointerClickHandler);
            }
            if (onClick != null)
            {
                onClick(gameObject);
                //  MsgCenter.Instance.SendToMsg(new MsgBase((ushort)AudioEvent.PlayButtonClick));
            }


            if (eventData.clickCount == 2 && onDoubleClick != null)
            {
                if (onDoubleClick != null)
                {
                    onDoubleClick(gameObject);
                }
            }

            if (eventData.button.ToString() == "Right")
            {
                EventSystem.current.SetSelectedGameObject(gameObject);
                if (onRightClick != null)
                {
                    onRightClick(gameObject, eventData.position);
                }
            }
            if (eventData.button.ToString() == "Left")
            {
               // EventSystem.current.SetSelectedGameObject(gameObject);
                if (onLeftClick != null)
                {
                    onLeftClick(gameObject, eventData.position);
                }
            }
        }


        public override void OnPointerDown(PointerEventData eventData)
        {
            if (onDown != null)
            {
                onDown(gameObject);
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null)
            {
                onEnter(gameObject);
            }

            if (onEnterPos!=null)
            {
                onEnterPos(gameObject, eventData.position);
            }
        }


        public override void OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null)
            {
                onExit(gameObject);
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (onUp != null)
            {
                onUp(gameObject);
            }
        }

        public override void OnSelect(BaseEventData eventBaseData)
        {
            if (onSelect != null)
            {
                onSelect(gameObject);
            }
        }

        public override void OnUpdateSelected(BaseEventData eventBaseData)
        {
            if (onUpdateSelect != null)
            {
                onUpdateSelect(gameObject);
            }
        }

        public override void OnDeselect(BaseEventData eventBaseData)
        {
            if (onDeselect != null)
            {
                onDeselect(gameObject);
            }
        }
        public override void OnScroll(PointerEventData eventData)
        {
            if (onScroll != null)
            {
                onScroll(gameObject);
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (onDrag != null)
            {
                onDrag(gameObject);
            }
        }
    }
}
