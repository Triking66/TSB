using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_swing : MonoBehaviour {

    public int damage;
    public int swing_speed;
    public float swing_time;
    private bool swinging = false;
    public GameObject owner;
    [SerializeField] private List<string> target_tags;
    [SerializeField] private float time_to_extend = 0.5f;
    [SerializeField] private int extend_mp_cost = 20;
    [SerializeField] private float extend_amt = 0.6f;
    private Vector3 prev_owner_pos = Vector3.zero;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (owner != null && swinging)
        {
            transform.RotateAround(owner.transform.position, transform.up, swing_speed * Time.deltaTime);
        }
	}

    private void Update()
    {
        if (swinging)
        {
            swing_time -= Time.deltaTime;
            if (swing_time <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (owner == null)
            {
                Destroy(gameObject);
            }
            if (owner.CompareTag("Player"))
            {
                PlayerController pc = owner.GetComponent<PlayerController>();
                if((Input.GetButton("Fire1") || Input.GetButton("Fire2")) && pc.magic >= extend_mp_cost)
                {
                    time_to_extend -= Time.deltaTime;
                    if(time_to_extend <= 0f)
                    {
                        swinging = true;
                        extend();
                        pc.restore_mp(-extend_mp_cost);
                    }
                }
                else
                {
                    swinging = true;
                }
            }
            else
            {
                swinging = true;
            }
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
            if (other.transform.root.gameObject.CompareTag(target_tags[a]) && target_tags[a] == "Villager")
            {
                other.gameObject.GetComponentInParent<VillagerBehaviour>().dealDamage(damage, transform.position - other.transform.position);
            }
            if (other.transform.root.gameObject.CompareTag(target_tags[a]) && target_tags[a] == "King")
            {
                other.gameObject.GetComponentInParent<KingBehaviour>().dealDamage(damage, transform.position - other.transform.position);
            }
            if (other.transform.root.gameObject.CompareTag(target_tags[a]) && target_tags[a] == "Hero")
            {
                other.gameObject.GetComponentInParent<HeroBehaviour>().dealDamage(damage, transform.position - other.transform.position);
            }
        }
    }

    private void extend()
    {
        transform.position += -transform.right * (extend_amt / 2);
        transform.localScale = new Vector3(transform.localScale.x + extend_amt, transform.localScale.y, transform.localScale.z);
    }
}
