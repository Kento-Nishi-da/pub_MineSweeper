using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

public class Tile : MonoBehaviour
{
    //タイルの状態
    TileType tiletype = TileType.SAFE;

    // マークの状態
    MarkState markstate = MarkState.NOMARK;

    // script取得
    GameManager gamemanager;
    
    public GameObject coverObj;
    [SerializeField] GameObject flagObj;
    [SerializeField] GameObject bombObj;
    [SerializeField] Text countText;

    // 周囲の爆弾の数によって変化する色.0-8
    [SerializeField] Color[] CountColors = new Color[9];
    // 周囲の爆弾の数
    public int ct = 0;

    // 開いてるかどうか
    public bool isDigged = false;
    // 個体識別番号
    Vector2Int index;

    // 初期化
    public void Init(Vector2Int id)
    {
        gamemanager = GameObject.Find("Canvas").GetComponent<GameManager>();
        index = id;
    }

    /// <summary>
    /// タイルを開いたときの処理
    /// </summary>
    public void OnDiggedFunc()
    {
        // マークついてたりもう開いてたら掘れない
        if (isDigged || markstate == MarkState.FLAGED) return;

        //print("押された");
        isDigged = true;
        coverObj.SetActive(false);
        switch (tiletype)
        {
            //安全マス
            case TileType.SAFE:
                // 掘った枚数をカウント
                gamemanager.DigSuccessFunc();
                break;

            //地雷マス
            case TileType.BOMB:
                //ゲームオーバー処理
                print("sinnda");
                gamemanager.GameOver();
                break;
        }
    }

    /// <summary>
    /// 周囲の爆弾の数をカウント/表示する関数
    /// </summary>
   public void SetBombsCountFunc()
    {

        // カウントの設定/表示
        countText.gameObject.SetActive(true);
        countText.text = ct.ToString();

        // 数字によって色を変える処理
        countText.color = CountColors[ct];
    }


    /// <summary>
    /// 自分自身が爆弾だという自覚を持たせる関数.
    /// <para>自分のtiletypeをBOMBにしてbombObjをActive</para>
    /// </summary>
    public void SetSelfBombFunc()
    {
        tiletype = TileType.BOMB;
        print("俺爆弾" + index);
        bombObj.SetActive(true);
        countText.gameObject.SetActive(false);
    }

    /// <summary>
    /// タイルに旗を立てる関数
    /// </summary>
    public void SetMarkFunc()
    {
        // タイルが開いてたらマークつけれない
        if(isDigged) return;

        switch (markstate)
        {
            case MarkState.NOMARK:
                flagObj.SetActive(true);
                break;
            case MarkState.FLAGED:
                flagObj.SetActive(false);
                break;
                default:
                // nothing
                break;
        }

        // 列挙体を使う場合、状態のループを扱いやすくなる
        // あと列挙体の数（==状態の数）を増やしても処理に変更がないので拡張性〇
        // ただし、状態がループする順番に定義していないと使えないので注意
        {
            // 状態の変化
            markstate++;
            // 列挙体の長さを取得
            int MarkStateLen = System.Enum.GetNames(typeof(MarkState)).Length;
            // 列挙体の最後まで行ったら長さ分引いて最初に戻す
            if ((int)markstate == MarkStateLen)
            {
                markstate -= MarkStateLen;
            }
        }
    }


    private void Start()
    {
        // カウント表示
        SetBombsCountFunc();
    }


}
