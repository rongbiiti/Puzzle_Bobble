using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleFallSpeedControll : MonoBehaviour
{
    /// <summary>
    /// この間、泡の落下速度が早まる
    /// </summary>
    [SerializeField] private float _bobbleFallSpeedUpTime = 5f;

    /// <summary>
    /// 落下速度がはまっている間、この速度で落下する
    /// </summary>
    [SerializeField] private float _SpeedUpFallSpeed = 10f;

    /// <summary>
    /// トリガーに泡が触れていなかったら、この秒数待ってからスピードアップをする
    /// もしこの間に泡が触れたら、スピードアップをしない
    /// </summary>
    [SerializeField] private float _speedUpDelay = 2f;

    private int contactCount;   // このオブジェクトのトリガー範囲と接触している泡の個数
    private bool isStartedSpeedUpCoroutine; // コルーチンを呼び出したか
    private float startFallSpeed;

    private void Start()
    {
        startFallSpeed = GameManager.Instance.gameSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bobble"))
        {
            contactCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Bobble"))
        {
            contactCount--;
        }

        if(contactCount <= 0 && !isStartedSpeedUpCoroutine)
        {
            isStartedSpeedUpCoroutine = true;
            StartCoroutine(nameof(FallSpeedUp));
        }
    }

    private IEnumerator FallSpeedUp()
    {
        yield return new WaitForSeconds(_speedUpDelay);

        if(contactCount <= 0)
        {
            GameManager.Instance.gameSpeed = _SpeedUpFallSpeed;
            yield return new WaitForSeconds(_bobbleFallSpeedUpTime);
        }
        

        GameManager.Instance.gameSpeed = startFallSpeed;
        isStartedSpeedUpCoroutine = false;
    }

    
}
