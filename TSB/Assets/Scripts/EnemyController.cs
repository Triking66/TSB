using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    //[SerializeField] private float speed = 500;
    //[SerializeField] private float rotateSpeed = 20;
    //[SerializeField] private int maxHP = 100;
    [SerializeField] private int damage = 20;
    [SerializeField] private float attack_interval = 2f;
    [SerializeField] private GameObject weapon;
    [SerializeField] private float speed;
    private GameObject player;
    public int health = 100;
    private float attack_CD;
    private Rigidbody rb;
    private AudioSource audio;
    private Animator animator;
    private const string IDLE_ANIMATION_BOOL = "Idle";
    private const string ATTACK_ANIMATION_BOOL = "Attack";
    private const string MOVE_ANIMATION_BOOL = "Move";
    private const string DIE_ANIMATION_BOOL = "Die";
    public AudioClip bleed;
    public AudioClip attacking;
    private ParticleSystem blood;
    private bool dead = false;

    private Transform target;
    private NavMeshAgent agent;
    //[SerializeField] private float friction;
    // Use this for initialization
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }
    void Start() {
        GetComponent<Patrol>().enabled = false;
        animator = GetComponent<Animator>();
        player = GameObject.Find("PlayerParent");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        audio = GetComponent<AudioSource>();
        target = player.transform;
        agent.destination = target.position;
        blood = GetComponentInChildren<ParticleSystem>();

        //health = maxHP;
        attack_CD = 0f;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //var dir = player.transform.position - transform.position;
        //transform.rotation = Quaternion.Slerp(
        //    transform.rotation,
        //    Quaternion.LookRotation(dir),
        //    Time.deltaTime * rotateSpeed);
        //rb.AddForce(dir * speed);
        

    }

    private void Update()
    {
        //transform.LookAt(player.transform.position + new Vector3(0, 2, 0));
        if (!dead)
        {
            if (attack_CD > 0)
            {
                attack_CD -= Time.deltaTime;
            }
            if (player != null)
            {
                target = player.transform;
                if (agent.enabled)
                {
                    agent.destination = target.position;
                    AnimateMove();
                }
                if ((transform.position - target.position).magnitude > 1.5)
                {
                    agent.enabled = true;
                }

                if ((transform.position - target.position).magnitude <= 1.5)
                {
                    agent.enabled = false;
                    AnimateIdle();
                }
                if ((transform.position - target.position).magnitude < 2.5 && attack_CD <= 0)
                {
                    attack();
                    attack_CD = attack_interval;
                }
            }
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "PlayerParent" && attack_CD <= 0)
        {
            collision.gameObject.GetComponent<PlayerController>().dealDamage(damage);
            attack_CD = attack_interval;
        }
    }
    */

    private void attack()
    {
        GameObject newWeap = Instantiate(weapon, transform.position, transform.rotation);
        Melee_swing nw = newWeap.GetComponent<Melee_swing>();
        AnimateAttack();
        audio.clip = attacking;
        audio.Play();
        newWeap.transform.position += -newWeap.transform.right * 1.2f;
        nw.damage = damage;
        nw.swing_speed = 200;
        nw.swing_time = .8f;
        nw.owner = gameObject;
    }

    public void dealDamage(int amt, Vector3 dir)
    {
        audio.clip = bleed;
        audio.Play();
        blood.Play();
        health -= amt;



        dir.y = 1;
        rb.AddForce(dir * -5, ForceMode.VelocityChange);

        if (health <= 0)
        {
            dead = true;
            AnimateDie();
            player.GetComponent<PlayerController>().dealDamage(-5, Vector3.zero);
            player.GetComponent<PlayerController>().restore_mp(10);
            Destroy(gameObject,2);
        }
    }

    private void Animate(string boolname)
    {
        DisableOtherAnimations(animator, boolname);
        animator.SetBool(boolname, true);
    }

    private void AnimateAttack()
    {
        Animate(ATTACK_ANIMATION_BOOL);
    }

    private void AnimateMove()
    {
        Animate(MOVE_ANIMATION_BOOL);
    }

    private void AnimateIdle()
    {
        Animate(IDLE_ANIMATION_BOOL);
    }
    private void AnimateDie()
    {
        Animate(DIE_ANIMATION_BOOL);
    }

    private void DisableOtherAnimations(Animator animator,string animation)
    {
        foreach(AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.name != animation)
            {
                animator.SetBool(parameter.name, false);
            }
        }
    }
}
