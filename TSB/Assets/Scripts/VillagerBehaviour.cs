using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillagerBehaviour : MonoBehaviour
{
    public int HealAmount = 20;
    public int health = 40;
    public float distance = 4f;
    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    Rigidbody rb;
    AudioSource audio;
    public AudioClip hit;
    public AudioClip bleed;
    public AudioClip scream;
    ParticleSystem blood;
    public GameObject[] places;
    private Vector3 destination;
    private const string TERRIFIED_ANIMATION_BOOL = "Terrified";
    private const string WALK_ANIMATION_BOOL = "Run";
    private const string DIE_ANIMATION_BOOL = "Die";
    private const string IDLE_ANIMATION_BOOL = "Idle";
    private bool dead = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerParent");
        destination = places[Random.Range(0, places.Length-1)].transform.position;
        animator = GetComponent<Animator>();
        rb=GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        audio = GetComponent<AudioSource>();
        blood = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if ((transform.position - player.transform.position).magnitude < distance)
            {
                agent.enabled = true;
                AnimateMove();
                if (!audio.isPlaying)
                    audio.PlayOneShot(scream);
                agent.destination = destination;
            }
            if (transform.position == destination || (transform.position - destination).magnitude <= 2)
            {
                agent.enabled = false;
                AnimateTerrified();
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
            player.GetComponent<PlayerController>().dealDamage(-HealAmount, Vector3.zero);
            //player.GetComponent<PlayerController>().restore_mp(10);
            GetComponent<NavMeshAgent>().enabled = false;
            //GetComponent<RadiusAggro>().enabled = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<BoxCollider>().enabled = false;
            //Destroy(gameObject);
        }
    }
    private void AnimateTerrified()
    {
        Animate(TERRIFIED_ANIMATION_BOOL);
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

    private void Animate(string boolname)
    {
        DisableOtherAnimations(animator, boolname);
        animator.SetBool(boolname, true);
    }
}
