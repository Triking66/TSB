using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    float initializedtime;
    // Start is called before the first frame update
    void Start()
    {
        initializedtime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Destroy();
    }

    private void Destroy()
    {
        if (Time.time -initializedtime>= 2)
        {
            Destroy(gameObject);
        }
    }
}
