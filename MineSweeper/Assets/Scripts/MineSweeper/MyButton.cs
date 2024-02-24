using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.Events;
using System;

/// <summary>
/// ����{�^���N���X.
/// Unity��Button�͍��N���b�N�݂̂Ȃ̂ň��p���đS�N���b�N�Ή��̃{�^�����쐬
/// <para>����͍��N���b�N��Dig�A�E�N���b�N��Flag</para>
/// </summary>
public class MyButton : MonoBehaviour, IPointerClickHandler
{
    // ���N���b�N
    [Serializable]
    public class LeftButtonClickedEvent : UnityEvent { }
    [FormerlySerializedAs("onLeftClick")]
    [SerializeField]
    private LeftButtonClickedEvent m_OnLeftClick = new LeftButtonClickedEvent();
    // �E�N���b�N
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
                // ���N���b�N���̏���
                m_OnLeftClick.Invoke();
                break;
            case PointerEventData.InputButton.Right:
                // �E�N���b�N���̏���
                m_OnRightClick.Invoke();
                break;
            case PointerEventData.InputButton.Middle:
                //�z�C�[���N���b�N���̏����A����͂Ȃ�
                break;
        }

    }
}
