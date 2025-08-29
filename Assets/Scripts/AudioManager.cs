using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;   // Nhạc nền
    public AudioSource sfxSource;     // Hiệu ứng

    [Header("Audio Clips")]
    public AudioClip bgm;
    public AudioClip jumpSound;
    public AudioClip fallSound;
    public AudioClip coinSound;

    // Trạng thái bật/tắt
    public bool isMusicOn;
    public bool isSfxOn;

    private void Awake()
    {
        // Singleton
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

        // Load trạng thái từ PlayerPrefs
        isMusicOn = PlayerPrefs.GetInt("Music", 1) == 1;
        isSfxOn = PlayerPrefs.GetInt("SFX", 1) == 1;
    }

    private void Start()
    {
        PlayMusic(bgm);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        if (isMusicOn)
            musicSource.Play();
    }

    //public void PlaySFX(AudioClip clip)
    //{
    //    if (isSfxOn)
    //        sfxSource.PlayOneShot(clip);
    //}

    // Toggle Music
    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        PlayerPrefs.SetInt("Music", isMusicOn ? 1 : 0);
        if (isMusicOn)
        {
            musicSource.Play();
        }
        else
        {
            musicSource.Stop();
        }

    }

    // Toggle SFX
    public void ToggleSFX()
    {
        isSfxOn = !isSfxOn;
        PlayerPrefs.SetInt("SFX", isSfxOn ? 1 : 0);
    }

    public void PlaySoundJump()
    {
        if (isSfxOn)
        {
            sfxSource.PlayOneShot(jumpSound);
        }
    }

    public void PlaySoundFall()
    {
        if (isSfxOn)
        {
            sfxSource.PlayOneShot(fallSound);
        }
    }

    public void PlaySoundCoin()
    {
        if (isSfxOn)
        {
            sfxSource.PlayOneShot(coinSound);
        }
    }
}
