using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Constants;

public class SelectController : MonoBehaviour
{
    // �I�����m�����`�F�b�N����
    public GameObject CheckPanel;
    public Text CheckText;

    // �e����ɕK�v
    public GameObject HowToPanel;
    public GameObject ExitPanel;


    //���x���{�^�����ƂɃ��[�h�ς���Game��ʂ�
    public void Lev5Func()
    {
        GameManager.gamelev = GameLevel.LEV5;
        CheckPanelFunc(GameLevel.LEV5);
    }

    public void Lev7Func()
    {
        GameManager.gamelev = GameLevel.LEV7;
        CheckPanelFunc(GameLevel.LEV7);
    }

    public void Lev10Func()
    {
        GameManager.gamelev = GameLevel.LEV10;
        CheckPanelFunc(GameLevel.LEV10);
    }

    // �p�l���o���Ċm�F
    void CheckPanelFunc(GameLevel lev)
    {
        CheckPanel.SetActive(true);
        CheckText.text = ((int)lev + "x" + (int)lev + "���I������Ă��܂�\n");
        print(lev);
    }

    // �S�������{�^���ōs�����������߂̊֐�
    public void CheckFalseFunc()
    {
        CheckPanel.SetActive(false);
    }

    // �V�[�����[�h���邾��
    public void LoadSceneFunc()
    {
        SceneManager.LoadScene("MineSweeper");
    }

    //�V�ѕ��{�^�������
    public void HowToFunc()
    {
        // �V�ѕ��p�l�����o��
        // ���������Draw���[�h�̖��L
        HowToPanel.SetActive(true);
        print("HowToPlay");
    }

    // �S�����{�^��(ry
    public void HowToFalseFunc()
    {
        HowToPanel.SetActive(false);
    }

    //�I���{�^�������
    //Panel�ŏI���`�F�b�N
    public void ExitPanelFunc()
    {
        ExitPanel.SetActive(true);
    }

    // �S����(ry
    public void ExitFalseFunc()
    {
        ExitPanel.SetActive(false);
    }

    // �Q�[���I���֐�
    public void GameEndFunc()
    {
        // �G�f�B�^�[�œ������Ă����true
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
        Application.Quit();//�Q�[���v���C�I��
#endif
    }

    private void Start()
    {
        CheckFalseFunc();
        HowToFalseFunc();
        ExitFalseFunc();
    }



}
