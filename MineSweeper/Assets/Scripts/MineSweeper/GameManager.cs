using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Constants;

public class GameManager : MonoBehaviour
{
    // 仮の難易度の初期化、ステージセレクトから変更予定
    public static GameLevel gamelev = GameLevel.LEV10;
    // intでも宣言
    int lev;
    // ゲームの状態
    public static GameMode mode = GameMode.GAMEPLAY;

    // prefab
    [SerializeField]
    GameObject tileprefabObj;
    // classとGameObjectの変換がめんどくさいので両方宣言
    [SerializeField]
    Tile tileprefabtile;
    // タイル格納用
    [SerializeField]
    GameObject TilesRoot;
    // 失敗成功それぞれの制御用
    [SerializeField]
    GameObject GameOverImage;
    [SerializeField]
    GameObject GameClearImage;

    // メニューパネル
    [SerializeField] private GameObject PosePanel;
    // ヘルプパネル
    [SerializeField] private GameObject HelpPanel;
    // ペイントを制御
    [SerializeField] PaintController paintController;
    // 現在のモードをわかりやすく
    [SerializeField] private Image ModeModule;
    [SerializeField] private RawImage RawImage;
    [SerializeField] private GameObject a;


    /// <summary>
    /// 開けた枚数を格納
    /// </summary>
    int DiggedTileCnt;


    // タイル二次配列宣言
    Tile[,] TilesArray = new Tile[Const.MAX_LEN, Const.MAX_LEN];


    BombLevel TotalBombsCnt;
    OpenedTile openedTile;

    private void Start()
    {
        // gamelevの応じて爆弾の個数を変更
        SetGameLevFunc();
        // intへ変換
        lev = (int)gamelev;
        DiggedTileCnt = 0;
        SetTilesFunc();
        SetBombsFunc();

        ModeModule.color = Color.white;

        // 初期化
        GameOverImage.SetActive(false);
        GameClearImage.SetActive(false);
        PosePanel.SetActive(false);
        HelpPanel.SetActive(false);
    }


    /// <summary>
    /// タイルの生成/設置
    /// </summary>
    void SetTilesFunc()
    {
        // タイルの一個目の位置を計算
        Vector2 ScreenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        //print(ScreenCenter);
        float tmp = ((lev - 1) / 2) * Const.TILE_SIZE;
        Vector2 LeftOverTilePos = ScreenCenter + new Vector2(-tmp, tmp);

        // タイルの設置

        for (int i = 0; i < Const.MAX_LEN; i++)
        {
            for (int j = 0; j < Const.MAX_LEN; j++)
            {
                Vector2 thisTilePos;
                if (i <= lev && i >= 1 && j <= lev && j >= 1)
                {
                    thisTilePos = (
                    LeftOverTilePos + new Vector2((j - 2), -(i - 2)) * Const.TILE_SIZE
                    );
                }
                else
                {
                    thisTilePos = Vector2.zero;
                }

                // タイル生成
                Tile tile = Instantiate(tileprefabtile, TilesRoot.transform);
                tile.GetComponent<RectTransform>().position = thisTilePos;
                // 識別番号割り当て
                if (i <= lev && i >= 1 && j <= lev && j >= 1)
                {
                    tile.Init(new Vector2Int(i, j));
                }
                TilesArray[i, j] = tile;

                // 配列外を探索しないように0-MAX_LENで生成するが使うのは1-levなので非表示
                TilesArray[i, j].gameObject.SetActive(false);
                if (i <= lev && i >= 1 && j <= lev && j >= 1)
                {
                    TilesArray[i, j].gameObject.SetActive(true);
                }
            }
        }
    }


