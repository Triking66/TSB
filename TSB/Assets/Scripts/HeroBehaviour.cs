using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class HeroBehaviour : MonoBehaviour
{
    public int health = 500;
    public int damage = 30;
    public int speed = 10;
    public float attackinterval = 2f;
    ParticleSystem[] fire;
    Animator animator;
    Rigidbody rb;
    AudioSource audio;
    public AudioClip bleed;
    public AudioClip hit;
    private bool dead = false;
    private const string ATTACK1_ANIMATION_BOOL = "Attack1";
    private const string ATTACK2_ANIMATION_BOOL = "Attack2";
    private const string CAST_ANIMATION_BOOL = "Cast";
    private const string RUN_ANIMATION_BOOL = "Run";
    private const string DIE_ANIMATION_BOOL = "Die";
    private const string IDLE_ANIMATION_BOOL = "Idle";
    ParticleSystem blood;
    GameObject player;
    NavMeshAgent agent;
    float initialTime;
    bool first = true;
    


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerParent");
        agent = GetComponent<NavMeshAgent>();
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        blood = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
        initialTime = Time.time;
        fire = GetComponentsInChildren<ParticleSystem>();
        agent.enabled = false ;
        //agent.destination = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            transform.LookAt(player.transform);
            if  ((transform.position - player.transform.position).magnitude > 5)
            {
                if (!agent.enabled)
                { 
                    AnimateMove();
                    agent.enabled = true;
                    agent.destination = player.transform.position;
                }
            }
            int choice = Random.Range(0, 1);
            if (choice == 0) {
                if ((transform.position - player.transform.position).magnitude <= 3 && Time.time-initialTime>=attackinterval)
                {
                    agent.enabled = false;
                    if (first)
                    {
                        AnimateAttack1();
                        first = false;
                    }
                    else
                    {
                        AnimateAttack2();
                        first = true;
                    }
                    initialTime = Time.time;
                }
            }
            else
            {
                if((transform.position - player.transform.position).magnitude >5)
                {
                    agent.enabled = false;
                    AnimateCast();
                    Magic();
                    initialTime = Time.time;
                }
            }
        }
    }

    public void dealDamage(int amt, Vector3 direction)
    {
        audio.Stop();
        audio.PlayOneShot(bleed);
        audio.PlayOneShot(hit);
        blood.Play();
        health -= amt;
        direction.y = 1;

        rb.AddForce(direction * -15, ForceMode.Impulse);
        if (health <= 0)
        {
            dead = true;
            AnimateDie();
            player.GetComponent<PlayerController>().dealDamage(0, Vector3.zero);
            //player.GetComponent<PlayerController>().restore_mp(10);
            GetComponent<NavMeshAgent>().enabled = false;
            //GetComponent<RadiusAggro>().enabled = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<BoxCollider>().enabled = false;
            //Destroy(gameObject);
        }
    }


    private void Magic()
    {
        for(int i = 0; i < 2; i++)
        {
            fire[i].Play();
        }
    }

    private void AnimateCast()
    {
        Animate(CAST_ANIMATION_BOOL);
    }

    private void AnimateMove()
    {
        Animate(RUN_ANIMATION_BOOL);
    }

    private void AnimateIdle()
    {
        Animate(IDLE_ANIMATION_BOOL);
    }
    private void AnimateDie()
    {
        Animate(DIE_ANIMATION_BOOL);
    }
    private void AnimateAttack1()
    {
        Animate(ATTACK1_ANIMATION_BOOL);
    }
    private void AnimateAttack2()
    {
        Animate(ATTACK2_ANIMATION_BOOL);
    }


    private void DisableOtherAnimations(Animator animator, string animation)
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.name != animation)
            {
                animator.SetBool(parameter.name, false);
            }
        }
    }

    private void Animate(string boolname)
    {
        DisableOtherAnimations(animator, boolname);
        animator.SetBool(boolname, true);
    }


}
