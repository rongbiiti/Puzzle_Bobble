using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangerZone : MonoBehaviour
{
    // ��ʂ�Ԃ�����p�l��
    [SerializeField] private Image _dangerPanel;

    private GameManager gm;     // GameManager�̃C���X�^���X
    private int preContactCount;    // �O�̃t���[����GameManager��dangerZoneContactCount
    private Color startColor;
    private int contactCount;   // ���̃I�u�W�F�N�g�̃g���K�[�͈͂ƐڐG���Ă���A�̌�

    void Start()
    {
        // �C���X�^���X�擾���Ă���
        gm = GameManager.Instance;

        // �p�l���̐F���擾
        startColor = _dangerPanel.color;

        // �����ɂ��Ă���
        _dangerPanel.color = Color.clear;
    }

    private void FixedUpdate()
    {
        // �Q�[�����I��������A��ʂ�Ԃ����点�Ȃ��悤�ɂ���
        if (GameManager.Instance.isBobbleFalloutGameOverZone)
        {
            _dangerPanel.color = Color.clear;
            this.enabled = false;
            return;
        }

        // �A���g���K�[�ɓ����Ă�����A��ʂ�Ԃ����点��
        if(0 < gm.dangerZoneContactCount)
        {
            _dangerPanel.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Sin(Time.time * 7) / 4 + 0.05f);
            GameManager.Instance.isDangerTime = true;
        }
        else
        {
            _dangerPanel.color = Color.clear;
            if(preContactCount != gm.dangerZoneContactCount)
            {
                StartCoroutine(nameof(LiftDangerTimeFlagCoroutine));
            }            
        }

        preContactCount = gm.dangerZoneContactCount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bobble"))
        {
            //contactCount++;
            //GameManager.Instance.isDangerTime = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Bobble"))
        {
            //contactCount--;
        }

        if (contactCount <= 0)
        {
            //_dangerPanel.color = Color.clear;
            //StartCoroutine(nameof(LiftDangerTimeFlagCoroutine));
        }
    }

    // ���̋ʂ����Ă�悤�ɂȂ��Ă���f���W���[�^�C���t���O��܂�
    private IEnumerator LiftDangerTimeFlagCoroutine()
    {
        while (GameManager.Instance.isBobbleDeleting)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.isDangerTime = false;
    }
}
