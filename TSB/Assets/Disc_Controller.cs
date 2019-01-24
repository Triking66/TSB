using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc_Controller : MonoBehaviour {

    [SerializeField] private float speed = 30;
    [SerializeField] private float invincible_time = 1;
    [SerializeField] private int damage = 50;

    public bool can_pick_up = false;
    private bool hit_wall = false;
    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
	}
	
	// Update is called once per frame
	void Update () {
        if (invincible_time > 0)
        {
            invincible_time -= Time.deltaTime;
            if (invincible_time <= 0)
            {
                can_pick_up = true;
            }
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !hit_wall)
        {
            hit_wall = true;
            can_pick_up = true;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }
        
        else if (collision.gameObject.CompareTag("Enemy") && !hit_wall) 
        {
            collision.gameObject.GetComponent<EnemyController>().dealDamage(damage);
            hit_wall = true;
            can_pick_up = true;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}
