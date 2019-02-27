using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10;
    public int damage = 5;
    public float push = 4;
    private float destroytime = 4;
    float initialtime;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        initialtime = Time.time;
        player = GameObject.FindGameObjectWithTag("Player").transform.root.gameObject;      
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - initialtime > destroytime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 dir = collision.GetContact(0).point - transform.position;
            dir = dir.normalized;
            player.GetComponent<PlayerController>().dealDamage(damage, Vector3.zero);
            player.GetComponent<Rigidbody>().AddForce(dir * push,ForceMode.VelocityChange);
            Destroy(gameObject);
        }
        else
        {
            damage = 0;
        }
    }
}
