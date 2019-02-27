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
    public ParticleSystem heal;
    public ParticleSystem bind;
    public ParticleSystem revive;
    NavMeshAgent agent;
    public float range = 11;
    private float interval;
    private float distance;
    float initialTime;
    float revinitialTime;
    GameObject player;
    public GameObject enemy;
    Rigidbody rb;
    

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Patrol>().enabled = false;
        player = GameObject.FindGameObjectWithTag("Player");
        //interval = Random.Range(3, healIntervalMax);
        distance = Random.Range(5, 10);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        initialTime = Time.time;
        revinitialTime = Time.time;
        agent.stoppingDistance = distance;
        rb = GetComponent<Rigidbody>();
        layermaskheal = LayerMask.GetMask("Enemy");
        layermaskbind = LayerMask.GetMask("Player");
        agent.enabled = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time - initialTime >= healInterval)
        {
            Heal();
        }
        if (Time.time - revinitialTime >= revivegap)
        {
            Resurrection();
        }
        if ((transform.position - player.transform.position).magnitude < distance-2)
        {
            agent.enabled = false;
            rb.velocity = player.transform.forward * 2;
            if (Time.time - initialTime >= bindInterval)
            {
                Bind();
                initialTime = Time.time;
            }
        }
        if ((transform.position - player.transform.position).magnitude == distance)
        {
            rb.velocity = Vector3.zero;
        }
        if ((transform.position - player.transform.position).magnitude > distance+2)
        {
            agent.enabled = true;
            agent.destination=player.transform.position;
            rb.velocity = Vector3.zero;
        }
    }

    void Heal()
    {
        Collider[] healees = Physics.OverlapSphere(transform.position, healDistance, layermaskheal);
        if (healees.Length != 0)
        {
            for(int i = 0; i < healees.Length; i++)
            {
                if (healees[i].transform.parent.GetComponent<EnemyController>().health <= 100-amountHealed)
                {
                    Vector3 healpoint = healees[i].transform.TransformPoint(0, 3, 0);
                    healees[i].gameObject.transform.parent.GetComponent<EnemyController>().health = healees[i].gameObject.transform.parent.GetComponent<EnemyController>().health + amountHealed;
                    ParticleSystem healeffect=Instantiate(heal, healpoint, transform.rotation * Quaternion.Euler(90, 0, 0));
                    healeffect.transform.parent = healees[i].transform;
                    initialTime = Time.time;
                    break;
                }
            }
        }
    }

    void Bind()
    {
        Collider[] healees = Physics.OverlapSphere(transform.position, healDistance, layermaskbind);
        if (healees.Length != 0)
        {
            Debug.Log("called");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            ParticleSystem healeffect = Instantiate(bind, player.transform.position+new Vector3(0,2,0), transform.rotation * Quaternion.Euler(90, 0, 0));
            healeffect.transform.parent = healees[1].transform;
        }
    }

    public void dealDamage(int hit, Vector3 direction)
    {
        health -= hit;
        direction.y = 1;

        rb.AddForce(direction * -15, ForceMode.Impulse);
        if (health <= 0)
        {
            player.GetComponent<PlayerController>().dealDamage(-5, Vector3.zero);
            player.GetComponent<PlayerController>().restore_mp(10);
            Destroy(gameObject);
        }
    }
    public void Resurrection()
    {
        Collider[] healees = Physics.OverlapSphere(transform.position, healDistance, layermaskheal);
        if (healees.Length == 0)
        {
            GameObject revivedenemy=Instantiate(enemy, transform.position + transform.forward * 2, Quaternion.Euler(90, 0, 0));
            Instantiate(revive,revivedenemy.transform.position,Quaternion.Euler(90,0,0));
            revinitialTime = Time.time;
        }
        
    }
}
