using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    //[SerializeField] private float speed = 500;
    //[SerializeField] private float rotateSpeed = 20;
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int damage = 20;
    [SerializeField] private float attack_interval = 2f;
    [SerializeField] private GameObject weapon;
    [SerializeField] private float speed;
    private GameObject player;
    private int health;
    private float attack_CD;
    private Rigidbody rb;

    private Transform target;
    private NavMeshAgent agent;
    //[SerializeField] private float friction;
    // Use this for initialization
    void Start() {
        GetComponent<Patrol>().enabled = false;
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("PlayerParent");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        target = player.transform;
        agent.destination = target.position;

        health = maxHP;
        attack_CD = 0f;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //var dir = player.transform.position - transform.position;
        //transform.rotation = Quaternion.Slerp(
        //    transform.rotation,
        //    Quaternion.LookRotation(dir),
        //    Time.deltaTime * rotateSpeed);
        //rb.AddForce(dir * speed);
        if (player != null)
        {
            target = player.transform;
            agent.destination = target.position;
            if ((transform.position - target.position).magnitude <= 1.5)
            {
                agent.enabled = false;
            }
            if ((transform.position - target.position).magnitude < 2.5 && attack_CD <= 0)
            {
                attack();
                attack_CD = attack_interval;
            }
            if ((transform.position - target.position).magnitude > 1.5)
            {
                agent.enabled = true;
            }
            
        }

    }

    private void Update()
    {
        if(attack_CD > 0)
        {
            attack_CD -= Time.deltaTime;
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "PlayerParent" && attack_CD <= 0)
        {
            collision.gameObject.GetComponent<PlayerController>().dealDamage(damage);
            attack_CD = attack_interval;
        }
    }
    */

    private void attack()
    {
        GameObject newWeap = Instantiate(weapon, transform);
        newWeap.transform.localPosition = new Vector3(-2f, 0, .3f);
        Melee_swing nw = newWeap.GetComponent<Melee_swing>();
        nw.damage = damage;
        nw.swing_speed = 200;
        nw.swing_time = .8f;
    }

    public void dealDamage(int amt, Vector3 dir)
    {
        health -= amt;

        dir.y = 1;
        rb.AddForce(dir * -15, ForceMode.VelocityChange);

        if (health <= 0)
        {
            player.GetComponent<PlayerController>().dealDamage(-5, Vector3.zero);
            Destroy(gameObject);
        }
    }
}
