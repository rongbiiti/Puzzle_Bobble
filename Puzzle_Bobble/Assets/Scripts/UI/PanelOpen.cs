using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpen : MonoBehaviour
{
    /// <summary>
    /// アクティブにするUIオブジェクト
    /// </summary>
    [SerializeField] private GameObject _openPanel;

    /// <summary>
    /// 非アクティブにするUI
    /// </summary>
    [SerializeField] private GameObject _closePanel;

    public void Open()
    {
        if(_openPanel != null) _openPanel.SetActive(true);

        if (_closePanel != null) _closePanel.SetActive(false);
    }
}
