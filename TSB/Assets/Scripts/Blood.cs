using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour

    
{
    [SerializeField]
    GameObject blood;
    ParticleSystem system;
    List<ParticleCollisionEvent> hits = new List<ParticleCollisionEvent>();

    // Start is called before the first frame update
    void Start()
    {
        system = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        system.GetCollisionEvents(other, hits);
        foreach(ParticleCollisionEvent p in hits)
        {
            var drop = Instantiate(blood);
            drop.transform.position = p.intersection;
            drop.transform.forward = p.normal * -1f;
        }
    }
}
