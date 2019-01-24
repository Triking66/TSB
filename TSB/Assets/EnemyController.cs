using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [SerializeField] private float speed = 500;
    [SerializeField] private float rotateSpeed = 20;
    [SerializeField] private int maxHP = 100;

    [SerializeField] private int damage = 20;
    [SerializeField] private float attack_interval = 2;

    private GameObject player;
    private int health;
    private float attack_CD;
    //[SerializeField] private float friction;

    private Rigidbody rb;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("PlayerParent");
        health = maxHP;
        attack_CD = 0f;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        var dir = player.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            Time.deltaTime * rotateSpeed);
        rb.AddForce(dir * speed);
    }

    private void Update()
    {
        if(attack_CD > 0)
        {
            attack_CD -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "PlayerParent" && attack_CD <= 0)
        {
            collision.gameObject.GetComponent<PlayerController>().dealDamage(damage);
            attack_CD = attack_interval;
        }
    }

    public void dealDamage(int amt)
    {
        health -= amt;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
