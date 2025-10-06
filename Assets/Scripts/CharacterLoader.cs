using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    public GameObject[] characters; // danh sách nhân vật trong scene

    void Start()
    {
        int index = PlayerPrefs.GetInt("CharacterSelected", 0); // mặc định 0 nếu chưa chọn

        // Bật nhân vật đã chọn
        if (index >= 0 && index < characters.Length)
        {
            characters[index].SetActive(true);
        }
    }
}
