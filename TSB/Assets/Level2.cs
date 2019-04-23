using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2 : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.name == "PlayerParent")
        {
            SceneManager.LoadScene(2);
        }
    }
}
