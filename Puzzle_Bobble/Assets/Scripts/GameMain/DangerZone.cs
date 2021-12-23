using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangerZone : MonoBehaviour
{
    // ��ʂ�Ԃ�����p�l��
    [SerializeField] private Image _dangerPanel;

    private GameManager gm;     // GameManager�̃C���X�^���X
    private Color startColor;   // �p�l���̏����F

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

        // �폜���o���Ȃ瑁�����^�[��
        if (GameManager.Instance.isBobbleDeleting) return;

        // �A���g���K�[�ɓ����Ă�����A��ʂ�Ԃ����点��
        if(0 < gm.dangerZoneContactCount)
        {
            // �p�l���̐F��Ԃ��_�ł�����i�����x��ς��Ă���j
            _dangerPanel.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Sin(Time.time * 7) / 4 + 0.05f);
            GameManager.Instance.isDangerTime = true;
        }
        else
        {
            // �p�l���𓧖��ɖ߂�
            _dangerPanel.color = Color.clear;
            StartCoroutine(nameof(LiftDangerTimeFlagCoroutine));
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bobble"))
        {
            gm.dangerZoneContactCount++;
            GameManager.Instance.isDangerTime = true;
        }
    }

    // ���̋ʂ����Ă�悤�ɂȂ��Ă���f���W���[�^�C���t���O��܂�
    private IEnumerator LiftDangerTimeFlagCoroutine()
    {
        // �ʂ̍폜���o���Ȃ珈�����~�߂�
        while (GameManager.Instance.isBobbleDeleting)
        {
            yield return new WaitForFixedUpdate();
        }

        // �ꔏ�u��
        yield return new WaitForSeconds(0.5f);

        // �f���W���[�]�[���ɖA�������Ă��Ȃ���΁A�f���W���[�^�C���t���O��܂�
        if(gm.dangerZoneContactCount <= 0)
        {
            GameManager.Instance.isDangerTime = false;
        }
        
    }
}
