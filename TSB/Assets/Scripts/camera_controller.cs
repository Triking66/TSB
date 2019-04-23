using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_controller : MonoBehaviour {
    [SerializeField] private GameObject target;
    [SerializeField] private int distance;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            Vector3 newpos = new Vector3(target.transform.position.x - distance, target.transform.position.y + (2 * distance), target.transform.position.z - distance);
            transform.position = newpos;
            transform.LookAt(target.transform.position);
        }
	}
}
