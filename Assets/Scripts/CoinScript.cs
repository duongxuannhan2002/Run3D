using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    GameObject player;
    private void Start()
    {
    }
    void Update()
    {
        transform.Rotate(0, 100f * Time.deltaTime, 0);
        player = GameObject.FindGameObjectWithTag("Player");
        if(PlayerController.hasMagnet)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance < 10f)
            {
                // Coin bay về phía player
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    player.transform.position,
                    100f * Time.deltaTime
                );
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            AudioManager.Instance.PlaySoundCoin();
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin", 0)+1);
            Destroy(this.gameObject);
        }
    }
}
