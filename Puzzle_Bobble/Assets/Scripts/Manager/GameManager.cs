using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    // �������ʂ��ړ�����
    public bool shootedBobbleMoving;

    // �A�̍폜���o����
    public bool isBobbleDeleting;

    // �A���Q�[���I�[�o�[�]�[���܂ŗ����؂�����
    public bool isBobbleFalloutGameOverZone;
}