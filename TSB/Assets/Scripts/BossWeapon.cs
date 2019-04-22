using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponentInParent<HeroBehaviour>().health <= 0)
        {
            //GetComponent<MeshCollider>().enabled = false;
            transform.parent = null;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponentInParent<HeroBehaviour>().damage = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().dealDamage(GetComponentInParent<HeroBehaviour>().damage, transform.forward);
        }
    }
}
