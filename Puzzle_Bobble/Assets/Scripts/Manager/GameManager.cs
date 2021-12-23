using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    /// <summary>
    /// �A�j���[�V��������Q�[���I�[�o�[�e�L�X�g��Prefab
    /// </summary>
    [SerializeField] private GameObject _gameOverTextPrefab;

    /// <summary>
    /// �Q�[���I�[�o�[UI
    /// </summary>
    [SerializeField] private GameObject _gameOverPanel;
    public GameObject GetGameOverPanel()
    {
        return _gameOverPanel;
    }

    // �������x�㏸�]�[���ɓ������A�̐�
    public int fallSpeedUpZoneContactCount;

    // �f���W���[�]�[���ɓ������A�̐�
    public int dangerZoneContactCount;

    // �������ʂ��ړ�����
    public bool shootedBobbleMoving;

    // �A�̍폜���o����
    public bool isBobbleDeleting;

    // �A���Q�[���I�[�o�[�]�[���܂ŗ����؂�����
    public bool isBobbleFalloutGameOverZone;

    // �A�����������Q�[���I�[�o�[�]�[���ɒB�������ȁu�f���W���[�^�C�����v��
    public bool isDangerTime;

    // �Q�[���X�s�[�h
    public float gameSpeed = 1f;

    private void Start()
    {
        // �Q�Ƃ��󂾂����猟�����ċL�����Ă����B
        if(_gameOverPanel == null)
        {
            _gameOverPanel = GameObject.Find("GameOverPanel");
        }
    }

    /// <summary>
    /// �A�j���[�V��������Q�[���I�[�o�[�e�L�X�g��Prefab���o��
    /// </summary>
    public void InstantiateGameOverText()
    {
        Instantiate(_gameOverTextPrefab, ScoreManager.Instance.GetCanvas().transform);
        isBobbleFalloutGameOverZone = true;
        FindObjectOfType<PauseButton>().gameObject.SetActive(false);
    }

    /// <summary>
    /// �Q�[���I�[�o�[����
    /// </summary>
    public void GameOver()
    {
        // �Q�[���I�[�o�[��UI���A�N�e�B�u�ɂ���
        _gameOverPanel.SetActive(true);
    }
}