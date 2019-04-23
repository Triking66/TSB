using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy_spawn_script : MonoBehaviour {


    [SerializeField] GameObject enemy_type;
    private bool active = false;
    private int enemies_remain = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            enemies_remain = 0;
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Enemy"))
                {
                    enemies_remain += 1;
                }
                if (child.CompareTag("ArcherEnemy"))
                {
                    enemies_remain += 1;
                }
            }
            if (enemies_remain == 0)
            {
                active = false;
                foreach(Transform child in transform)
                {
                    if (child.CompareTag("Wall"))
                    {
                        child.gameObject.GetComponent<MeshRenderer>().enabled = false;
                        child.gameObject.GetComponent<BoxCollider>().enabled = false;
                    }
                }
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.CompareTag("Player"))
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Wall"))
                {
                    child.gameObject.GetComponent<MeshRenderer>().enabled = true;
                    child.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
                else if (child.CompareTag("Spawn"))
                {
                    GameObject ne = Instantiate(enemy_type, transform);
                    ne.GetComponent<NavMeshAgent>().Warp(child.transform.position);
                    Destroy(child.gameObject);
                }
            }
            active = true;
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
