using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideCircle : MonoBehaviour
{
    /// <summary>
    /// 発射ガイドの子要素のSpriteRenderer
    /// </summary>
    private SpriteRenderer[] _chiledSprRenderers;

    void Start()
    {
        // 子要素のSpriteRenderer一気に取得
        _chiledSprRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// 子要素の表示状態切り替え
    /// </summary>
    /// <param name="value">trueで表示、falseで非表示</param>
    public void SetActiveGuideCircle(bool value)
    {
        // まだSpriteRenderer取得してなかったら取得させる
        if(_chiledSprRenderers == null)
        {
            Start();
        }

        // 子要素の表示状態をすべて切り替える
        int i = 0;
        foreach(var sp in _chiledSprRenderers)
        {
            
            if (value)
            {
                StartCoroutine(ChiledRendererActive(sp, i++ * 0.0167f));
            }
            else
            {
                sp.enabled = value;
                StopAllCoroutines();
            }
        }
    }

    private IEnumerator ChiledRendererActive(SpriteRenderer sp, float time)
    {
        yield return new WaitForSeconds(time);
        sp.enabled = true;
    }
}
