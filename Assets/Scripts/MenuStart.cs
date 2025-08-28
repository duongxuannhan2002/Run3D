using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStart : MonoBehaviour
{
    // Start is called before the first frame update
    

    // Update is called once per frame
    public void OnClickPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void OnClickSetting()
    {
        SceneManager.LoadScene(3);
    }

    public void OnClickRank()
    {
        SceneManager.LoadScene(4);
    }
    public void OnClickQuit()
    {
        Application.Quit();
    }
    public void ResetGameData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
