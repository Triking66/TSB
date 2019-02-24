using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject weapon2;
    [SerializeField] private float weapon_cool;

    [SerializeField] private float accel;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private float invincible_time;
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int maxMP = 100;

    [SerializeField] private Material opaque;
    [SerializeField] private Material transp;

    [SerializeField] private UnityEvent reset_level;

    [SerializeField] private Image health_bar;
    [SerializeField] private Image magic_bar;
    [SerializeField] private Text hp_text;
    [SerializeField] private Text mp_text;
    //[SerializeField] private float friction;

    private Rigidbody rb;
    private int health;
    private int magic;
    private float weapon_cd;
    private float cur_invincible;
    private bool beingHandled;

    private GameObject blockingWall;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        health = maxHP;
        magic = maxMP;
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
        if (Input.GetButtonDown("Fire1") && weapon_cd <= 0 && magic > 15)
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
                weapon_cd = weapon_cool;
                magic -= 15;
                mp_text.text = "MP " + magic.ToString();
                magic_bar.rectTransform.sizeDelta = new Vector2(magic * 2, 20);
            }
            
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
                newweap.transform.position += -newweap.transform.right * 1.6f;
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
        if (rb.constraints == RigidbodyConstraints.FreezePosition)
        {
            if (!beingHandled)
            {
                StartCoroutine(Free());
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

    private IEnumerator Free()
    {
        beingHandled = true;
        yield return new WaitForSeconds(3f);
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        beingHandled = false;
    }

    public void restore_mp(int amt)
    {
        magic += amt;
        mp_text.text = "MP " + magic.ToString();
        magic_bar.rectTransform.sizeDelta = new Vector2(magic * 2, 20);
    }
}
