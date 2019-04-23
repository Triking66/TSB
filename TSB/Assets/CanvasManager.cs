using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    private bool fading = false;
    private bool won = false;
    [SerializeField] private int FadeTime = 120;
    [SerializeField] private float FadeSpeed = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            foreach (Transform child in canvas.transform)
            {
                if (child.transform.gameObject.transform.name == "Background")
                {
                    Color c = child.transform.gameObject.GetComponent<Image>().color;
                    if (c.a < 1)
                        c.a = c.a + FadeSpeed;
                    child.transform.gameObject.GetComponent<Image>().color = c;
                }
                if (child.transform.gameObject.transform.name == "Victory" && won)
                {
                    Color c = child.transform.gameObject.GetComponent<Text>().color;
                    if (c.a < 1)
                        c.a = c.a + FadeSpeed;
                    child.transform.gameObject.GetComponent<Text>().color = c;
                }
                if (child.transform.gameObject.transform.name == "Dead" && !won)
                {
                    Color c = child.transform.gameObject.GetComponent<Text>().color;
                    if (c.a < 1)
                        c.a = c.a + FadeSpeed;
                    child.transform.gameObject.GetComponent<Text>().color = c;
                }
            }
            FadeTime -= 1;
            if (FadeTime <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
