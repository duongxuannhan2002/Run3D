using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationManger : MonoBehaviour
{
    public static NotificationManger Instance;
    [SerializeField] private GameObject notifiactionPrefab;
    [SerializeField] private Transform notificationParent;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    public void ShowNotification(string message, float duration)
    {
        GameObject notif = Instantiate(notifiactionPrefab, notificationParent);
        notif.GetComponentInChildren<TextMeshProUGUI>().SetText(message);
        Destroy(notif,duration);
    }
}
