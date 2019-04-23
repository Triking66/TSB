using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class reset_level : MonoBehaviour {
    [SerializeField] private GameObject canvas;

    private bool fading = false;
    private bool won = false;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.transform.root.CompareTag("Player"))
        {
            Destroy(other.transform.gameObject);
            win();
        }
    }

    public void lose()
    {
        print("Lost");
        GameManager.instance.Reset_Level();
    }

    public void win()
    {
        print("Won");
        GameManager.instance.Advance_Level();
    }
}
