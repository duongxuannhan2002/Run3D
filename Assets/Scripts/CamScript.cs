using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Player;
    void Start()
    {
        Player = GameObject.FindWithTag("Player").gameObject.transform;
    }
    // Update is called once per frame
    private void LateUpdate()
    {
        transform.position = new Vector3(Player.position.x,Player.position.y+6f,Player.position.z-8f);
    }
}
