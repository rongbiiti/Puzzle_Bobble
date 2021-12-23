using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private Transform _canvasTransform;        // CanvasのTransform指定
    [SerializeField] private GameObject _gameStartText_Ready;   // ゲームスタートの文字画像・Ready
    [SerializeField] private GameObject _gameStartText_GO;      // ゲームスタートの文字画像・GO

    void Start()
    {
        StartCoroutine(nameof(GameStart));
    }
   
    private IEnumerator GameStart()
    {
        // 操作可能になるまで、ゲームスピード（落下速度と生成速度）を0にしておく
        float gmGameSpeed = GameManager.Instance.gameSpeed;
        GameManager.Instance.gameSpeed = 0;

        GameManager.Instance.isBobbleFalloutGameOverZone = true;
        
        // ポーズボタン押せないようにする
        FindObjectOfType<PauseButton>().GetComponent<Button>().interactable = false;

        yield return new WaitForSeconds(0.5f);

        // Ready　表示、3秒後にオブジェクト削除
        Destroy( Instantiate(_gameStartText_Ready, _canvasTransform), 3f);
        yield return new WaitForSeconds(1.5f);

        // GO 表示
        Destroy( Instantiate(_gameStartText_GO, _canvasTransform), 3f);
        yield return new WaitForSeconds(1f);

        // ゲームスピード元に戻す
        GameManager.Instance.gameSpeed = gmGameSpeed;
        GameManager.Instance.isBobbleFalloutGameOverZone = false;

        // ボタン押せるようにする
        FindObjectOfType<PauseButton>().GetComponent<Button>().interactable = true;
        FindObjectOfType<DangerZone>().enabled = true;

    }
}
