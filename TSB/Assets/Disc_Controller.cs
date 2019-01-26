using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc_Controller : MonoBehaviour {

    [SerializeField] private float speed = 30;
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
            hit_wall = true;
            collision.gameObject.GetComponent<EnemyController>().dealDamage(damage);
            can_pick_up = true;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}
