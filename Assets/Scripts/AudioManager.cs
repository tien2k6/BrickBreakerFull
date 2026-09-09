using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        // Tạo AudioManager duy nhất
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Nếu chưa từng lưu âm lượng
        // thì mặc định âm lượng = 100%
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1f);
        }

        if (!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("SFXVolume", 1f);
        }

        PlayerPrefs.Save();

        // Lấy âm lượng đã lưu
        LoadVolume();
    }

    private void Start()
    {
        // Tự động phát nhạc nền
        if (musicSource != null)
        {
            musicSource.loop = true;

            if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }
    }

    // =========================
    // CHỈNH ÂM LƯỢNG NHẠC
    // =========================
    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }

        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    // =========================
    // CHỈNH ÂM THANH HIỆU ỨNG
    // =========================
    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
        }

        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    // =========================
    // PHÁT ÂM THANH HIỆU ỨNG
    // =========================
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // =========================
    // LẤY ÂM LƯỢNG ĐÃ LƯU
    // =========================
    private void LoadVolume()
    {
        float musicVolume =
            PlayerPrefs.GetFloat("MusicVolume", 1f);

        float sfxVolume =
            PlayerPrefs.GetFloat("SFXVolume", 1f);

        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }

        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
}