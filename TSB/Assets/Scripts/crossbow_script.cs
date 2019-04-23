using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crossbow_script : MonoBehaviour
{
    [SerializeField] private float time_to_charge = 0.3f;
    [SerializeField] private int charge_mp_cost = 6;
    [SerializeField] private float arrow_spd;
    [SerializeField] private GameObject arrow;

    public GameObject owner;

    private bool charged;
    private Vector3 prev_owner_pos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (owner == null)
        {
            Destroy(gameObject);
        }
        if (owner.CompareTag("Player"))
        {
            PlayerController pc = owner.GetComponent<PlayerController>();
            if ((Input.GetButton("Fire1") || Input.GetButton("Fire2")) && pc.magic >= charge_mp_cost)
            {
                time_to_charge -= Time.deltaTime;
                if (time_to_charge <= 0f)
                {
                    charged = true;
                    pc.restore_mp(-charge_mp_cost);
                    Fire();
                    Destroy(gameObject);
                }
            }
            else
            {
                Fire();
                Destroy(gameObject);
            }
        }
        else
        {
            Fire();
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
    }

    private void Fire()
    {
        GameObject ar = Instantiate(arrow, transform.position, transform.rotation);

        
        ar.transform.position += transform.forward;
        ar.GetComponent<Rigidbody>().velocity = arrow_spd * transform.forward;
        ar.GetComponent<Player_Projectile>().charged = charged;
    }
}
