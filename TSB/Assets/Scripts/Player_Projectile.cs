using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Projectile : MonoBehaviour
{
    public int damage;
    public float explosion_radius;
    public float explode_time;
    public bool charged = false;
    [SerializeField] private List<string> target_tags;

    private SphereCollider explosion;

    // Start is called before the first frame update
    void Start()
    {
        explosion = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        for (int a = 0; a < target_tags.Count; a++)
        {
            if (collision.transform.root.gameObject.CompareTag(target_tags[a]) && target_tags[a] == "Player")
            {
                collision.gameObject.GetComponentInParent<PlayerController>().dealDamage(damage, transform.position - collision.transform.position);
            }
            if (collision.transform.root.gameObject.CompareTag(target_tags[a]) && target_tags[a] == "Enemy")
            {
                collision.gameObject.GetComponentInParent<EnemyController>().enabled = true;
                collision.gameObject.GetComponentInParent<EnemyController>().dealDamage(damage, transform.position - collision.transform.position);
            }
            if (collision.transform.root.gameObject.CompareTag(target_tags[a]) && target_tags[a] == "ArcherEnemy")
            {
                collision.gameObject.GetComponentInParent<RangedEnemy>().enabled = true;
                collision.gameObject.GetComponentInParent<RangedEnemy>().dealDamage(damage, transform.position - collision.transform.position);
            }
            if (collision.transform.root.gameObject.CompareTag(target_tags[a]) && target_tags[a] == "PriestEnemy")
            {
                collision.gameObject.GetComponentInParent<PriestEnemy>().enabled = true;
                collision.gameObject.GetComponentInParent<PriestEnemy>().dealDamage(damage, transform.position - collision.transform.position);
            }
            if (collision.transform.root.gameObject.CompareTag(target_tags[a]) && target_tags[a] == "Villager")
            {
                collision.gameObject.GetComponentInParent<VillagerBehaviour>().dealDamage(damage, transform.position - collision.transform.position);
            }
        }
        if (charged)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            explosion.radius = explosion_radius;
            explode_time -= Time.deltaTime;
            if(explode_time <= 0f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
