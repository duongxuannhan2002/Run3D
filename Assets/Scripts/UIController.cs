using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField] TextMeshProUGUI TextCoin;
    [SerializeField] TextMeshProUGUI TextScore;
    public GameObject TextStart;
    Transform Player;
    public GameObject PausePanel;
    public GameObject PauseButton;
    public GameObject StartButton;

    private void Awake()
    {
        instance=this;
    }

    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        TextCoin.SetText(PlayerPrefs.GetInt("Coin",0).ToString());
        TextScore.SetText(Player.transform.position.z.ToString("00.0") + "m");
        //if(Player.transform.position.z> PlayerPrefs.GetFloat("BestScore", 0))
        //{
        //    PlayerPrefs.SetFloat("BestScore", Player.transform.position.z);
        //}
    }
    // Start is called before the first frame update
    public void OnclickPause()
    {
        PlayerController.instance.IsGameStarted = false;
        PauseButton.SetActive(false);
        PausePanel.SetActive(true);
    }

    public void OnclickBack()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClickStart()
    {
        PlayerController.instance.StartGame();
        StartButton.SetActive(false );
    }

    public void GameStart()
    {
        PauseButton.SetActive(true);
        TextStart.SetActive(false);
    }
}
