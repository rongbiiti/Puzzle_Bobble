using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    // �^�C�g�����BGM
    [SerializeField] private BGM _titleSceneBGM;

    // �^�C�g����ʂɑJ�ڂ����Ƃ��Ɏ��s���鏈��
    void Start()
    {
        // �^�C�g�����BGM���Đ�����
        SoundManager.Instance.PlayBGM(_titleSceneBGM);
    }

    // �^�C�g�����BGM���t�F�[�h�A�E�g
    public void TitleBGMFadeOut()
    {
        SoundManager.Instance.BGMFadeOut(1.9f);
    }
    
}