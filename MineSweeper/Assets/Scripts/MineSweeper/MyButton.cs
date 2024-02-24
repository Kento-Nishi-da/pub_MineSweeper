using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.Events;
using System;

/// <summary>
/// 自作ボタンクラス.
/// UnityのButtonは左クリックのみなので引用して全クリック対応のボタンを作成
/// <para>今回は左クリックでDig、右クリックでFlag</para>
/// </summary>
public class MyButton : MonoBehaviour, IPointerClickHandler
{
    // 左クリック
    [Serializable]
    public class LeftButtonClickedEvent : UnityEvent { }
    [FormerlySerializedAs("onLeftClick")]
    [SerializeField]
    private LeftButtonClickedEvent m_OnLeftClick = new LeftButtonClickedEvent();
    // 右クリック
    [Serializable]
    public class RightButtonClickedEvent : UnityEvent { }
    [FormerlySerializedAs("onRightClick")]
    [SerializeField]
    private LeftButtonClickedEvent m_OnRightClick = new LeftButtonClickedEvent();


    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                // 左クリック時の処理
                m_OnLeftClick.Invoke();
                break;
            case PointerEventData.InputButton.Right:
                // 右クリック時の処理
                m_OnRightClick.Invoke();
                break;
            case PointerEventData.InputButton.Middle:
                //ホイールクリック時の処理、今回はなし
                break;
        }

    }
}
