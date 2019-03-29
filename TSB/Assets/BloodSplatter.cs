using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    ParticleSystem bloodsystem = new ParticleSystem();
    [SerializeField]
    private GameObject decal;
    // Start is called before the first frame update
    void Start()
    {
        bloodsystem = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> bloods = new List<ParticleCollisionEvent>();
        UnityEngine.ParticlePhysicsExtensions.GetCollisionEvents(bloodsystem, other, bloods);
        for(int i = 0; i < bloods.Count; i++)
        {
            SpawnBlood(bloods[i]);
        }
        
    }

    private void SpawnBlood(ParticleCollisionEvent hit)
    {
        var blood = Instantiate(decal);
        blood.transform.position = hit.intersection;
        blood.transform.forward = hit.normal * -1f;
        Vector3 euler = blood.transform.eulerAngles;
        euler.z = Random.Range(0f, 360f);
        blood.transform.eulerAngles = euler;
    }

}
