using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField] TextMeshProUGUI TextCoin;
    [SerializeField] TextMeshProUGUI TextScore;
    public TextMeshProUGUI countdownText;

    public GameObject TextStart;
    Transform Player;
    public GameObject PausePanel;
    public GameObject PauseButton;
    public GameObject StartButton;
    public GameObject ImageMagnet;
    public GameObject ImageShied;
    public GameObject ImageWings;
    public GameObject ImageTemp;
    public GameObject RolePanel;
    //public GameObject targetObject;
    private Coroutine blinkMagnetCoroutine;
    private Coroutine blinkShieldCoroutine;
    private Coroutine blinkWingsCoroutine;


    private void Awake()
    {
        instance=this;
    }

    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        if(PlayerPrefs.GetInt("FirstPlay",0) == 0)
        {
            RolePanel.SetActive(true);
            StartButton.SetActive(false);
        }
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
        if (PlayerController.hasMagnet)
        {
            if(!ImageMagnet.activeSelf) ImageMagnet.SetActive(true);
        }
        else
        {
            if(ImageMagnet.activeSelf) ImageMagnet.SetActive(false);
        }
        if (PlayerController.hasShied)
        {
            if (!ImageShied.activeSelf) ImageShied.SetActive(true);
        }
        else
        {
            if (ImageShied.activeSelf) ImageShied.SetActive(false);
        }
        if (PlayerController.isFly)
        {
            if (!ImageWings.activeSelf) ImageWings.SetActive(true);
        }
        else
        {
            if (ImageWings.activeSelf) ImageWings.SetActive(false);
        }
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

    public void OnClickHelp()
    {
        PausePanel.SetActive(false);
        RolePanel.SetActive(true);
    }

    public void GameStart()
    {
        PauseButton.SetActive(true);
        TextStart.SetActive(false);
    }

    public void OnClickOk()
    {
        RolePanel.SetActive(false);
        StartButton.SetActive(true);
        foreach (var text in StartButton.GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            text.gameObject.SetActive(true);
            text.ForceMeshUpdate();
        }
        PlayerPrefs.SetInt("FirstPlay", 1);
    }

    public void StartBlink(float duration, int numItem, float interval = 0.3f)
    {
        if (numItem == 0)
        {
            if (blinkMagnetCoroutine != null) StopCoroutine(blinkMagnetCoroutine);
            blinkMagnetCoroutine = StartCoroutine(BlinkRoutine(ImageMagnet, duration, interval, () => blinkMagnetCoroutine = null));
        }
        else if(numItem == 1)
        {
            if (blinkShieldCoroutine != null) StopCoroutine(blinkShieldCoroutine);
            blinkShieldCoroutine = StartCoroutine(BlinkRoutine(ImageShied, duration, interval, () => blinkShieldCoroutine = null));
        }
        else
        {
            if (blinkWingsCoroutine != null) StopCoroutine(blinkWingsCoroutine);
            blinkWingsCoroutine = StartCoroutine(BlinkRoutine(ImageWings, duration, interval, () => blinkWingsCoroutine = null));
        }
    }

    public void StopBlink(int numItem)
    {
        if (numItem == 0)
        {
            if (blinkMagnetCoroutine != null) StopCoroutine(blinkMagnetCoroutine);
            ImageMagnet.GetComponent<UnityEngine.UI.Image>().enabled = true;
            blinkMagnetCoroutine = null;
        }
        else if (numItem == 1)
        {
            if (blinkShieldCoroutine != null) StopCoroutine(blinkShieldCoroutine);
            ImageShied.GetComponent<UnityEngine.UI.Image>().enabled = true;
            blinkShieldCoroutine = null;
        }
        else
        {
            if (blinkWingsCoroutine != null) StopCoroutine(blinkWingsCoroutine);
            ImageWings.GetComponent<UnityEngine.UI.Image>().enabled = true;
            blinkWingsCoroutine = null;
        }
    }

    private IEnumerator BlinkRoutine(GameObject target, float duration, float interval, Action onComplete)
    {
        float timer = 0f;
        bool visible = true;

        while (timer < duration)
        {
            visible = !visible;
            if (target != null) target.GetComponent<UnityEngine.UI.Image>().enabled = visible;

            yield return new WaitForSeconds(interval);
            timer += interval;
        }

        if (target != null) target.GetComponent<UnityEngine.UI.Image>().enabled = true;
        onComplete?.Invoke();
    }
    public IEnumerator CountdownBeforeStart()
    {
        int count = 3;

        countdownText.gameObject.SetActive(true);

        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSecondsRealtime(1f);
            count--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.gameObject.SetActive(false);

        PlayerController.instance.IsGameStarted = true;
    }
}
