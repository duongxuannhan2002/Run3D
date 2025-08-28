using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TextBestScore;
    [SerializeField] TextMeshProUGUI TextScore;
    Transform Player;
    private List<float> highScores = new List<float>();
    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        LoadScores();
        AddScore(Player.transform.position.z);
        if (highScores.Count > 0)
            TextBestScore.SetText(highScores[0].ToString("00.0") + "m");
        else
            TextBestScore.SetText("00.0m");

        TextScore.SetText(Player.transform.position.z.ToString("00.0") + "m");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(float newScore)
    {
        highScores.Add(newScore);

        // Sắp xếp giảm dần
        highScores = highScores.OrderByDescending(s => s).ToList();

        // Chỉ giữ 5 kỷ lục cao nhất
        if (highScores.Count > 5)
        {
            highScores = highScores.Take(5).ToList();
        }

        SaveScores();
    }

    // Lưu vào PlayerPrefs
    private void SaveScores()
    {
        for (int i = 0; i < highScores.Count; i++)
        {
            PlayerPrefs.SetFloat("BestScore" + i, highScores[i]);
        }
        PlayerPrefs.Save();
    }

    // Tải lại điểm
    public void LoadScores()
    {
        highScores.Clear();
        for (int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.HasKey("BestScore" + i))
                highScores.Add(PlayerPrefs.GetFloat("BestScore" + i));
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(2);
    }
}
