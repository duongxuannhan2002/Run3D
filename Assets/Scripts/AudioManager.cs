using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip bgm;
    public AudioClip jumpSound;
    public AudioClip fallSound;
    public AudioClip coinSound;
    public AudioClip clickSound;
    public AudioClip itemSound;

    public bool isMusicOn;
    public bool isSfxOn;

    private void Awake()
    {
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

        isMusicOn = PlayerPrefs.GetInt("Music", 1) == 1;
        isSfxOn = PlayerPrefs.GetInt("SFX", 1) == 1;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlayMusic(bgm);
        RegisterAllButtons();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RegisterAllButtons();
    }

    private void RegisterAllButtons()
    {
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(PlaySoundClick);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        if (isMusicOn)
            musicSource.Play();
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        PlayerPrefs.SetInt("Music", isMusicOn ? 1 : 0);
        if (isMusicOn) musicSource.Play();
        else musicSource.Stop();
    }

    public void ToggleSFX()
    {
        isSfxOn = !isSfxOn;
        PlayerPrefs.SetInt("SFX", isSfxOn ? 1 : 0);
    }

    public void PlaySoundJump() { if (isSfxOn) sfxSource.PlayOneShot(jumpSound); }
    public void PlaySoundFall() { if (isSfxOn) sfxSource.PlayOneShot(fallSound); }
    public void PlaySoundCoin() { if (isSfxOn) sfxSource.PlayOneShot(coinSound); }
    public void PlaySoundClick() { if (isSfxOn) sfxSource.PlayOneShot(clickSound); }
    public void PlaySoundCollectItem() { if (isSfxOn) sfxSource.PlayOneShot(itemSound); }
}
