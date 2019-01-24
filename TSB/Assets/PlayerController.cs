using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] GameObject weapon;
    [SerializeField] private int num_disc;

    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private int maxHP = 100;
    //[SerializeField] private float friction;

    private Rigidbody rb;
    private int cur_disc;
    private int health;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        cur_disc = num_disc;
        health = maxHP;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        var dir = Vector3.zero;
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");

        if(dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                Time.deltaTime * rotateSpeed);
        }
        rb.AddForce(dir * speed);
        //var frict = new Vector3(-rb.velocity.x * (1-friction), 0, -rb.velocity.z * (1-friction));
        //rb.AddForce(frict);
	}

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && cur_disc > 0)
        {
            var screenclicked = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var newdisc = Instantiate(weapon, transform.position, transform.rotation);
            newdisc.transform.LookAt(screenclicked);
            newdisc.transform.position += newdisc.transform.forward;
            cur_disc -= 1;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerDisc"))
        {
            if (collision.gameObject.GetComponent<Disc_Controller>().can_pick_up)
            {
                Destroy(collision.gameObject);
                cur_disc += 1;
            }
        }
    }

    public void dealDamage(int amt)
    {
        health -= amt;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
