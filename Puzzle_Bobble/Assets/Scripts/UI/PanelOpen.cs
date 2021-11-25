using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpen : MonoBehaviour
{
    /// <summary>
    /// �A�N�e�B�u�ɂ���UI�I�u�W�F�N�g
    /// </summary>
    [SerializeField] private GameObject _openPanel;

    /// <summary>
    /// ��A�N�e�B�u�ɂ���UI
    /// </summary>
    [SerializeField] private GameObject _closePanel;

    public void Open()
    {
        if(_openPanel != null) _openPanel.SetActive(true);

        if (_closePanel != null) _closePanel.SetActive(false);
    }
}
