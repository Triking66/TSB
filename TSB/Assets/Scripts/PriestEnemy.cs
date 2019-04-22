using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PriestEnemy : MonoBehaviour
{
    int layermaskheal;
    int layermaskbind;
    public int health = 100;
    public float speed = 5;
    public float healInterval = 4f;
    public float bindInterval = 5f;
    public float healDistance = 10f;
    public float revivegap = 10f;
    public int amountHealed = 50;
    public GameObject heal;
    public GameObject bind;
    public GameObject revive;
    NavMeshAgent agent;
    public float range = 11;
    private float interval;
    private float distance;
    float initialTime;
    float revinitialTime;
    GameObject player;
    public GameObject enemy;
    ParticleSystem blood;
    Rigidbody rb;
    Animator animator;
    private AudioSource audio;
    public AudioClip bleed;
    public AudioClip hit;
    public AudioClip cast;
    private const string IDLE_ANIMATION_BOOL = "Idle";
    private const string CAST_ANIMATION_BOOL = "Cast";
    private const string WALK_ANIMATION_BOOL = "Walk";
    private const string DIE_ANIMATION_BOOL = "Die";
    private bool dead = false;
    private bool initial = true;

    // Start is called before the first frame update

    private void Awake()
    {
        player = GameObject.Find("PlayerParent");
        layermaskheal = LayerMask.GetMask("Enemy");
        layermaskbind = LayerMask.GetMask("Player");
        rb = GetComponent<Rigidbody>();
        animator=GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        blood = GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        GetComponent<Patrol>().enabled = false;

        //interval = Random.Range(3, healIntervalMax);
        distance = Random.Range(5, 10);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        initialTime = Time.time;
        revinitialTime = Time.time;
        agent.destination = player.transform.position;
        agent.stoppingDistance = distance;
        agent.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dead)
        {
            transform.LookAt(player.transform.position);
            if (Time.time - initialTime >= healInterval || initial) 
            {
                Heal();
            }
            if (Time.time - revinitialTime >= revivegap || initial)
            {
                Resurrection();            
            }
            if (Time.time - initialTime >= bindInterval || initial)
            {
                Bind();
            }
            //if ((transform.position - player.transform.position).magnitude <= distance - 2)
            //{
            //    agent.enabled = false;
            //    AnimateMove();
            //    rb.velocity = player.transform.forward * 2;
            //}
            if ((transform.position - player.transform.position).magnitude >= distance && (transform.position - player.transform.position).magnitude < distance + 4)
            {
                AnimateIdle();
                agent.enabled = false;
               //rb.velocity = Vector3.zero;
            }
            if ((transform.position - player.transform.position).magnitude >distance + 4)
            {
                agent.enabled = true;
                AnimateMove();
                agent.destination = player.transform.position;
               //rb.velocity = Vector3.zero;
            }
        }
    }

    void Heal()
    {
        Collider[] healees = Physics.OverlapSphere(transform.position, healDistance, layermaskheal);
        if (healees.Length != 0)
        {
            Debug.Log(healees[0].transform);
            for(int i = 0; i < healees.Length; i++)
            {
                if (healees[i].transform.GetComponent<EnemyController>().health <= 100-amountHealed)
                {
                    initial = false;
                    AnimateCast();
                    //Vector3 healpoint = healees[i].transform.TransformPoint(0, 3, 0);
                    healees[i].gameObject.GetComponent<EnemyController>().health = healees[i].gameObject.GetComponent<EnemyController>().health + amountHealed;
                    //GameObject healeffect=Instantiate(heal,healees[i].gameObject.transform.position,transform.rotation * Quaternion.Euler(-90, 0, 0));
                    Instantiate(heal, healees[i].transform);
                    //healeffect.transform.parent = healees[i].transform;
                    initialTime = Time.time;
                    break;
                }
            }
        }
    }

    void Bind()
    {
        Collider[] playee = Physics.OverlapSphere(transform.position, healDistance, layermaskbind);
        if (playee.Length != 0)
        {
            initial = false;
            AnimateCast();
            Debug.Log("called");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            GameObject bindeffect = Instantiate(bind, player.transform.position, transform.rotation * Quaternion.Euler(0, 0, 0));
            bindeffect.transform.parent = playee[0].transform;
            initialTime = Time.time;
        }
    }

    public void dealDamage(int amt, Vector3 direction)
    {
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
            player.GetComponent<PlayerController>().dealDamage(-5, Vector3.zero);
            player.GetComponent<PlayerController>().restore_mp(10);
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<RadiusAggro>().enabled = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<BoxCollider>().enabled = false;
            //Destroy(gameObject);
        }
    }
    public void Resurrection()
    {
        Collider[] healees = Physics.OverlapSphere(transform.position, healDistance, layermaskheal);
        if (healees.Length == 0)
        {
            initial = false;
            AnimateCast();
            GameObject revivedenemy=Instantiate(enemy, transform.position + transform.forward * 2, Quaternion.Euler(90, 0, 0));
            GameObject effect =Instantiate(revive,revivedenemy.transform.position,Quaternion.Euler(90,0,0));
            effect.transform.parent = revivedenemy.transform;
            revinitialTime = Time.time;
        }
        
    }

    private void Animate(string boolname)
    {
        DisableOtherAnimations(animator, boolname);
        animator.SetBool(boolname, true);
    }

    private void AnimateCast()
    {
        Animate(CAST_ANIMATION_BOOL);
    }

    private void AnimateMove()
    {
        Animate(WALK_ANIMATION_BOOL);
    }

    private void AnimateIdle()
    {
        Animate(IDLE_ANIMATION_BOOL);
    }
    private void AnimateDie()
    {
        Animate(DIE_ANIMATION_BOOL);
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

}
