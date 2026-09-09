using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("Không tìm thấy AudioManager!");
            return;
        }

        // Kiểm tra an toàn trước khi gán
        if (musicSlider != null)
        {
            musicSlider.SetValueWithoutNotify(AudioManager.Instance.GetMusicVolume());
        }
        else
        {
            Debug.LogWarning("Chưa gán Music Slider trên Inspector của SettingsPanel!");
        }

        if (sfxSlider != null)
        {
            sfxSlider.SetValueWithoutNotify(AudioManager.Instance.GetSFXVolume());
        }
        else
        {
            Debug.LogWarning("Chưa gán SFX Slider trên Inspector của SettingsPanel!");
        }
    }

    public void ChangeMusicVolume(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }
    }

    public void ChangeSFXVolume(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }
    }
}