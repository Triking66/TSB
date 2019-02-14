﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RadiusAggro : MonoBehaviour
{
    int layermask;
    GameObject enemy;
    [SerializeField] float radius = 5;

    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject;
        if (gameObject.tag == ("Enemy"))
        {
            enemy.GetComponent<EnemyController>().enabled = false;
        }
        if (gameObject.tag == "ArcherEnemy")
        {
            enemy.GetComponent<RangedEnemy>().enabled = false;
        }
        layermask = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void Update()
    {
    Collider[] player = Physics.OverlapSphere(transform.position, radius,layermask);
        if (player.Length != 0)
        {
            RaycastHit hit;
            transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
            if (Physics.Raycast(transform.position + transform.forward, transform.forward, out hit, radius))
            {
                if (hit.transform.tag == "Player")
                {
                    if (gameObject.tag == ("Enemy"))
                    {
                        enemy.GetComponent<EnemyController>().enabled = true;
                    }
                    if (gameObject.tag == "ArcherEnemy")
                    {
                        enemy.GetComponent<RangedEnemy>().enabled = true;
                    }
                }
                else
                {
                    print("Hit wall");
                }
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 8)
        {
            if (gameObject.tag == ("Enemy"))
            {
                enemy.GetComponent<EnemyController>().enabled = true;
            }
            if (gameObject.tag == "ArcherEnemy")
            {
                enemy.GetComponent<RangedEnemy>().enabled = true;
            }
        }
    }
}
