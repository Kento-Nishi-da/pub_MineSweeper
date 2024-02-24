using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Constants;

public class GameManager : MonoBehaviour
{
    // ���̓�Փx�̏������A�X�e�[�W�Z���N�g����ύX�\��
    public static GameLevel gamelev = GameLevel.LEV10;
    // int�ł��錾
    int lev;
    // �Q�[���̏��
    public static GameMode mode = GameMode.GAMEPLAY;

    // prefab
    [SerializeField]
    GameObject tileprefabObj;
    // class��GameObject�̕ϊ����߂�ǂ������̂ŗ����錾
    [SerializeField]
    Tile tileprefabtile;
    // �^�C���i�[�p
    [SerializeField]
    GameObject TilesRoot;
    // ���s�������ꂼ��̐���p
    [SerializeField]
    GameObject GameOverImage;
    [SerializeField]
    GameObject GameClearImage;

    // ���j���[�p�l��
    [SerializeField] private GameObject PosePanel;
    // �w���v�p�l��
    [SerializeField] private GameObject HelpPanel;
    // �y�C���g�𐧌�
    [SerializeField] PaintController paintController;
    // ���݂̃��[�h���킩��₷��
    [SerializeField] private Image ModeModule;
    [SerializeField] private RawImage RawImage;
    [SerializeField] private GameObject a;


    /// <summary>
    /// �J�����������i�[
    /// </summary>
    int DiggedTileCnt;


    // �^�C���񎟔z��錾
    Tile[,] TilesArray = new Tile[Const.MAX_LEN, Const.MAX_LEN];


    BombLevel TotalBombsCnt;
    OpenedTile openedTile;

    private void Start()
    {
        // gamelev�̉����Ĕ��e�̌���ύX
        SetGameLevFunc();
        // int�֕ϊ�
        lev = (int)gamelev;
        DiggedTileCnt = 0;
        SetTilesFunc();
        SetBombsFunc();

        ModeModule.color = Color.white;

        // ������
        GameOverImage.SetActive(false);
        GameClearImage.SetActive(false);
        PosePanel.SetActive(false);
        HelpPanel.SetActive(false);
    }


