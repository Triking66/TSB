using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throne : MonoBehaviour
{

    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerParent");
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - player.transform.position).magnitude < 10)
        {
            GetComponentInChildren<MeshCollider>().enabled = true;
        }
    }
}
