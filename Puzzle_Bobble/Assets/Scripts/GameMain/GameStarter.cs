using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private Transform _canvasTransform;
    [SerializeField] private GameObject _gameStartText_Ready;
    [SerializeField] private GameObject _gameStartText_GO;


    void Start()
    {
        StartCoroutine(nameof(GameStart));
    }
   
    void Update()
    {
        
    }

    private IEnumerator GameStart()
    {
        // 操作可能になるまで、ゲームスピード（落下速度と生成速度）を0にしておく
        float gmGameSpeed = GameManager.Instance.gameSpeed;
        GameManager.Instance.gameSpeed = 0;

        GameManager.Instance.isBobbleFalloutGameOverZone = true;
        FindObjectOfType<GameOverZone>().enabled = false;
        FindObjectOfType<PauseButton>().GetComponent<Button>().interactable = false;

        yield return new WaitForSeconds(0.5f);

        Destroy( Instantiate(_gameStartText_Ready, _canvasTransform), 3f);
        yield return new WaitForSeconds(1.5f);

        Destroy( Instantiate(_gameStartText_GO, _canvasTransform), 3f);
        yield return new WaitForSeconds(1f);

        GameManager.Instance.gameSpeed = gmGameSpeed;
        GameManager.Instance.isBobbleFalloutGameOverZone = false;
        FindObjectOfType<GameOverZone>().enabled = true;
        FindObjectOfType<PauseButton>().GetComponent<Button>().interactable = true;

    }
}
