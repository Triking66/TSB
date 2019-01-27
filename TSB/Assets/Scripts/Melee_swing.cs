using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_swing : MonoBehaviour {

    public int damage;
    public int swing_speed;
    public float swing_time;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.RotateAround(transform.parent.position, transform.up, swing_speed*Time.deltaTime);
	}

    private void Update()
    {
        swing_time -= Time.deltaTime;
        if(swing_time <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.name == "PlayerParent")
        {
            other.gameObject.GetComponentInParent<PlayerController>().dealDamage(damage, transform.position - other.transform.position);
        }
    }
}
