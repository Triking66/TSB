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

    private GameObject player;
    private int health;
    private float attack_CD;

    private Transform target;
    private NavMeshAgent agent;
    //[SerializeField] private float friction;

    //private Rigidbody rb;
    // Use this for initialization
    void Start () {
        //rb = GetComponent<Rigidbody>();
        player = GameObject.Find("PlayerParent");
        agent = GetComponent<NavMeshAgent>();
        target = player.transform;
        agent.destination = target.position;

        health = maxHP;
        print("Starting hp: " + health.ToString());
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
            if ((transform.position - target.position).magnitude < 2.5 && attack_CD <= 0)
            {
                attack();
                attack_CD = attack_interval;
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
        newWeap.transform.localPosition = new Vector3(-0.8f, 0, 0);
        Melee_swing nw = newWeap.GetComponent<Melee_swing>();
        nw.damage = damage;
        nw.swing_speed = 200;
        nw.swing_time = .8f;
    }

    public void dealDamage(int amt)
    {
        health -= amt;
        print("Took " + amt.ToString() + " damage, at " + health.ToString() + " HP");
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
