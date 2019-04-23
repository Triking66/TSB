using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knife_dup : MonoBehaviour
{
    public int damage;
    public float stab_speed = 2;
    public float slow_speed = 0.2f;
    private bool stabbing = false;
    public GameObject owner;
    [SerializeField] private List<string> target_tags;
    private Vector3 prev_owner_pos = Vector3.zero;

    private float cur_speed;

    // Start is called before the first frame update
    void Start()
    {
        cur_speed = stab_speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stabbing)
        {
            transform.position += transform.forward * cur_speed;
            cur_speed -= slow_speed * Time.deltaTime;
            if (cur_speed <= -stab_speed)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        if (owner == null)
        {
            Destroy(gameObject);
        }
        if (owner.CompareTag("Player"))
        {
            stabbing = true;
        }
        if (owner != null)
        {
            if (prev_owner_pos != Vector3.zero)
            {
                transform.position += owner.transform.position - prev_owner_pos;
            }
            prev_owner_pos = owner.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int a = 0; a < target_tags.Count; a++)
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
}
