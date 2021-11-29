using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamingColorChangeAnim : MonoBehaviour
{
    /// <summary>
    /// �F�ύX�Ԋu
    /// </summary>
    [SerializeField] private float ChangeColorTime = 0.01f;

    /// <summary>
    /// �F�ύX�̂Ȃ߂炩��
    /// </summary>
    [SerializeField] private float Smooth = 0.01f;

    /// <summary>
    /// �F��
    /// </summary>
    [SerializeField, Range(0, 1)] private float HSV_Hue = 1.0f;

    /// <summary>
    /// �ʓx
    /// </summary>
    [SerializeField, Range(0, 1)] private float HSV_Saturation = 1.0f;

    /// <summary>
    /// ���x
    /// </summary>
    [SerializeField, Range(0, 1)] private float HSV_Brightness = 1.0f;

    /// <summary>
    /// �F��MAX
    /// </summary>
    [SerializeField, Range(0, 1)] private float HSV_Hue_Max = 1.0f;

    /// <summary>
    /// �F��MIN
    /// </summary>
    [SerializeField, Range(0, 1)] private float HSV_Hue_Min = 0.0f;

    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        HSV_Hue = HSV_Hue_Min;
        StartCoroutine(nameof(ChangeColor));
    }

    /// <summary>
    /// Text��Color��1680���F�Ɍ��点��A�j���[�V�����R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeColor()
    {
        // �������[�v
        while (true)
        {
            HSV_Hue += Smooth;

            if (HSV_Hue >= HSV_Hue_Max)
            {
                HSV_Hue = HSV_Hue_Min;
            }

            // HSV��RGB�ɕϊ����AText��Color�ɃZ�b�g
            text.color = Color.HSVToRGB(HSV_Hue, HSV_Saturation, HSV_Brightness);

            yield return new WaitForSeconds(ChangeColorTime);
        }        

    }
}
