using UnityEngine;
using System.Collections.Generic;
using System;

public class Pauser : MonoBehaviour
{
    public static List<Pauser> targets = new List<Pauser>();   // �|�[�Y�Ώۂ̃X�N���v�g

    public static bool isPaused;            // �|�[�Y�{�^���������Ẵ|�[�Y����
    public static bool isCanPausing = true;     // �|�[�Y�{�^���������Ẵ|�[�Y�����Ă�������

    // �|�[�Y�Ώۂ̃R���|�[�l���g
    Behaviour[] pauseBehavs = null;

    Rigidbody[] rgBodies = null;
    Vector3[] rgBodyVels = null;
    Vector3[] rgBodyAVels = null;

    Rigidbody2D[] rg2dBodies = null;
    Vector2[] rg2dBodyVels = null;
    float[] rg2dBodyAVels = null;

    // ������
    void Awake()
    {
        // �|�[�Y�Ώۂɒǉ�����
        targets.Add(this);
    }

    private void OnDestroy()
    {
        // �|�[�Y�Ώۂ��珜�O����
        targets.Remove(this);
    }

    // �|�[�Y���ꂽ�Ƃ�
    void OnPause()
    {
        if (!ReferenceEquals(pauseBehavs, null))
        {
            return;
        }

        // �L���ȃR���|�[�l���g���擾
        if (ReferenceEquals(pauseBehavs, null))
            pauseBehavs = Array.FindAll(GetComponentsInChildren<Behaviour>(), (obj) => { return obj.enabled; });
        foreach (var com in pauseBehavs)
        {
            com.enabled = false;
        }

        rgBodies = Array.FindAll(GetComponentsInChildren<Rigidbody>(), (obj) => { return !obj.IsSleeping(); });
        rgBodyVels = new Vector3[rgBodies.Length];
        rgBodyAVels = new Vector3[rgBodies.Length];
        for (var i = 0; i < rgBodies.Length; ++i)
        {
            rgBodyVels[i] = rgBodies[i].velocity;
            rgBodyAVels[i] = rgBodies[i].angularVelocity;
            rgBodies[i].Sleep();
        }

        rg2dBodies = Array.FindAll(GetComponentsInChildren<Rigidbody2D>(), (obj) => { return !obj.IsSleeping(); });
        rg2dBodyVels = new Vector2[rg2dBodies.Length];
        rg2dBodyAVels = new float[rg2dBodies.Length];
        for (var i = 0; i < rg2dBodies.Length; ++i)
        {
            rg2dBodyVels[i] = rg2dBodies[i].velocity;
            rg2dBodyAVels[i] = rg2dBodies[i].angularVelocity;
            rg2dBodies[i].Sleep();
        }


    }

    // �|�[�Y�������ꂽ�Ƃ�
    void OnResume()
    {
        if (pauseBehavs == null)
        {
            return;
        }

        // �|�[�Y�O�̏�ԂɃR���|�[�l���g�̗L����Ԃ𕜌�
        foreach (var com in pauseBehavs)
        {
            com.enabled = true;
        }

        for (var i = 0; i < rgBodies.Length; ++i)
        {
            rgBodies[i].WakeUp();
            rgBodies[i].velocity = rgBodyVels[i];
            rgBodies[i].angularVelocity = rgBodyAVels[i];
        }

        for (var i = 0; i < rg2dBodies.Length; ++i)
        {
            rg2dBodies[i].WakeUp();
            rg2dBodies[i].velocity = rg2dBodyVels[i];
            rg2dBodies[i].angularVelocity = rg2dBodyAVels[i];
        }

        pauseBehavs = null;

        rgBodies = null;
        rgBodyVels = null;
        rgBodyAVels = null;

        rg2dBodies = null;
        rg2dBodyVels = null;
        rg2dBodyAVels = null;


    }    

    // �����|�[�Y
    public static void Pause()
    {
        if (!isCanPausing) return;

        foreach (var obj in targets)
        {
            obj.OnPause();
        }
        isPaused = true;
        Time.timeScale = 0f;
    }

    // �����|�[�Y����
    public static void Resume()
    {
        foreach (var obj in targets)
        {
            obj.OnResume();
        }
        isPaused = false;
        Time.timeScale = 1f;
    }
}