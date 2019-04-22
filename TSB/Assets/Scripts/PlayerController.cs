using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject weapon2;
    [SerializeField] private float weapon_cool;
    [SerializeField] private float weapon_cool2;
    [SerializeField] private int mp_cost1;
    [SerializeField] private int mp_cost2;

    [SerializeField] private List<GameObject> drops;

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

    [SerializeField] private Vector3 weapon_offset;

    private Rigidbody rb;
    private int health;
    public int magic;
    private float weapon_cd;
    private float cur_invincible;
    private bool beingHandled;
    private Animator animator;
    private const string IDLE_ANIMATION_BOOL = "Idle";
    private const string ATTACK_ANIMATION_TRIGGER = "Attack";
    private const string MOVE_ANIMATION_BOOL = "Move";
    private const string MAGIC_ANIMATION_TRIGGER = "Cast";
    private const string DIE_ANIMATION_BOOL = "Die";
    public bool dead = false;

    private GameObject blockingWall;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        health = maxHP;
        magic = maxMP;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!dead)
        {
            var dir = Vector3.zero;
            dir.x += Input.GetAxis("Horizontal") * 0.5f;
            dir.z -= Input.GetAxis("Horizontal") * 0.5f;
            dir.z += Input.GetAxis("Vertical") * 0.5f;
            dir.x += Input.GetAxis("Vertical") * 0.5f;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == true && rb.velocity.magnitude > 0.2)
            {
                Animate(MOVE_ANIMATION_BOOL);
            }
            animator.SetFloat("Speed", rb.velocity.magnitude / 5);
            /*if(dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(dir),
                    Time.deltaTime * rotateSpeed);
            }
            */
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
                    try
                    {
                        blockingWall.GetComponent<Renderer>().material = transp;
                    }
                    catch { }
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
                try
                {
                    blockingWall.GetComponent<Renderer>().material = opaque;
                    blockingWall = null;
                }
                catch { }

            }
        }
    }

    void Update()
    {
        if (!dead)
        {
            RaycastHit fire_dir;
            Ray clicked = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layer = 1 << 8;
            if (Physics.Raycast(clicked, out fire_dir, 50f, layer, QueryTriggerInteraction.Ignore))
            {
                Vector3 dir = fire_dir.point;
                dir.y = transform.position.y;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir - transform.position, Vector3.up), rotateSpeed * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            if (Input.GetButtonDown("Fire1") && weapon != null && weapon_cd <= 0 && magic >= mp_cost1)
            {
                if (Physics.Raycast(clicked, out fire_dir, 50f, layer, QueryTriggerInteraction.Ignore))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        Vector3 dir = fire_dir.point;
                        dir.y += 1;
                        GameObject weapon_drop = null;
                        switch (weapon.name)
                        {
                            case ("Disk"):
                                weapon_drop = drops[0];
                                break;
                            case ("Player_Sword"):
                                weapon_drop = drops[1];
                                break;
                            case ("Knife"):
                                weapon_drop = drops[2];
                                break;
                            case ("crossbow_weap"):
                                weapon_drop = drops[3];
                                break;
                        }
                        var newweap = Instantiate(weapon_drop, transform.position, transform.rotation);
                        newweap.transform.LookAt(dir);
                        newweap.transform.position += newweap.transform.forward * 1.5f;
                        newweap.GetComponent<Rigidbody>().velocity = newweap.transform.forward * 2f;

                        weapon = null;
                    }
                    else
                    {
                        Vector3 dir = fire_dir.point;
                        var newweap = Instantiate(weapon, transform.position, transform.rotation);
                        dir.y = newweap.transform.position.y;

                        switch (weapon.name)
                        {
                            case ("Disk"):
                                animator.SetBool(MOVE_ANIMATION_BOOL, false);
                                animator.SetTrigger(MAGIC_ANIMATION_TRIGGER);
                                newweap.transform.LookAt(dir);
                                newweap.transform.position += newweap.transform.forward * 1.2f + weapon_offset;
                                break;
                            case ("Player_Sword"):
                                animator.SetTrigger(ATTACK_ANIMATION_TRIGGER);
                                newweap.GetComponent<Melee_swing>().owner = gameObject;
                                newweap.transform.LookAt(dir);
                                newweap.transform.position += -newweap.transform.right * 2f + weapon_offset;
                                break;
                            case ("Knife"):
                                animator.SetTrigger(ATTACK_ANIMATION_TRIGGER);
                                newweap.GetComponent<first_knife>().owner = gameObject;
                                newweap.transform.LookAt(dir);
                                newweap.transform.position += newweap.transform.forward * 1.2f + weapon_offset;
                                break;
                            case ("crossbow_weap"):
                                newweap.GetComponent<crossbow_script>().owner = gameObject;
                                newweap.transform.LookAt(dir);
                                newweap.transform.position += newweap.transform.forward * 1.2f + weapon_offset;
                                break;
                        }
                        weapon_cd = weapon_cool;
                        magic -= mp_cost1;
                        mp_text.text = "MP " + magic.ToString();
                        magic_bar.rectTransform.sizeDelta = new Vector2(magic * 2, 20);
                    }
                }

            }
            if (Input.GetButtonDown("Fire2") && weapon2 != null && weapon_cd <= 0 && magic >= mp_cost2)
            {
                if (Physics.Raycast(clicked, out fire_dir, 50f, layer, QueryTriggerInteraction.Ignore))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        GameObject weapon_drop = null;
                        switch (weapon2.name)
                        {
                            case ("Disk"):
                                weapon_drop = drops[0];
                                break;
                            case ("Player_Sword"):
                                weapon_drop = drops[1];
                                break;
                            case ("Knife"):
                                weapon_drop = drops[2];
                                break;
                            case ("crossbow_weap"):
                                weapon_drop = drops[3];
                                break;
                        }
                        Vector3 dir = fire_dir.point;
                        dir.y += 1;
                        var newweap = Instantiate(weapon_drop, transform.position, transform.rotation);
                        newweap.transform.LookAt(dir);
                        newweap.transform.position += newweap.transform.forward;
                        newweap.GetComponent<Rigidbody>().velocity = newweap.transform.forward;

                        weapon2 = null;
                    }
                    else
                    {
                        Vector3 dir = fire_dir.point;
                        dir.y += 1;
                        var newweap = Instantiate(weapon2, transform.position, transform.rotation);

                        switch (weapon2.name)
                        {
                            case ("Disk"):
                                animator.SetTrigger(MAGIC_ANIMATION_TRIGGER);
                                newweap.transform.LookAt(dir);
                                newweap.transform.position += newweap.transform.forward * 1.2f + weapon_offset;
                                break;
                            case ("Player_Sword"):
                                animator.SetTrigger(ATTACK_ANIMATION_TRIGGER);
                                newweap.GetComponent<Melee_swing>().owner = gameObject;
                                newweap.transform.LookAt(dir);
                                newweap.transform.position += -newweap.transform.right * 2f + weapon_offset;
                                break;
                            case ("Knife"):
                                animator.SetTrigger(ATTACK_ANIMATION_TRIGGER);
                                newweap.GetComponent<first_knife>().owner = gameObject;
                                newweap.transform.LookAt(dir);
                                newweap.transform.position += newweap.transform.forward * 1.2f + weapon_offset;
                                break;
                            case ("crossbow_weap"):
                                newweap.GetComponent<crossbow_script>().owner = gameObject;
                                newweap.transform.LookAt(dir);
                                newweap.transform.position += newweap.transform.forward * 1.2f + weapon_offset;
                                break;
                        }
                        weapon_cd = weapon_cool2;
                        magic -= mp_cost2;
                        mp_text.text = "MP " + magic.ToString();
                        magic_bar.rectTransform.sizeDelta = new Vector2(magic * 2, 20);
                    }
                }
            }
            if (cur_invincible > 0)
            {
                cur_invincible -= Time.deltaTime;
            }
            if (weapon_cd > 0)
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
    }

    public void dealDamage(int amt, Vector3 dir)
    {
        if (cur_invincible <= 0)
        {
            health -= amt;
            if (health > maxHP)
            {
                health = maxHP;
            }
            hp_text.text = "HP: " + health.ToString();
            health_bar.rectTransform.sizeDelta = new Vector2(health * 2, 20);
            cur_invincible = invincible_time;

            if (dir != Vector3.zero)
            {
                dir.y = .3f;
                rb.AddForce(dir * -100, ForceMode.VelocityChange);
            }

            if (health <= 0)
            {
                dead = true;
                Animate(DIE_ANIMATION_BOOL);
                reset_level.Invoke();
                rb.constraints = RigidbodyConstraints.FreezeAll; 
               // Destroy(gameObject,2);
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
        if(magic > maxMP)
        {
            magic = maxMP;
        }
        mp_text.text = "MP " + magic.ToString();
        magic_bar.rectTransform.sizeDelta = new Vector2(magic * 2, 20);
    }

    public bool pick_up(GameObject new_weap)
    {
        if(weapon == null)
        {
            weapon = new_weap;
            switch (new_weap.name)
            {
                case "Disk":
                    mp_cost1 = 15;
                    break;
                case "Player_Sword":
                    mp_cost1 = 0;
                    break;
                case "Knife":
                    mp_cost1 = 0;
                    break;
                default:
                    mp_cost1 = 0;
                    break;
            }
            return true;
        }
        else if(weapon2 == null)
        {
            weapon2 = new_weap;
            switch (new_weap.name)
            {
                case "Disk":
                    mp_cost2 = 15;
                    break;
                case "Player_Sword":
                    mp_cost2 = 0;
                    break;
                case "Knife":
                    mp_cost2 = 0;
                    break;
                default:
                    mp_cost2 = 0;
                    break;
            }
            return true;
        }
        return false;
    }

    private void Animate(string boolname)
    {
        DisableOtherAnimations(animator, boolname);
        animator.SetBool(boolname, true);
    }

    private void DisableOtherAnimations(Animator animator, string animation)
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.type==AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(parameter.name, false);
            }
            if (parameter.type == AnimatorControllerParameterType.Trigger)
            {
                animator.SetTrigger(0);
            }
        }
    }
}
