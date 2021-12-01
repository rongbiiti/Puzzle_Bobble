using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSE : MonoBehaviour
{
    [SerializeField] private SysSE _playSysSE;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void PlayButtonSE()
    {
        SoundManager.Instance.PlaySystemSE(_playSysSE);
    }
}
