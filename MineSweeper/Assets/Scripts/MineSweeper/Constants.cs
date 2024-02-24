using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Constants
{

    #region 列挙体

    ///<summary>ペイントモード中のペンの色</summary>
    enum PenColors
    {
        COLOR_RED,
        COLOR_BLUE,
        COLOR_GREEN,
        COLOR_WHITE,
        COLOR_BLACK
    }

    ///<summary>
    ///タイルの状態.
    ///<para>SAFE:非爆弾タイル</para>
    ///<para>BOMB:爆弾タイル</para>
    /// </summary>
    enum TileType
    {
        SAFE,
        BOMB,
        COUNT
    }

    /// <summary>
    /// ゲームモード
    /// <para>GAMEPLAY</para>
    /// <para>GAMEOVER</para>
    /// <para>GAMECLEAR</para>
    /// </summary>
    public enum GameMode
    {
        GAMEPLAY,
        GAMEOVER,
        GAMECLEAR
    }

    /// <summary>
    /// タイルマークの有無.今回実装しないが、
    /// <para>なし＞旗＞？</para>
    /// <para>の場合もあるため拡張性を持たせた.詳細はTile.SetMarkFunc()</para>
    /// <para>NOMARK</para>
    /// <para>FLAGED</para>
    /// </summary>
    enum MarkState { NOMARK, FLAGED }

    /// <summary>
    /// スタート画面で選択するゲームの難易度
    /// <para>LEV5</para>
    /// <para>LEV7</para>
    /// <para>LEV10</para>
    /// </summary>
    public enum GameLevel
    {
        NONE = 0,
        LEV5 = 5,
        LEV7 = 7,
        LEV10 = 10
    }

    /// <summary>
    /// GameLevelに応じた爆弾の総数.Variantsを参考にしたが、むずすぎたので減らしました
    /// <para>5=10</para>
    /// <para>7=20</para>
    /// <para>10=40</para>
    /// </summary>
    enum BombLevel
    {
        LEV_5 = 5,
        LEV_7 = 10,
        LEV_10 = 20
    }

    /// <summary>
    /// 難易度易化のため開始時に開く個数を定義
    /// </summary>
    enum OpenedTile
    {
        LEV_5 = 3,
        LEV_7 = 8,
        LEV_10 = 20
    }

    #endregion

    /// <summary>
    /// 定数などの定義
    /// </summary>
    public class Const
    {

        /// <summary>配列の最大の長さ</summary>
        public const int MAX_LEN = 12;
        public const float TILE_SIZE = 75f;



        /// <summary>
        /// タイルの周囲探索用配列
        ///二次配列の番号と一致させるためx座標とy座標ではない
        /// <para>7 0 1</para>
        /// <para>6 x 2</para>
        /// <para>5 4 3</para>
        /// </summary>
        public static Vector2Int[] ExploreVector =
        {
            new Vector2Int( 1, 0),
            new Vector2Int( 1, 1),
            new Vector2Int( 0, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1,-1),
            new Vector2Int( 0,-1),
            new Vector2Int( 1,-1)
        };
    }


}