    /// <summary>
    /// �^�C���̐���/�ݒu
    /// </summary>
    void SetTilesFunc()
    {
        // �^�C���̈�ڂ̈ʒu���v�Z
        Vector2 ScreenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        //print(ScreenCenter);
        float tmp = ((lev - 1) / 2) * Const.TILE_SIZE;
        Vector2 LeftOverTilePos = ScreenCenter + new Vector2(-tmp, tmp);

        // �^�C���̐ݒu

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

                // �^�C������
                Tile tile = Instantiate(tileprefabtile, TilesRoot.transform);
                tile.GetComponent<RectTransform>().position = thisTilePos;
                // ���ʔԍ����蓖��
                if (i <= lev && i >= 1 && j <= lev && j >= 1)
                {
                    tile.Init(new Vector2Int(i, j));
                }
                TilesArray[i, j] = tile;

                // �z��O��T�����Ȃ��悤��0-MAX_LEN�Ő������邪�g���̂�1-lev�Ȃ̂Ŕ�\��
                TilesArray[i, j].gameObject.SetActive(false);
                if (i <= lev && i >= 1 && j <= lev && j >= 1)
                {
                    TilesArray[i, j].gameObject.SetActive(true);
                }
            }
        }
    }


    /// <summary>
    /// ���e�̊��蓖��
    /// </summary>
    void SetBombsFunc()
    {
        // Vector2Int�^�̃��X�g�����ATilesArray�ɑΉ�����悤��Add
        List<Vector2Int> ArrayIndex = new List<Vector2Int>(lev * lev);
        for (int i = 1; i <= lev; i++)
        {
            for (int j = 1; j <= lev; j++)
            {
                ArrayIndex.Add(new Vector2Int(i, j));
                //print("i:" + i + "\nj:" + j);
            }
        }

        // ���ꂽ�����e�����[�v
        // ��������X�g���烉���_���ɑI�o�A���X�g���珜�O
        for (int i = 0; i < (int)TotalBombsCnt; i++)
        {

            // �܂��K�v�ȗv�f���̒��ŗ������Ƃ�
            int BombListIndex = Random.Range(0, ArrayIndex.Count);

            //print("BombListIndex:" + BombListIndex);
            //print(ArrayIndex[BombListIndex]);

            // ���̃^�C���̔z�� [���Ƃ������X�g�̈ʒu.x , ���Ƃ������X�g�̈ʒu.y]
            // �̔��e�Z�b�g�֐����Ă�ł�
            // ���̃^�C���̔z��=�����ƕ\�����Ă�z��
            TilesArray[(ArrayIndex[BombListIndex].x), (ArrayIndex[BombListIndex].y)].SetSelfBombFunc();

            // ���e�ɂȂ��Ă��܂����߂����^�C���̔z��̔ԍ���n��
            AddAroundCountFunc(new Vector2Int((ArrayIndex[BombListIndex].x), (ArrayIndex[BombListIndex].y)));
            // List���珜�O
            ArrayIndex.RemoveAt(BombListIndex);
        }

        // ������Ƃނ�������̂ł��������S�}�X���J��
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
    /// ���������e�Ȃ�z��̎��͂́u���͂̔��e�̐��v��++.���e�̊��蓖�ĂƓ����ɌĂ�
    /// </summary>
    void AddAroundCountFunc(Vector2Int vec)
    {
        // ���������e�Ȃ���͂̔z��́u���͂̔��e�̐��v��++
        for (int i = 0; i < 8; i++)
        {
            Vector2Int tmpvec = vec + Const.ExploreVector[i];
            //print(tmpvec);
            TilesArray[tmpvec.x, tmpvec.y].ct++;
        }
    }

    /// <summary>
    /// GameLevel�ɂ���ē�Փx�̕ύX
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
    /// �^�C�����󂯂ꂽ��
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
    /// ��ʂ��d�˂�Continue.or.Select�ɍs��
    /// </summary>
    public void GameOver()
    {
        GameOverImage.SetActive(true);
    }
    /// <summary>
    /// ��ʂ��d�˂�Continue.or.Select�ɍs��
    /// </summary>
    void GameClear()
    {
        GameClearImage.SetActive(true);
    }

    /// <summary>
    /// �|�[�Y���j���[���Ǘ�
    /// </summary>
    public void PoseFunc()
    {
        PosePanel.SetActive(!PosePanel.activeSelf);
        paintController.isArrowErase = !paintController.isArrowErase;
    }


    /// <summary>
    /// �w���v���Ǘ�
    /// </summary>
    public void HelpFunc()
    {
        HelpPanel.SetActive(!HelpPanel.activeSelf);
        paintController.isArrowErase = !paintController.isArrowErase;
    }

    /// <summary>
    /// �y�C���g���[�h�ƃv���C���[�h�̐؂�ւ�
    /// </summary>
    public void ModeChangeFunc()
    {
        RawImage.raycastTarget = !(RawImage.raycastTarget);
        if (RawImage.raycastTarget)
        {
            ModeModule.color = Color.yellow;
            // TODO
            // ���W���[�����I�t�ɂ���
        }
        else
        {
            ModeModule.color = Color.white;
            // TODO
            // ���W���[�����I���ɂ���
        }
    }

    /// <summary>
    /// �Q�[���V�[���̓ǂݍ���
    /// <para>��Ƀ��v���C�p</para>
    /// </summary>
    public void ReplayFunc()
    {
        SceneManager.LoadScene("MineSweeper");
    }

    /// <summary>
    /// �Z���N�g�V�[���̓ǂݍ���
    /// <para>��Փx�I����ʂ�</para>
    /// </summary>
    public void BackToSelectFunc()
    {
        SceneManager.LoadScene("StageSelect");
    }

}
