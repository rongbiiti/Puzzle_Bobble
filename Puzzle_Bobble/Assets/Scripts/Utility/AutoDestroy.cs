using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    /// <summary>
    /// 自動で破壊されるまでの待機時間
    /// </summary>
    [SerializeField] private float _destroyTime = 1f;
    public float DestroyTime {
        set { _destroyTime = value; }
        get { return _destroyTime; }
    }

    void Start()
    {
        StartCoroutine(nameof(DestroyCoroutine));
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(_destroyTime);
        Destroy(gameObject);
    }
}
