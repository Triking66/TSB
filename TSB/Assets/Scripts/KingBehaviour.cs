using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class KingBehaviour : MonoBehaviour
{
    public GameObject[] points;
    GameObject player;
    NavMeshAgent agent;
    AudioSource audio;
    public AudioClip bleed;
    public AudioClip hit;
    Animator animator;
    Rigidbody rb;
    ParticleSystem blood;
    GameObject shield;
    GameObject[] priests;
    int health = 10;
    private bool dead = false;
    private const string TERRIFIED_ANIMATION_BOOL = "Terrified";
    private const string WALK_ANIMATION_BOOL = "Run";
    private const string DIE_ANIMATION_BOOL = "Die";
    private const string IDLE_ANIMATION_BOOL = "Idle";

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerParent");
        agent = GetComponent<NavMeshAgent>();
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        blood = GetComponentInChildren<ParticleSystem>();
        shield = GameObject.Find("Freeze");
        shield.SetActive(false);
        priests = GameObject.FindGameObjectsWithTag("PriestEnemy");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                if (player != null)
                {
                    if ((transform.position - player.transform.position).magnitude <= 10)
                    {
                        GetComponent<BoxCollider>().enabled = false;
                        AnimateMove();
                        agent.enabled = true;
                        agent.destination = points[0].transform.position;
                    }
                    if ((transform.position - points[0].transform.position).magnitude <= 1)
                    {
                        AnimateTerrified();
                    }
                    if (priests[0].GetComponent<PriestEnemy>().health <= 0 && priests[1].GetComponent<PriestEnemy>().health <= 0)
                    {
                        Destroy(player.transform.gameObject);
                        GameManager.instance.Advance_Level();
                        gameObject.SetActive(false);
                    }
                }
            }
            else if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                Debug.Log(shield.gameObject.name);
                try
                {
                    transform.LookAt(player.transform);
                }
                catch { }
                GetComponent<BoxCollider>().enabled = false;
                shield.SetActive(true);
                ParticleSystem[] shields = shield.GetComponentsInChildren<ParticleSystem>();
                foreach(ParticleSystem p in shields)
                {
                    p.Play();
                }
                AnimateTerrified();
                if (GameObject.FindGameObjectWithTag("Hero").GetComponent<HeroBehaviour>().health<=0)
                {
                    GetComponent<BoxCollider>().enabled = true;
                    foreach (ParticleSystem p in shields)
                    {
                        p.Stop();
                    }
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
