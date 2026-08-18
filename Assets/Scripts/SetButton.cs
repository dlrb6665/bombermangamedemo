
using UnityEngine;
using UnityEngine.EventSystems;//事件系统

//                                      监听指针按下的接口   监听指针弹起的接口
public class SetButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public bool Pressed;//按下
    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    //响应此事件的前提是：必须响应过OnPointerDown事件
    {
        Pressed = false;
    }
}

