using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] GameObject weapon;
    [SerializeField] private int num_disc;

    [SerializeField] private float accel;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private float invincible_time;
    [SerializeField] private int maxHP = 100;

    [SerializeField] private Material opaque;
    [SerializeField] private Material transp;
    //[SerializeField] private float friction;

    private Rigidbody rb;
    private int cur_disc;
    private int health;
    private float cur_invincible;

    private GameObject blockingWall;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        cur_disc = num_disc;
        health = maxHP;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        var dir = Vector3.zero;
        dir.x += Input.GetAxis("Horizontal") * 0.5f;
        dir.z -= Input.GetAxis("Horizontal") * 0.5f;
        dir.z += Input.GetAxis("Vertical") * 0.5f;
        dir.x += Input.GetAxis("Vertical") * 0.5f;

        if(dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                Time.deltaTime * rotateSpeed);
        }
        rb.AddForce(dir * accel);
        //var frict = new Vector3(-rb.velocity.x * (1-friction), 0, -rb.velocity.z * (1-friction));
        //rb.AddForce(frict);

        RaycastHit wall;
        var playerScreen = Camera.main.WorldToScreenPoint(transform.position);
        Ray toPlayer = Camera.main.ScreenPointToRay(playerScreen);
        int layer = 1 << 9;
        if (Physics.Raycast(toPlayer, out wall, (Camera.main.transform.position - transform.position).magnitude, layer, QueryTriggerInteraction.Ignore))
        {
            if (blockingWall == null)
            {
                blockingWall = wall.collider.gameObject;
                blockingWall.GetComponent<Renderer>().material = transp;
            }
            else if (blockingWall != wall.collider.gameObject)
            {
                blockingWall.GetComponent<Renderer>().material = opaque;
                blockingWall = wall.collider.gameObject;
                blockingWall.GetComponent<Renderer>().material = transp;
            }
        }
        else if (blockingWall != null)
        {
            blockingWall.GetComponent<Renderer>().material = opaque;
            blockingWall = null;
        }
	}

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && cur_disc > 0)
        {
            RaycastHit fire_dir;
            Ray clicked = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layer = 1 << 8;
            if (Physics.Raycast(clicked, out fire_dir, 50f, layer, QueryTriggerInteraction.Ignore))
            {
                Vector3 dir = fire_dir.point;
                dir.y += 1;
                var newdisc = Instantiate(weapon, transform.position, transform.rotation);
                newdisc.transform.LookAt(dir);
                newdisc.transform.position += newdisc.transform.forward;
                cur_disc -= 1;
            }
        }
        if(cur_invincible > 0)
        {
            cur_invincible -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerDisc"))
        {
            if (collision.gameObject.GetComponent<Disc_Controller>().can_pick_up)
            {
                collision.gameObject.GetComponent<Disc_Controller>().can_pick_up = false;
                Destroy(collision.gameObject);
                cur_disc += 1;
            }
        }
    }

    public void dealDamage(int amt)
    {
        if (cur_invincible <= 0)
        {
            health -= amt;
            print("Took " + amt.ToString() + " damage, at " + health.ToString() + " HP");
            cur_invincible = invincible_time;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
