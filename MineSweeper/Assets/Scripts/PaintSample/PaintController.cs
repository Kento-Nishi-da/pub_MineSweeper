using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Constants;

public class PaintController : MonoBehaviour
{
    /*
     * Texture2D�̕׋�
     */

    //RawImage�I�u�W�F�N�g�ƃe�N�X�`���̐錾
    [SerializeField]
    private RawImage RawImageCanvas = null;
    private Texture2D Texture = null;

    //�y���Ə����S���̑傫���̐ݒ�
    [SerializeField] private int PenWidth = 8;
    [SerializeField] private int PenHeight = 8;
    [SerializeField] private int ErasWidth = 30;
    [SerializeField] private int ErasHeight = 30;

    private Vector2 tmpPos;
    private Vector2 TouchPos;

    private Rect CanvasSize;

    private float ClickTime, tmpClickTime;


    #region ��ʓ��̊e���W���[���̕���
    // ���ݑI�𒆂̐F�����o�I�ɂ킩��₷��
    [SerializeField] private Image PenColorModule;

    // �F�̊Ǘ���񋓑̂�
    PenColors pencol;

    // �I�𒆂̐F�����@�\���|�[�Y���Ȃǂł������Ă��܂����߂���𐧌䂷��
    public bool isArrowErase;


    #endregion


    /// <summary>�y���̐F�̎w��</summary>
    [SerializeField]
    private Color PenColor;



    public void OnDrag(BaseEventData arg) //����`��
    {
        // �y�����[�h.���N���b�N�����Ȃ���h���b�O�Ő���������
        if (Input.GetMouseButton(0))
        {
            PenModeFunc(arg, PenColor, PenWidth, PenHeight);
        }


        // �����S�����[�h.�E�N���b�N�����Ȃ���h���b�O�Ő���������
        if (Input.GetMouseButton(1))
        {
            PenModeFunc(arg, Color.clear, ErasWidth, ErasHeight);
        }

    }

    private void Start()
    {
        CanvasSize = RawImageCanvas.gameObject.GetComponent<RectTransform>().rect;
        Texture = new Texture2D((int)CanvasSize.width, (int)CanvasSize.height, TextureFormat.RGBA32, false);

        //�e�N�X�`��������
        ResetTextureFunc();
        RawImageCanvas.texture = Texture;


        // �e���W���[���̏�����
        pencol = PenColors.COLOR_RED;
        PenColor = Color.red;
        PenColorModule.color = PenColor;
        RawImageCanvas.raycastTarget = false;

        isArrowErase = true;
    }
    private void Update()
    {
        // ���ݑI�𒆂̐F����
        if (Input.GetMouseButtonDown(2))
        {
            ColorResetTextureFunc();
        }

        // �F�ς��̏���
        ColorChangeFunc();

    }

    /// <summary>
    /// �h���b�O���ɕ`�悵�Ă����֐�.
    /// EventTrigger��BaseEventData�ƐF�A�y���̑傫����n��
    /// </summary>
    private void PenModeFunc(BaseEventData arg, Color color, int Width, int Height)
    {

        PointerEventData _event = arg as PointerEventData; //�^�b�`�̏��擾

        // ������Ă���Ƃ��̏���
        TouchPos = _event.position; //���݂̃|�C���^�̍��W
        ClickTime = _event.clickTime; //�Ō�ɃN���b�N�C�x���g�����M���ꂽ���Ԃ��擾

        float disTime = ClickTime - tmpClickTime; //�O��̃N���b�N�C�x���g�Ƃ̎���


        var dir = tmpPos - TouchPos; //���O�̃^�b�`���W�Ƃ̍�
        if (disTime > 0.01) dir = new Vector2(0, 0); //0.1�b�ȏ�Ԋu����������^�b�`���W�̍���0�ɂ���

        var dist = (int)dir.magnitude; //�^�b�`���W�x�N�g���̐�Βl

        dir = dir.normalized; //���K��

        //�w��̃y���̑���(�s�N�Z��)�ŁA�O��̃^�b�`���W���獡��̃^�b�`���W�܂œh��Ԃ�
        for (int d = 0; d < dist; ++d)
        {
            var p_pos = TouchPos + dir * d; //paint position
            p_pos.y -= Height / 2.0f;
            p_pos.x -= Width / 2.0f;
            for (int h = 0; h < Height; ++h)
            {
                int y = (int)(p_pos.y + h);
                if (y < 0 || y > Texture.height) continue; //�^�b�`���W���e�N�X�`���̊O�̏ꍇ�A�`�揈�����s��Ȃ�

                for (int w = 0; w < Width; ++w)
                {
                    int x = (int)(p_pos.x + w);
                    if (x >= 0 && x <= Texture.width)
                    {
                        Texture.SetPixel(x, y, color); //����`�悷��ʒu������
                    }
                }
            }
        }
        // ���ۂ̕`��͂���
        Texture.Apply();
        // �O�t���[���Ɣ�r���邽�ߍŌ�ɋL�^
        tmpPos = TouchPos;
        tmpClickTime = ClickTime;

    }

    /// <summary>
    /// �L�[���͂���y���̐F��ύX����֐�
    /// 0.black
    /// 1.red
    /// 2.blue
    /// 3.green
    /// <para>���C���̓z�C�[���R���R���ŕύX���Ă��炤</para>
    /// </summary>
    private void ColorChangeFunc()
    {
        if (isArrowErase)
        {
            GetWheelFunc();

            //���F
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                PenColor = Color.black;
            }
            //�ԐF
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PenColor = Color.red;
            }
            //�F
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PenColor = Color.blue;
            }
            //�ΐF
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PenColor = Color.green;
            }
            PenColorModule.color = PenColor;
        }

    }

    /// <summary>
    /// �z�C�[���̓��͂���F��ύX����
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

    ///<summary>��ʑS�̂̃e�N�X�`���������֐�</summary>
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
    /// ���݂̃y���̐F�݂̂��������������̊֐�.
    /// �z�C�[���N���b�N�Ŕ���
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