using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGM
{
    Main,

    Max
}

public enum SE
{
    WallReflect,
    BobbleDelete,
    CannonAiming,
    CannonFire,
    BobbleFall,
    BobbleSeted,

    Max
}

public enum SysSE
{
    Touch,
    GameStart,

    MAX
}

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    // ����
    public SoundVolume volume = new SoundVolume();

    // === AudioSource ===
    // BGM
    private AudioSource BGMsource;
    // SE
    private AudioSource[] SEsources = new AudioSource[32];
    // ����
    private AudioSource[] SystemSEsources = new AudioSource[16];

    // === AudioClip ===
    // BGM
    public AudioClip[] BGM;
    // SE
    public AudioClip[] SE;
    // ����
    public AudioClip[] SystemSE;


    protected override void Awake()
    {
        base.Awake();

        // �V�[���J�ڎ���Destroy����Ȃ��悤�ɂ���
        DontDestroyOnLoad(gameObject);

        // BGM AudioSource
        BGMsource = gameObject.AddComponent<AudioSource>();
        // BGM�̓��[�v��L���ɂ���
        BGMsource.loop = true;

        // SE AudioSource
        for (int i = 0; i < SEsources.Length; i++)
        {
            SEsources[i] = gameObject.AddComponent<AudioSource>();
        }

        // ���� AudioSource
        for (int i = 0; i < SystemSEsources.Length; i++)
        {
            SystemSEsources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // �~���[�g�ݒ�
        BGMsource.mute = volume.Mute;
        foreach (AudioSource source in SEsources)
        {
            source.mute = volume.Mute;
        }
        foreach (AudioSource source in SystemSEsources)
        {
            source.mute = volume.Mute;
        }

        // �{�����[���ݒ�
        BGMsource.volume = volume.BGM;
        foreach (AudioSource source in SEsources)
        {
            source.volume = volume.SE;
        }
        foreach (AudioSource source in SystemSEsources)
        {
            source.volume = volume.SystemSE;
        }


    }

    // ***** BGM�Đ� *****
    // BGM�Đ�
    public void PlayBGM(BGM index)
    {
        if (0 > index || BGM.Length <= (int)index)
        {
            return;
        }
        // ����BGM�̏ꍇ�͉������Ȃ�
        if (BGMsource.clip == BGM[(int)index])
        {
            return;
        }
        BGMsource.Stop();
        BGMsource.clip = BGM[(int)index];
        BGMsource.Play();
    }

    // BGM�ꎞ��~
    public void PauseBGM()
    {
        BGMsource.Pause();
    }

    // BGM�Đ��ĊJ
    public void UnPauseBGM()
    {
        BGMsource.UnPause();
    }

    // BGM��~
    public void StopBGM()
    {
        BGMsource.Stop();
        BGMsource.clip = null;
    }


    // ***** SE�Đ� *****
    // SE�Đ�
    public void PlaySE(SE index)
    {
        if (0 > index || SE.Length <= ((int)index))
        {
            return;
        }

        // �Đ����Ŗ���AudioSouce�Ŗ炷
        foreach (AudioSource source in SEsources)
        {
            if (false == source.isPlaying)
            {
                source.clip = SE[((int)index)];
                source.Play();
                return;
            }
        }
    }

    // SE�ꎞ��~
    public void PauseSE()
    {
        foreach (AudioSource source in SEsources)
        {
            source.Pause();
        }
    }

    // SE�Đ��ĊJ
    public void UnPauseSE()
    {
        foreach (AudioSource source in SEsources)
        {
            source.UnPause();
        }
    }

    // SE��~
    public void StopSE()
    {
        // �S�Ă�SE�p��AudioSouce���~����
        foreach (AudioSource source in SEsources)
        {
            source.Stop();
            source.clip = null;
        }
    }


    // ***** �����Đ� *****
    // �����Đ�
    public void PlaySystemSE(SysSE index)
    {
        if (0 > index || SystemSE.Length <= ((int)index))
        {
            return;
        }
        // �Đ����Ŗ���AudioSouce�Ŗ炷
        foreach (AudioSource source in SystemSEsources)
        {
            if (false == source.isPlaying)
            {
                source.clip = SystemSE[((int)index)];
                source.Play();
                return;
            }
        }
    }

    // ������~
    public void StopSystemSE()
    {
        // �S�Ẳ����p��AudioSouce���~����
        foreach (AudioSource source in SystemSEsources)
        {
            source.Stop();
            source.clip = null;
        }
    }

    // �t�F�[�h�A�E�g�R���[�`���Ăяo��
    public void BGMFadeOut(float fadeOutTime)
    {
        StartCoroutine(FadeOut(fadeOutTime));
    }

    // �t�F�[�h�A�E�g
    private IEnumerator FadeOut(float fadeOutTime)
    {
        float currentTime = 0.0f;
        float firstVol = BGMsource.volume;

        // ���ʂ����X�ɉ�����
        while(fadeOutTime > currentTime)
        {
            currentTime += Time.fixedDeltaTime;
            BGMsource.volume = Mathf.Clamp01(firstVol * (fadeOutTime - currentTime) / fadeOutTime);
            yield return new WaitForFixedUpdate();
        }

        // 0�ɉ�������
        BGMsource.volume = 0;
        // BGM��~
        StopBGM();

        // ��u�҂��Ă��特�ʂ�߂�
        yield return new WaitForFixedUpdate();
        BGMsource.volume = firstVol;
    }
}

// ���ʃN���X
[Serializable]
public class SoundVolume
{
    public float BGM = 1.0f;
    public float SE = 1.0f;
    public float SystemSE = 1.0f;
    public bool Mute = false;

    public void Init()
    {
        BGM = 1.0f;
        SE = 1.0f;
        SystemSE = 1.0f;
        Mute = false;
    }
}
