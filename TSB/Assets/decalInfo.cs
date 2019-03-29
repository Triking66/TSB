using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class decalInfo : MonoBehaviour
{
    [SerializeField]
    private float bloodDisappear;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, bloodDisappear);
    }
}
