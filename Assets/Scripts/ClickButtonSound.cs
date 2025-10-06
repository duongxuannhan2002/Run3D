using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour, IPointerDownHandler
{ 
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerPress == gameObject)
        {
            AudioManager.Instance.PlaySoundClick();
        }
    }
}