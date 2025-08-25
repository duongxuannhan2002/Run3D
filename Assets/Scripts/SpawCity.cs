using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawCity : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject Player;
    [SerializeField] GameObject City;
    bool Spawned = false;
    void Start()
    {
        Player = GameObject.Find("Player").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.transform.position.z > transform.position.z + 257f)
        {
            Destroy(this.gameObject);
        }
        if (Player.transform.position.z > transform.position.z && !Spawned)
        {
            Instantiate(City, new Vector3(transform.position.x, transform.position.y, transform.position.z + 257f), transform.rotation);
            Spawned = true;
        }
    }
}
