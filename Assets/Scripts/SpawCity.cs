using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawCity : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject Player;
    [SerializeField] GameObject[] City;
    bool Spawned = false;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.FindWithTag("Player").gameObject;
        if (Player.transform.position.z > transform.position.z + 270f)
        {
            Destroy(this.gameObject);
        }
        if (Player.transform.position.z > transform.position.z && !Spawned)
        {
            int value = Random.Range(0, 2);
            Debug.Log(value);
            Instantiate(City[value], new Vector3(transform.position.x, transform.position.y, transform.position.z + 257f), transform.rotation);
            Spawned = true;
        }
    }
}
