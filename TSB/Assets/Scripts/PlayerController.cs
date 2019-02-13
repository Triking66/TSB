using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject weapon2;
    [SerializeField] private GameObject last_thrown;
    [SerializeField] private int num_disc;
    [SerializeField] private float weapon_cool;

    [SerializeField] private float accel;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private float invincible_time;
    [SerializeField] private int maxHP = 100;

    [SerializeField] private Material opaque;
    [SerializeField] private Material transp;

    [SerializeField] private UnityEvent reset_level;

    [SerializeField] private Image health_bar;
    [SerializeField] private Text hp_text;
    [SerializeField] private Text disk_text;
    //[SerializeField] private float friction;

    private Rigidbody rb;
    private int cur_disc;
    private int health;
    private float weapon_cd;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetButtonDown("Fire1") && weapon_cd <= 0)
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
                last_thrown = newdisc;
                weapon_cd = weapon_cool;
            }
            disk_text.text = "Boomerangs Left: " + cur_disc.ToString();
        }
        if(Input.GetButtonDown("Fire2"))
        {
            RaycastHit fire_dir;
            Ray clicked = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layer = 1 << 8;
            if (Physics.Raycast(clicked, out fire_dir, 50f, layer, QueryTriggerInteraction.Ignore))
            {
                Vector3 dir = fire_dir.point;
                dir.y += 1;
                var newweap = Instantiate(weapon2, transform.position, transform.rotation);
                newweap.transform.parent = transform;
                newweap.transform.LookAt(dir);
                newweap.transform.position += -newweap.transform.right * 1.2f;
                last_thrown = newweap;
                weapon_cd = weapon_cool;
            }
        }
        if(cur_invincible > 0)
        {
            cur_invincible -= Time.deltaTime;
        }
        if(weapon_cd > 0)
        {
            weapon_cd -= Time.deltaTime;
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
                disk_text.text = "Boomerangs Left: " + cur_disc.ToString();
            }
        }
    }

    public void dealDamage(int amt, Vector3 dir)
    {
        if (cur_invincible <= 0)
        {
            health -= amt;if (health >= 100)
            {
                health = 100;
            }
            hp_text.text = "HP: " + health.ToString();
            health_bar.rectTransform.sizeDelta = new Vector2(health * 2, 20);
            cur_invincible = invincible_time;

            if (dir != Vector3.zero)
            {
                dir.y = .3f;
                rb.AddForce(dir * -20, ForceMode.VelocityChange);
            }

            if (health <= 0)
            {
                reset_level.Invoke();
                Destroy(gameObject);
            }
            
        }
    }

    public int get_disk()
    {
        return cur_disc;
    }

    public void return_disk()
    {
        Destroy(last_thrown);
        cur_disc += 1;
        disk_text.text = "Boomerangs Left: " + cur_disc.ToString();
        print("Returned");
    }
}
