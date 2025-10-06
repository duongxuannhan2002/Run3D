using UnityEngine;

public static class CharacterManager
{
    public static bool IsPurchased(string id)
    {
        return PlayerPrefs.GetInt("character_" + id, 0) == 1;
    }

    public static void Purchase(string id)
    {
        PlayerPrefs.SetInt("character_" + id, 1);
        PlayerPrefs.Save();
    }
}
