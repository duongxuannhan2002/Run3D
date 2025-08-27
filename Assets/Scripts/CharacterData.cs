using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    public string id;       // mã định danh nhân vật
    public string nameCharacter;     // tên nhân vật
    public int price;       // giá nhân vật
}
