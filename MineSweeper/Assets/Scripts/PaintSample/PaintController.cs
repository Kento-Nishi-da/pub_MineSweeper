using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Constants;

public class PaintController : MonoBehaviour
{
    /*
     * Texture2Dの勉強
     */

    //RawImageオブジェクトとテクスチャの宣言
    [SerializeField]
    private RawImage RawImageCanvas = null;
    private Texture2D Texture = null;

    //ペンと消しゴムの大きさの設定
    [SerializeField] private int PenWidth = 8;
    [SerializeField] private int PenHeight = 8;
    [SerializeField] private int ErasWidth = 30;
    [SerializeField] private int ErasHeight = 30;

    private Vector2 tmpPos;
    private Vector2 TouchPos;

    private Rect CanvasSize;

    private float ClickTime, tmpClickTime;


    #region 画面内の各モジュールの部分
    // 現在選択中の色を視覚的にわかりやすく
    [SerializeField] private Image PenColorModule;

    // 色の管理を列挙体で
    PenColors pencol;

    // 選択中の色消す機能がポーズ中などでも動いてしまうためそれを制御する
    public bool isArrowErase;


    #endregion


    /// <summary>ペンの色の指定</summary>
    [SerializeField]
    private Color PenColor;



    public void OnDrag(BaseEventData arg) //線を描画
    {
        // ペンモード.左クリック押しながらドラッグで線をかける
        if (Input.GetMouseButton(0))
        {
            PenModeFunc(arg, PenColor, PenWidth, PenHeight);
        }


        // 消しゴムモード.右クリック押しながらドラッグで線を消せる
        if (Input.GetMouseButton(1))
        {
            PenModeFunc(arg, Color.clear, ErasWidth, ErasHeight);
        }

    }

    private void Start()
    {
        CanvasSize = RawImageCanvas.gameObject.GetComponent<RectTransform>().rect;
        Texture = new Texture2D((int)CanvasSize.width, (int)CanvasSize.height, TextureFormat.RGBA32, false);

        //テクスチャ初期化
        ResetTextureFunc();
        RawImageCanvas.texture = Texture;


        // 各モジュールの初期化
        pencol = PenColors.COLOR_RED;
        PenColor = Color.red;
        PenColorModule.color = PenColor;
        RawImageCanvas.raycastTarget = false;

        isArrowErase = true;
    }
    private void Update()
    {
        // 現在選択中の色消去
        if (Input.GetMouseButtonDown(2))
        {
            ColorResetTextureFunc();
        }

        // 色変えの処理
        ColorChangeFunc();

    }

    /// <summary>
    /// ドラッグ中に描画していく関数.
    /// EventTriggerのBaseEventDataと色、ペンの大きさを渡す
    /// </summary>
    private void PenModeFunc(BaseEventData arg, Color color, int Width, int Height)
    {

        PointerEventData _event = arg as PointerEventData; //タッチの情報取得

        // 押されているときの処理
        TouchPos = _event.position; //現在のポインタの座標
        ClickTime = _event.clickTime; //最後にクリックイベントが送信された時間を取得

        float disTime = ClickTime - tmpClickTime; //前回のクリックイベントとの時差


        var dir = tmpPos - TouchPos; //直前のタッチ座標との差
        if (disTime > 0.01) dir = new Vector2(0, 0); //0.1秒以上間隔があいたらタッチ座標の差を0にする

        var dist = (int)dir.magnitude; //タッチ座標ベクトルの絶対値

        dir = dir.normalized; //正規化

        //指定のペンの太さ(ピクセル)で、前回のタッチ座標から今回のタッチ座標まで塗りつぶす
        for (int d = 0; d < dist; ++d)
        {
            var p_pos = TouchPos + dir * d; //paint position
            p_pos.y -= Height / 2.0f;
            p_pos.x -= Width / 2.0f;
            for (int h = 0; h < Height; ++h)
            {
                int y = (int)(p_pos.y + h);
                if (y < 0 || y > Texture.height) continue; //タッチ座標がテクスチャの外の場合、描画処理を行わない

                for (int w = 0; w < Width; ++w)
                {
                    int x = (int)(p_pos.x + w);
                    if (x >= 0 && x <= Texture.width)
                    {
                        Texture.SetPixel(x, y, color); //線を描画する位置を決定
                    }
                }
            }
        }
        // 実際の描画はここ
        Texture.Apply();
        // 前フレームと比較するため最後に記録
        tmpPos = TouchPos;
        tmpClickTime = ClickTime;

    }

    /// <summary>
    /// キー入力からペンの色を変更する関数
    /// 0.black
    /// 1.red
    /// 2.blue
    /// 3.green
    /// <para>メインはホイールコロコロで変更してもらう</para>
    /// </summary>
    private void ColorChangeFunc()
    {
        if (isArrowErase)
        {
            GetWheelFunc();

            //黒色
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                PenColor = Color.black;
            }
            //赤色
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PenColor = Color.red;
            }
            //青色
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PenColor = Color.blue;
            }
            //緑色
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PenColor = Color.green;
            }
            PenColorModule.color = PenColor;
        }

    }

    /// <summary>
    /// ホイールの入力から色を変更する
    /// </summary>
    void GetWheelFunc()
    {
        var scrooldelta = Input.mouseScrollDelta.y * Time.deltaTime;

        if (scrooldelta < 0)
        {
            pencol++;
            int PenColorsLen = System.Enum.GetNames(typeof(PenColors)).Length;
            if ((int)pencol == PenColorsLen)
            {
                pencol -= PenColorsLen;
            }
            //print(pencol);
        }
        else if (scrooldelta > 0)
        {
            pencol--;
            int PenColorsLen = System.Enum.GetNames(typeof(PenColors)).Length;
            if ((int)pencol < 0)
            {
                pencol += PenColorsLen;
            }
            //print(pencol);
        }
        switch (pencol)
        {
            case PenColors.COLOR_RED:
                PenColor = Color.red;
                break;
            case PenColors.COLOR_BLUE:
                PenColor = Color.blue;
                break;
            case PenColors.COLOR_GREEN:
                PenColor = Color.green;
                break;
            case PenColors.COLOR_WHITE:
                PenColor = Color.white;
                break;
            case PenColors.COLOR_BLACK:
                PenColor = Color.black;
                break;
        }
    }

    ///<summary>画面全体のテクスチャを消す関数</summary>
    public void ResetTextureFunc()
    {
        for (int w = 0; w < (int)CanvasSize.width; w++)
        {
            for (int h = 0; h < (int)CanvasSize.height; h++)
            {
                Texture.SetPixel(w, h, Color.clear);
            }
        }
        Texture.Apply();
    }

    /// <summary>
    /// 現在のペンの色のみを消去したい時の関数.
    /// ホイールクリックで発動
    /// </summary>
    private void ColorResetTextureFunc()
    {
        if (isArrowErase)
        {
            for (int w = 0; w < (int)CanvasSize.width; w++)
            {
                for (int h = 0; h < (int)CanvasSize.height; h++)
                {
                    if (Texture.GetPixel(w, h) == PenColor)
                    {
                        Texture.SetPixel(w, h, Color.clear);
                    }
                }
            }
            Texture.Apply();
        }
    }




}