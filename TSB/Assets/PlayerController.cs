using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] GameObject weapon;
    [SerializeField] private float weaponCD;

    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float friction;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        var dir = Vector3.zero;
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");

        if(dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                Time.deltaTime * rotateSpeed);
        }
        else
        {
            //transform.rotation = Quaternion.identity;
        }
        rb.AddForce(dir * speed);
        var frict = new Vector3(-rb.velocity.x * (1-friction), 0, -rb.velocity.z * (1-friction));
        rb.AddForce(frict);
	}
}
