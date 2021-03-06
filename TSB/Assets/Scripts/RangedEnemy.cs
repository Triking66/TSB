﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : MonoBehaviour
{
    public int health = 100;
    public float speed = 5;
    public float attackIntervalMax = 4f;
    public float attackDistanceMax = 10;
    NavMeshAgent agent;
    public GameObject projectile;
    public float range = 11;
    private float interval;
    private float distance;
    float initialTime;
    GameObject player;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Patrol>().enabled = false;
        player = GameObject.FindGameObjectWithTag("Player");
        interval = Random.Range(2,attackIntervalMax);
        distance = Random.Range(5, 10);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        initialTime = Time.time;
        agent.stoppingDistance = distance;
        rb=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((transform.position - player.transform.position).magnitude <= range)
        {
            if (Time.time - initialTime >= interval)
            {
                transform.LookAt(player.transform);
                Attack();
                initialTime = Time.time;
            }
        }
        if ((transform.position - player.transform.position).magnitude < distance-2)
        {
            agent.enabled = false;
            rb.velocity = player.transform.forward*2;
        }
        if ((transform.position - player.transform.position).magnitude == distance)
        {
            rb.velocity = Vector3.zero;
        }
        if ((transform.position - player.transform.position).magnitude > distance+2)
        {
            rb.velocity = Vector3.zero;
            agent.enabled = true;
            agent.destination = player.transform.position;
            transform.LookAt(player.transform);
        }
    }

    void Attack()
    {
        Vector3 attackpoint = transform.TransformPoint(0, 0, 1);
        GameObject Arrow = Instantiate(projectile, attackpoint, transform.rotation*Quaternion.Euler(90,0,0));
        Arrow.GetComponent<Rigidbody>().velocity = transform.forward * (Arrow.GetComponent<Projectile>().speed+rb.velocity.magnitude);
    }

    public void dealDamage(int hit, Vector3 direction)
    {
        health -= hit;
        direction.y = 1;
        
        rb.AddForce(direction * -15, ForceMode.Impulse);
        if (health <= 0)
        {
            player.GetComponent<PlayerController>().dealDamage(-5, Vector3.zero);
            player.GetComponent<PlayerController>().restore_mp(10);
            Destroy(gameObject);
        }
    }
}
