using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMagic : MonoBehaviour
{

    ParticleSystem system;
    List<ParticleCollisionEvent> hit = new List<ParticleCollisionEvent>();
    // Start is called before the first frame update
    void Start()
    {
        system = GetComponent<ParticleSystem>();
        system.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        system.GetCollisionEvents(other, hit);
        foreach(ParticleCollisionEvent collision in hit)
        {
            if (collision.colliderComponent.gameObject.tag == "Player")
            {
                collision.colliderComponent.gameObject.transform.GetComponent<PlayerController>().dealDamage(1, Vector3.zero);
            }
        }
        system.Pause();
    }
}
