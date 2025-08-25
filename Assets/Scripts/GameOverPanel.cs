using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TextBestScore;
    [SerializeField] TextMeshProUGUI TextScore;
    Transform Player;

    void Start()
    {
        Player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        TextBestScore.SetText(PlayerPrefs.GetFloat("BestScore", 0).ToString("00.0")+"m");
        TextScore.SetText(Player.transform.position.z.ToString("00.0") + "m");
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(0);
    }
}
