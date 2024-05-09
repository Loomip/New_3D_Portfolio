using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : SingletonDontDestroy<SoundManager>
{
    [Header("BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    public int bgmChannels;
    AudioSource[] bgmPlayers;
    int bgmIndex;

    [Header("SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int sfxChannels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    void Init()
    {
        //����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayers = new AudioSource[bgmChannels];
        for (int i = 0; i < bgmPlayers.Length; ++i)
        {
            bgmPlayers[i] = bgmObject.AddComponent<AudioSource>();
            bgmPlayers[i].playOnAwake = true;
            bgmPlayers[i].loop = true;
            bgmPlayers[i].volume = bgmVolume;
        }
        

        //ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[sfxChannels];

        for (int i = 0; i < sfxPlayers.Length; ++i)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    public void PlayBgm(e_Bgm bgm)
    {
        for(int i = 0; i < bgmPlayers.Length; ++i)
        {
            int LoopIndex = (i + bgmIndex) % bgmPlayers.Length;

            if (bgmPlayers[LoopIndex].isPlaying)
                continue;

            bgmIndex = LoopIndex;
            bgmPlayers[LoopIndex].clip = bgmClips[(int)bgm];
            bgmPlayers[LoopIndex].Play();
            break;
        }
    }

    public void PlaySfx(e_Sfx sfx)
    {
        for(int i = 0; i < sfxPlayers.Length; ++i)
        {
            int LoopIndex = (i + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[LoopIndex].isPlaying)
                continue;

            int RanIndex = 0;
            if(sfx == e_Sfx.Sword)
            {
                RanIndex = Random.Range(0, 3);
            }
            if (sfx == e_Sfx.Hit)
            {
                RanIndex = Random.Range(0, 4);
            }

            channelIndex = LoopIndex;
            sfxPlayers[LoopIndex].clip = sfxClips[(int)sfx + RanIndex];
            sfxPlayers[LoopIndex].Play();
            break;
        }
    }

    public void StopSfx()
    {
        foreach (var sfxPlayer in sfxPlayers)
        {
            if (sfxPlayer.isPlaying)
            {
                sfxPlayer.Stop();
            }
        }
    }

    public void StopBgm()
    {
        foreach(var bgmPlayer in bgmPlayers)
        {
            if(bgmPlayer.isPlaying)
            {
                bgmPlayer.Stop();
            }
        }
    }

    //���Ұ� ������
    bool isMuted = false;

    public bool IsMuted()
    {
        return isMuted;
    }

    public void SetMute(bool mute)
    {
        isMuted = mute;

        foreach (var bgmPlayer in bgmPlayers)
        {
            bgmPlayer.mute = isMuted;
        }

        foreach (var sfxPlayer in sfxPlayers)
        {
            sfxPlayer.mute = isMuted;
        }
    }

    public float GetBgmVolume()
    {
        return bgmVolume; // �Ǵ� sfxVolume
    }
    public float GetSfxVolume()
    {
        return sfxVolume;
    }

    public void SetBgmVolume(float volume)
    {
        bgmVolume = volume;
        foreach (var bgmPlayer in bgmPlayers)
        {
            bgmPlayer.volume = bgmVolume;
        }
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
        foreach (var sfxPlayer in sfxPlayers)
        {
            sfxPlayer.volume = sfxVolume;
        }
    }

    protected override void DoAwake()
    {
        Init();
    }
}
