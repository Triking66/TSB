using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void Playgame ()
    {
        GameManager.instance.Advance_Level();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quitgame ()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
