using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SlideNumberEffectController : MonoBehaviour
{
    public Action OnComplete = null;//�I��������̃R�[���o�b�N

    private Text text;
    private float speed;
    private float number;
    private float targetNumber;

    private Coroutine playCoroutine = null;

#if UNITY_EDITOR
    [SerializeField]
    private int debugToNumber = 10;
    [SerializeField]
    private float debugDuration = 1.0f;
#endif

    void Awake()
    {
        text = GetComponent<Text>();
    }

    //
    // ���l�������ɃZ�b�g
    //
    public void SetNumber(int n)
    {
        number = (float)n;
        text.text = ((int)number).ToString("N0");
        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }
    }

    //
    // from_number ���� to_number �ɏ��X�Ɉڍs
    //
    public void SlideToNumber(int from_number, int to_number, float duration)
    {
        SetNumber(from_number);
        SlideToNumber(to_number, duration);
    }

    //
    // ���̒l���� ���� to_number �ɏ��X�Ɉڍs
    //
    public void SlideToNumber(int to_number, float duration)
    {
        targetNumber = to_number;
        speed = ((targetNumber - number) / duration);

        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
        }
        playCoroutine = StartCoroutine("slideTo");
    }

    private IEnumerator slideTo()
    {
        while (true)
        {
            var delta = speed * Time.deltaTime;
            var next_number = number + delta;
            text.text = ((int)next_number).ToString("N0");

            number = next_number;

            if (UnityEngine.Mathf.Sign(speed) * (targetNumber - number) <= 0.0f)
            {
                break;
            }
            yield return null;
        }
        playCoroutine = null; ;
        number = targetNumber;
        text.text = ((int)number).ToString("N0");
        if (OnComplete != null)
        {
            OnComplete();
            OnComplete = null;
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Test")]
    void Test()
    {
        SlideToNumber(debugToNumber, debugDuration);
    }
#endif

}