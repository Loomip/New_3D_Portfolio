using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Toggle muteToggle;
    [SerializeField] private TextMeshProUGUI bgmText;
    [SerializeField] private TextMeshProUGUI sfxText;
    [SerializeField] private TextMeshProUGUI muteText;
    [SerializeField] private TextMeshProUGUI exitText;

    private void Start()
    {
        //�ؽ�Ʈ ����
        bgmText.text = DataManager.instance.GetWordData("BGM");
        sfxText.text = DataManager.instance.GetWordData("SFX");
        muteText.text = DataManager.instance.GetWordData("Mute");
        if(exitText != null)
        {
            exitText.text = DataManager.instance.GetWordData("Exit");
        }

        // �ʱ� ���� ����
        bgmVolumeSlider.value = SoundManager.instance.GetBgmVolume();
        sfxVolumeSlider.value = SoundManager.instance.GetSfxVolume();
        muteToggle.isOn = SoundManager.instance.IsMuted();

        // ���� ���� �̺�Ʈ ���
        bgmVolumeSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        muteToggle.onValueChanged.AddListener(OnMuteChanged);
    }

    private void OnBgmVolumeChanged(float value)
    {
        SoundManager.instance.SetBgmVolume(value);
    }

    private void OnSfxVolumeChanged(float value)
    {
        SoundManager.instance.SetSfxVolume(value);
    }

    private void OnMuteChanged(bool isMuted)
    {
        SoundManager.instance.SetMute(isMuted);
    }

    // ���ø����̼� ����
    public void Clear()
    {
        Application.Quit();
    }
}