    /// <summary>
    /// 爆弾の割り当て
    /// </summary>
    void SetBombsFunc()
    {
        // Vector2Int型のリストを作り、TilesArrayに対応するようにAdd
        List<Vector2Int> ArrayIndex = new List<Vector2Int>(lev * lev);
        for (int i = 1; i <= lev; i++)
        {
            for (int j = 1; j <= lev; j++)
            {
                ArrayIndex.Add(new Vector2Int(i, j));
                //print("i:" + i + "\nj:" + j);
            }
        }

        // 入れたい爆弾分ループ
        // 作ったリストからランダムに選出、リストから除外
        for (int i = 0; i < (int)TotalBombsCnt; i++)
        {

            // まず必要な要素数の中で乱数をとる
            int BombListIndex = Random.Range(0, ArrayIndex.Count);

            //print("BombListIndex:" + BombListIndex);
            //print(ArrayIndex[BombListIndex]);

            // 元のタイルの配列 [今とったリストの位置.x , 今とったリストの位置.y]
            // の爆弾セット関数を呼んでる
            // 元のタイルの配列=生成と表示してる配列
            TilesArray[(ArrayIndex[BombListIndex].x), (ArrayIndex[BombListIndex].y)].SetSelfBombFunc();

            // 爆弾になってしまった悲しいタイルの配列の番号を渡す
            AddAroundCountFunc(new Vector2Int((ArrayIndex[BombListIndex].x), (ArrayIndex[BombListIndex].y)));
            // Listから除外
            ArrayIndex.RemoveAt(BombListIndex);
        }

        // ちょっとむずすぎるのでいくつか安全マスを開封
        for (int i = 0; i < (int)openedTile; i++)
        {
            int SafeOpenedIndex = Random.Range(0, ArrayIndex.Count);
            TilesArray[ArrayIndex[SafeOpenedIndex].x, ArrayIndex[SafeOpenedIndex].y].coverObj.SetActive(false);
            TilesArray[ArrayIndex[SafeOpenedIndex].x, ArrayIndex[SafeOpenedIndex].y].isDigged = true;
            DiggedTileCnt++;
            ArrayIndex.RemoveAt(SafeOpenedIndex);
        }
    }

    /// <summary>
    /// 自分が爆弾なら配列の周囲の「周囲の爆弾の数」を++.爆弾の割り当てと同時に呼ぶ
    /// </summary>
    void AddAroundCountFunc(Vector2Int vec)
    {
        // 自分が爆弾なら周囲の配列の「周囲の爆弾の数」を++
        for (int i = 0; i < 8; i++)
        {
            Vector2Int tmpvec = vec + Const.ExploreVector[i];
            //print(tmpvec);
            TilesArray[tmpvec.x, tmpvec.y].ct++;
        }
    }

    /// <summary>
    /// GameLevelによって難易度の変更
    /// </summary>
    void SetGameLevFunc()
    {
        switch (gamelev)
        {
            case GameLevel.LEV5:
                TotalBombsCnt = BombLevel.LEV_5;
                openedTile = OpenedTile.LEV_5;
                break;
            case GameLevel.LEV7:
                TotalBombsCnt = BombLevel.LEV_7;
                openedTile = OpenedTile.LEV_7;
                break;
            case GameLevel.LEV10:
                TotalBombsCnt = BombLevel.LEV_10;
                openedTile = OpenedTile.LEV_10;
                break;
        }
    }


    /// <summary>
    /// タイルを空けれた時
    /// </summary>
    public void DigSuccessFunc()
    {
        DiggedTileCnt++;
        if (DiggedTileCnt == lev * lev - (int)TotalBombsCnt)
        {
            GameClear();
        }
    }

    /// <summary>
    /// 画面を重ねてContinue.or.Selectに行く
    /// </summary>
    public void GameOver()
    {
        GameOverImage.SetActive(true);
    }
    /// <summary>
    /// 画面を重ねてContinue.or.Selectに行く
    /// </summary>
    void GameClear()
    {
        GameClearImage.SetActive(true);
    }

    /// <summary>
    /// ポーズメニューを管理
    /// </summary>
    public void PoseFunc()
    {
        PosePanel.SetActive(!PosePanel.activeSelf);
        paintController.isArrowErase = !paintController.isArrowErase;
    }


    /// <summary>
    /// ヘルプを管理
    /// </summary>
    public void HelpFunc()
    {
        HelpPanel.SetActive(!HelpPanel.activeSelf);
        paintController.isArrowErase = !paintController.isArrowErase;
    }

    /// <summary>
    /// ペイントモードとプレイモードの切り替え
    /// </summary>
    public void ModeChangeFunc()
    {
        RawImage.raycastTarget = !(RawImage.raycastTarget);
        if (RawImage.raycastTarget)
        {
            ModeModule.color = Color.yellow;
            // TODO
            // モジュールをオフにする
        }
        else
        {
            ModeModule.color = Color.white;
            // TODO
            // モジュールをオンにする
        }
    }

    /// <summary>
    /// ゲームシーンの読み込み
    /// <para>主にリプレイ用</para>
    /// </summary>
    public void ReplayFunc()
    {
        SceneManager.LoadScene("MineSweeper");
    }

    /// <summary>
    /// セレクトシーンの読み込み
    /// <para>難易度選択画面へ</para>
    /// </summary>
    public void BackToSelectFunc()
    {
        SceneManager.LoadScene("StageSelect");
    }

}
