using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TextCoin;
    [SerializeField] TextMeshProUGUI TextScore;
    Transform Player;

    void Start()
    {
        Player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        TextCoin.SetText(PlayerPrefs.GetInt("Coin",0).ToString());
        TextScore.SetText(Player.transform.position.z.ToString("00.0") + "m");
        if(Player.transform.position.z> PlayerPrefs.GetFloat("BestScore", 0))
        {
            PlayerPrefs.SetFloat("BestScore", Player.transform.position.z);
        }
    }
}
