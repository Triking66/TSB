using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectdel : MonoBehaviour
{
    [SerializeField] float Effectduration = 3f;
    // Update is called once per frame
    private void Start()
    {
        Destroy(gameObject, Effectduration);
    }
}
