using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private Transform _canvasTransform;        // Canvas��Transform�w��
    [SerializeField] private GameObject _gameStartText_Ready;   // �Q�[���X�^�[�g�̕����摜�EReady
    [SerializeField] private GameObject _gameStartText_GO;      // �Q�[���X�^�[�g�̕����摜�EGO

    void Start()
    {
        StartCoroutine(nameof(GameStart));
    }
   
    private IEnumerator GameStart()
    {
        // ����\�ɂȂ�܂ŁA�Q�[���X�s�[�h�i�������x�Ɛ������x�j��0�ɂ��Ă���
        float gmGameSpeed = GameManager.Instance.gameSpeed;
        GameManager.Instance.gameSpeed = 0;

        GameManager.Instance.isBobbleFalloutGameOverZone = true;
        
        // �|�[�Y�{�^�������Ȃ��悤�ɂ���
        FindObjectOfType<PauseButton>().GetComponent<Button>().interactable = false;

        yield return new WaitForSeconds(0.5f);

        // Ready�@�\���A3�b��ɃI�u�W�F�N�g�폜
        Destroy( Instantiate(_gameStartText_Ready, _canvasTransform), 3f);
        yield return new WaitForSeconds(1.5f);

        // GO �\��
        Destroy( Instantiate(_gameStartText_GO, _canvasTransform), 3f);
        yield return new WaitForSeconds(1f);

        // �Q�[���X�s�[�h���ɖ߂�
        GameManager.Instance.gameSpeed = gmGameSpeed;
        GameManager.Instance.isBobbleFalloutGameOverZone = false;

        // �{�^��������悤�ɂ���
        FindObjectOfType<PauseButton>().GetComponent<Button>().interactable = true;
        FindObjectOfType<DangerZone>().enabled = true;

    }
}
