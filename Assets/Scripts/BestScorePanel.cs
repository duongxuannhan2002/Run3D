using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HighScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] rankTexts; // Drag 5 Text vào Inspector
    private List<float> highScores = new List<float>();

    void Start()
    {
        LoadScores();
        ShowScores();
    }

    // Load điểm từ PlayerPrefs
    private void LoadScores()
    {
        highScores.Clear();
        for (int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.HasKey("BestScore" + i))
                highScores.Add(PlayerPrefs.GetFloat("BestScore" + i));
        }
    }

    // Hiển thị lên UI
    private void ShowScores()
    {
        for (int i = 0; i < rankTexts.Length; i++)
        {
            if (i < highScores.Count)
                rankTexts[i].SetText(highScores[i].ToString("00.0") + "m");
            else
                rankTexts[i].SetText("---");
        }
    }

    public void OnClickBack()
    {
        Debug.Log("hello");
        SceneManager.LoadScene(0);
    }
}
