using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Constants
{

    #region �񋓑�

    ///<summary>�y�C���g���[�h���̃y���̐F</summary>
    enum PenColors
    {
        COLOR_RED,
        COLOR_BLUE,
        COLOR_GREEN,
        COLOR_WHITE,
        COLOR_BLACK
    }

    ///<summary>
    ///�^�C���̏��.
    ///<para>SAFE:�񔚒e�^�C��</para>
    ///<para>BOMB:���e�^�C��</para>
    /// </summary>
    enum TileType
    {
        SAFE,
        BOMB,
        COUNT
    }

    /// <summary>
    /// �Q�[�����[�h
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
    /// �^�C���}�[�N�̗L��.����������Ȃ����A
    /// <para>�Ȃ��������H</para>
    /// <para>�̏ꍇ�����邽�ߊg��������������.�ڍׂ�Tile.SetMarkFunc()</para>
    /// <para>NOMARK</para>
    /// <para>FLAGED</para>
    /// </summary>
    enum MarkState { NOMARK, FLAGED }

    /// <summary>
    /// �X�^�[�g��ʂőI������Q�[���̓�Փx
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
    /// GameLevel�ɉ��������e�̑���.Variants���Q�l�ɂ������A�ނ��������̂Ō��炵�܂���
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
    /// ��Փx�Չ��̂��ߊJ�n���ɊJ�������`
    /// </summary>
    enum OpenedTile
    {
        LEV_5 = 3,
        LEV_7 = 8,
        LEV_10 = 20
    }

    #endregion

    /// <summary>
    /// �萔�Ȃǂ̒�`
    /// </summary>
    public class Const
    {

        /// <summary>�z��̍ő�̒���</summary>
        public const int MAX_LEN = 12;
        public const float TILE_SIZE = 75f;



        /// <summary>
        /// �^�C���̎��͒T���p�z��
        ///�񎟔z��̔ԍ��ƈ�v�����邽��x���W��y���W�ł͂Ȃ�
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
