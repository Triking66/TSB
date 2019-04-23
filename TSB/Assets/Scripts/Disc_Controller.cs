using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc_Controller : MonoBehaviour {

    [SerializeField] private float speed = 30;
    [SerializeField] private int damage = 50;
    [SerializeField] private float rotation_speed = 30;

    public bool can_pick_up = false;
    private bool hit_wall = false;
    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        GetComponent<AudioSource>().Play();
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        rb.maxAngularVelocity = rotation_speed;
        rb.angularVelocity = new Vector3(0, rb.maxAngularVelocity, 0);
    }
	
	// Update is called once per frame
	void Update () {
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !hit_wall)
        {
            hit_wall = true;
            Destroy(gameObject);
        }
        
        else if (collision.gameObject.CompareTag("Enemy") && !hit_wall) 
        {
            hit_wall = true;
            collision.gameObject.GetComponent<EnemyController>().enabled = true;
            collision.gameObject.GetComponent<EnemyController>().dealDamage(damage, transform.position - collision.transform.position);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("ArcherEnemy") && !hit_wall)
        {
            hit_wall = true;

            collision.gameObject.GetComponentInParent<RangedEnemy>().enabled = true;
            collision.gameObject.GetComponentInParent<RangedEnemy>().dealDamage(damage, transform.position - collision.transform.position);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("PriestEnemy") && !hit_wall)
        {
            hit_wall = true;
            collision.gameObject.GetComponentInParent<PriestEnemy>().enabled = true;
            collision.gameObject.GetComponentInParent<PriestEnemy>().dealDamage(damage, transform.position - collision.transform.position);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Villager") && !hit_wall)
        {
            hit_wall = true;
            collision.gameObject.GetComponentInParent<VillagerBehaviour>().dealDamage(damage, transform.position - collision.transform.position);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("King") && !hit_wall)
        {
            hit_wall = true;
            collision.gameObject.GetComponentInParent<KingBehaviour>().dealDamage(damage, transform.position - collision.transform.position);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Hero") && !hit_wall)
        {
            hit_wall = true;
            collision.gameObject.GetComponentInParent<HeroBehaviour>().dealDamage(damage, transform.position - collision.transform.position);
            Destroy(gameObject);
        }
    }
}
