using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

public class Tile : MonoBehaviour
{
    //�^�C���̏��
    TileType tiletype = TileType.SAFE;

    // �}�[�N�̏��
    MarkState markstate = MarkState.NOMARK;

    // script�擾
    GameManager gamemanager;
    
    public GameObject coverObj;
    [SerializeField] GameObject flagObj;
    [SerializeField] GameObject bombObj;
    [SerializeField] Text countText;

    // ���͂̔��e�̐��ɂ���ĕω�����F.0-8
    [SerializeField] Color[] CountColors = new Color[9];
    // ���͂̔��e�̐�
    public int ct = 0;

    // �J���Ă邩�ǂ���
    public bool isDigged = false;
    // �̎��ʔԍ�
    Vector2Int index;

    // ������
    public void Init(Vector2Int id)
    {
        gamemanager = GameObject.Find("Canvas").GetComponent<GameManager>();
        index = id;
    }

    /// <summary>
    /// �^�C�����J�����Ƃ��̏���
    /// </summary>
    public void OnDiggedFunc()
    {
        // �}�[�N���Ă�������J���Ă���@��Ȃ�
        if (isDigged || markstate == MarkState.FLAGED) return;

        //print("�����ꂽ");
        isDigged = true;
        coverObj.SetActive(false);
        switch (tiletype)
        {
            //���S�}�X
            case TileType.SAFE:
                // �@�����������J�E���g
                gamemanager.DigSuccessFunc();
                break;

            //�n���}�X
            case TileType.BOMB:
                //�Q�[���I�[�o�[����
                print("sinnda");
                gamemanager.GameOver();
                break;
        }
    }

    /// <summary>
    /// ���͂̔��e�̐����J�E���g/�\������֐�
    /// </summary>
   public void SetBombsCountFunc()
    {

        // �J�E���g�̐ݒ�/�\��
        countText.gameObject.SetActive(true);
        countText.text = ct.ToString();

        // �����ɂ���ĐF��ς��鏈��
        countText.color = CountColors[ct];
    }


    /// <summary>
    /// �������g�����e���Ƃ������o����������֐�.
    /// <para>������tiletype��BOMB�ɂ���bombObj��Active</para>
    /// </summary>
    public void SetSelfBombFunc()
    {
        tiletype = TileType.BOMB;
        print("�����e" + index);
        bombObj.SetActive(true);
        countText.gameObject.SetActive(false);
    }

    /// <summary>
    /// �^�C���Ɋ��𗧂Ă�֐�
    /// </summary>
    public void SetMarkFunc()
    {
        // �^�C�����J���Ă���}�[�N����Ȃ�
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

        // �񋓑̂��g���ꍇ�A��Ԃ̃��[�v�������₷���Ȃ�
        // ���Ɨ񋓑̂̐��i==��Ԃ̐��j�𑝂₵�Ă������ɕύX���Ȃ��̂Ŋg�����Z
        // �������A��Ԃ����[�v���鏇�Ԃɒ�`���Ă��Ȃ��Ǝg���Ȃ��̂Œ���
        {
            // ��Ԃ̕ω�
            markstate++;
            // �񋓑̂̒������擾
            int MarkStateLen = System.Enum.GetNames(typeof(MarkState)).Length;
            // �񋓑̂̍Ō�܂ōs�����璷���������čŏ��ɖ߂�
            if ((int)markstate == MarkStateLen)
            {
                markstate -= MarkStateLen;
            }
        }
    }


    private void Start()
    {
        // �J�E���g�\��
        SetBombsCountFunc();
    }


}
