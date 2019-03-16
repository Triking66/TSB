using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Projectile : MonoBehaviour
{
    public int damage;
    public float speed;
    public float turn_speed;
    private bool homing = false;
    private bool charging = true;
    [SerializeField] private List<string> target_tags;
    [SerializeField] private float time_to_homing = 0.3f;
    [SerializeField] private int homing_mp_cost = 6;

    private Rigidbody rb;
    private PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (homing)
        {
            // TODO
        }
        else
        {
            if (charging)
            {
                if ((Input.GetButton("Fire1") || Input.GetButton("Fire2")) && pc.magic >= homing_mp_cost)
                {
                    time_to_homing -= Time.deltaTime;
                    if (time_to_homing <= 0f)
                    {
                        homing = true;
                        charging = false;
                        pc.restore_mp(-homing_mp_cost);
                    }
                }
                else
                {
                    charging = false;
                    rb.velocity = transform.up * speed;
                }
            }
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
        Destroy(gameObject);
    }
}
