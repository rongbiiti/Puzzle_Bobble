using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{

    // ���C���ɑJ�ڂ����Ƃ��Ɏ��s���鏈��
    void Start()
    {
        
    }

    // �^�C�g�����BGM���t�F�[�h�A�E�g
    public void MainBGMFadeOut()
    {
        SoundManager.Instance.BGMFadeOut(1.9f);
    }
}
