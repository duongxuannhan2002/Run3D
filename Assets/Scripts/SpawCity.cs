using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawCity : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject Player;
    [SerializeField] GameObject[] City;
    bool Spawned = false;
    public static int value;

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.FindWithTag("Player").gameObject;
        if (Player.transform.position.z > transform.position.z + 530f)
        {
            Destroy(this.gameObject);
        }
        if (Player.transform.position.z > transform.position.z && !Spawned)
        {
            value++;
            Instantiate(City[value], new Vector3(transform.position.x, transform.position.y, transform.position.z + 519f), transform.rotation);
            Spawned = true;
            if (value == 2) value = -1;
        }
    }
}
