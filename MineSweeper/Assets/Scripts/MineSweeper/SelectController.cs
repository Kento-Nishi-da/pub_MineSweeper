using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Constants;

public class SelectController : MonoBehaviour
{
    // 選択が確かかチェックする
    public GameObject CheckPanel;
    public Text CheckText;

    // 各制御に必要
    public GameObject HowToPanel;
    public GameObject ExitPanel;


    //レベルボタンごとにモード変えてGame画面へ
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

    // パネル出して確認
    void CheckPanelFunc(GameLevel lev)
    {
        CheckPanel.SetActive(true);
        CheckText.text = ((int)lev + "x" + (int)lev + "が選択されています\n");
        print(lev);
    }

    // 全処理をボタンで行いたいがための関数
    public void CheckFalseFunc()
    {
        CheckPanel.SetActive(false);
    }

    // シーンロードするだけ
    public void LoadSceneFunc()
    {
        SceneManager.LoadScene("MineSweeper");
    }

    //遊び方ボタンを作る
    public void HowToFunc()
    {
        // 遊び方パネルを出す
        // 操作説明とDrawモードの明記
        HowToPanel.SetActive(true);
        print("HowToPlay");
    }

    // 全処理ボタン(ry
    public void HowToFalseFunc()
    {
        HowToPanel.SetActive(false);
    }

    //終了ボタンを作る
    //Panelで終了チェック
    public void ExitPanelFunc()
    {
        ExitPanel.SetActive(true);
    }

    // 全処理(ry
    public void ExitFalseFunc()
    {
        ExitPanel.SetActive(false);
    }

    // ゲーム終了関数
    public void GameEndFunc()
    {
        // エディターで動かしているとtrue
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
        Application.Quit();//ゲームプレイ終了
#endif
    }

    private void Start()
    {
        CheckFalseFunc();
        HowToFalseFunc();
        ExitFalseFunc();
    }



}
