using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    /// <summary>
    /// Ž©“®‚Å”j‰ó‚³‚ê‚é‚Ü‚Å‚Ì‘Ò‹@ŽžŠÔ
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
