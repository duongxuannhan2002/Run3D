using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanelScript : MonoBehaviour
{
    private void Start()
    {
        UpdateUI();
    }
    public void OnClickRestart()
    {
        SceneManager.LoadScene(2);
    }

    public void OnClickContinue()
    {
        this.gameObject.SetActive(false);
        UIController.instance.PauseButton.SetActive(true);
        PlayerController.instance.IsGameStarted = true;
    }

    public void OnClickQuit()
    {
        SceneManager.LoadScene(0);
    }

  
    public void OnClickSound()
    {
        AudioManager.Instance.ToggleSFX();
        UpdateUI();
    }

    public void OnClickMusic()
    {
        AudioManager.Instance.ToggleMusic();
        UpdateUI();
    }

    public void UpdateUI()
    {
        Image ImgSoundButton = GameObject.Find("ButtonSound").GetComponent<Button>().image;
        Image ImgMusicButton = GameObject.Find("ButtonMusic").GetComponent<Button>().image;
        if (AudioManager.Instance.isMusicOn)
        {
            ImgMusicButton.color = Color.green;
        }
        else
        {
            ImgMusicButton.color = Color.red;
        }

        if (AudioManager.Instance.isSfxOn)
        {
            ImgSoundButton.color = Color.green;
        }
        else
        {
            ImgSoundButton.color = Color.red;
        }

    }
}
