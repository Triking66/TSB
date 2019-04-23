using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Pickup : MonoBehaviour
{

    [SerializeField] private GameObject item;
    private GameObject player;
    private bool trying = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !trying)
        {
            trying = true;
            player = collision.gameObject;
            if (player.GetComponent<PlayerController>().pick_up(item))
            {
                Destroy(gameObject);
            }
            trying = false;
        }
    }
}
