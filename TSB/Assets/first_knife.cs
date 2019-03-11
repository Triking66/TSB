using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class first_knife : MonoBehaviour
{
    public int damage;
    public float stab_speed = 2;
    public float slow_speed = 0.2f;
    private bool stabbing = false;
    public GameObject owner;
    [SerializeField] private List<string> target_tags;
    [SerializeField] private float time_to_dup = 0.3f;
    [SerializeField] private int duplicate_mp_cost = 6;
    [SerializeField] private GameObject dups;
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
            if(cur_speed <= -stab_speed)
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
        if (owner.CompareTag("Player") && !stabbing)
        {
            PlayerController pc = owner.GetComponent<PlayerController>();
            if ((Input.GetButton("Fire1") || Input.GetButton("Fire2")) && pc.magic >= duplicate_mp_cost)
            {
                time_to_dup -= Time.deltaTime;
                if (time_to_dup <= 0f)
                {
                    stabbing = true;
                    duplicate();
                    pc.restore_mp(-duplicate_mp_cost);
                }
            }
            else
            {
                stabbing = true;
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
        }
    }

    private void duplicate()
    {
        GameObject k1 = Instantiate(dups, transform.position, transform.rotation);
        GameObject k2 = Instantiate(dups, transform.position, transform.rotation);

        k1.transform.position += k1.transform.right * .1f;
        k1.transform.Rotate(0, 45, 0);
        k1.GetComponent<knife_dup>().owner = owner;
        k2.transform.position -= k1.transform.right * .1f;
        k2.transform.Rotate(0, -45, 0);
        k2.GetComponent<knife_dup>().owner = owner;
    }
}
