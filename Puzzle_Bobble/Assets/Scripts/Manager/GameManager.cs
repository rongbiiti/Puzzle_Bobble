using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    /// <summary>
    /// �Q�[���I�[�o�[UI
    /// </summary>
    [SerializeField] private GameObject _gameOverPanel;
    public GameObject GetGameOverPanel()
    {
        return _gameOverPanel;
    }

    // �������ʂ��ړ�����
    public bool shootedBobbleMoving;

    // �A�̍폜���o����
    public bool isBobbleDeleting;

    // �A���Q�[���I�[�o�[�]�[���܂ŗ����؂�����
    public bool isBobbleFalloutGameOverZone;

    private void Start()
    {
        // �Q�Ƃ��󂾂����猟�����ċL�����Ă����B
        if(_gameOverPanel == null)
        {
            _gameOverPanel = GameObject.Find("GameOverPanel");
        }
    }
}