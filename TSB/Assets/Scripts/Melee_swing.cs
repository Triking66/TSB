using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_swing : MonoBehaviour {

    public int damage;
    public int swing_speed;
    public float swing_time;
    public GameObject owner;
    [SerializeField] private List<string> target_tags;
    private Vector3 prev_owner_pos = Vector3.zero;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (owner != null)
        {
            transform.RotateAround(owner.transform.position, transform.up, swing_speed * Time.deltaTime);
        }
	}

    private void Update()
    {
        swing_time -= Time.deltaTime;
        if(swing_time <= 0)
        {
            Destroy(gameObject);
        }
        if (owner != null)
        {
            if (prev_owner_pos != Vector3.zero)
            {
                transform.position += owner.transform.position - prev_owner_pos;
            }
            prev_owner_pos = owner.transform.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int a= 0; a < target_tags.Count; a++)
        {
            if (other.transform.root.gameObject.CompareTag(target_tags[a]) && target_tags[a] == "Player")
            {
                other.gameObject.GetComponentInParent<PlayerController>().dealDamage(damage, transform.position - other.transform.position);
            }
            if (other.transform.root.gameObject.CompareTag(target_tags[a]) && target_tags[a] == "Enemy")
            {
                other.gameObject.GetComponentInParent<EnemyController>().dealDamage(damage, transform.position - other.transform.position);
            }
            if (other.transform.root.gameObject.CompareTag(target_tags[a]) && target_tags[a] == "ArcherEnemy")
            {
                other.gameObject.GetComponentInParent<RangedEnemy>().dealDamage(damage, transform.position - other.transform.position);
            }
            if (other.transform.root.gameObject.CompareTag(target_tags[a]) && target_tags[a] == "PriestEnemy")
            {
                other.gameObject.GetComponentInParent<PriestEnemy>().dealDamage(damage, transform.position - other.transform.position);
            }
        }
    }
}
