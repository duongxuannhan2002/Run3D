using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSwitcher : MonoBehaviour
{
    [Header("Danh sách prefab nhân vật")]
    public GameObject[] playerPrefabs;

    private GameObject currentPlayer;
    private int currentIndex = 0;
    public CharacterData[] data;
    public Button PlayButton;
    public Button BuyButton;
    public TextMeshProUGUI CharacterName;
    public TextMeshProUGUI Coin;
    

    void Start()
    {
        //PlayerPrefs.SetInt("character_" + 0, 0);
        //PlayerPrefs.SetInt("character_" + 1, 0);
        currentIndex = PlayerPrefs.GetInt("CharacterSelected", 0);
        // Khởi tạo nhân vật đầu tiên
        SpawnPlayer(currentIndex);
        UpdateUI();
    }

    void SpawnPlayer(int index)
    {
        // Lấy vị trí + hướng của nhân vật cũ (nếu có)
        
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        // Tạo nhân vật mới
        currentPlayer = Instantiate(playerPrefabs[index], playerPrefabs[index].transform.position, playerPrefabs[index].transform.rotation);
    }

    public void NextCharacter()
    {
        currentIndex++;
        if (currentIndex >= playerPrefabs.Length)
            currentIndex = 0; // quay lại nhân vật đầu tiên

        SpawnPlayer(currentIndex);
        UpdateUI();
    }

    public void PreviousCharacter()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = playerPrefabs.Length - 1; // quay lại nhân vật cuối cùng

        SpawnPlayer(currentIndex);
        UpdateUI();
    }

    public void SwitchToCharacter()
    {
        PlayerPrefs.SetInt("CharacterSelected", currentIndex);
        SceneManager.LoadScene(2);
    }

    void UpdateUI()
    {
        Coin.SetText(PlayerPrefs.GetInt("Coin", 0).ToString());
        CharacterName.SetText(data[currentIndex].nameCharacter);
        if (CharacterManager.IsPurchased(data[currentIndex].id))
        {
            PlayButton.gameObject.SetActive(true);
            BuyButton.gameObject.SetActive(false);
        }
        else
        {
            PlayButton.gameObject.SetActive(false);
            BuyButton.gameObject.SetActive(true);
            BuyButton.GetComponentInChildren<TextMeshProUGUI>().SetText(data[currentIndex].price.ToString());
        }
    }

    public void OnBuyButton()
    {
        if (!CharacterManager.IsPurchased(data[currentIndex].id)&& PlayerPrefs.GetInt("Coin", 0)>= data[currentIndex].price)
        {
            // trừ tiền player
            CharacterManager.Purchase(data[currentIndex].id);
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin", 0) - data[currentIndex].price);
            UpdateUI();
        }
    }

    public void OnClickBack()
    {
        SceneManager.LoadScene(0);
    }
}
