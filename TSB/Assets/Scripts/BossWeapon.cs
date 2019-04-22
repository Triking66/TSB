using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    private bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(dead);
        if (!dead)
        {
            if (gameObject.transform.root.GetComponent<HeroBehaviour>().health <= 0)
            {
                //GetComponent<MeshCollider>().enabled = false;
                gameObject.transform.root.GetComponent<HeroBehaviour>().damage = 0;

                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                dead = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!dead)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerController>().dealDamage(GetComponentInParent<HeroBehaviour>().damage, transform.forward);
            }
        }
    }
}
