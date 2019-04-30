using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject player;

    private bool fading = false;
    private bool won = false;
    [SerializeField] private int FadeTime = 120;
    [SerializeField] private float FadeSpeed = 0.01f;
    private int cur_fade;
    public static GameManager instance;
    private int player_hp;
    private int player_mp;
    private GameObject weapon1;
    private GameObject weapon2;
    int level_num = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Refresh_Canvas();
        cur_fade = FadeTime;

        SceneManager.activeSceneChanged += OnLevelLoaded;


    }

    public void Advance_Level()
    {
        canvas.GetComponent<Canvas>().sortingOrder = 1;
        if (player != null)
        {
            player_hp = player.GetComponent<PlayerController>().health;
            player_mp = player.GetComponent<PlayerController>().magic;
            weapon1 = player.GetComponent<PlayerController>().weapon;
            weapon2 = player.GetComponent<PlayerController>().weapon2;
        }
        else
        {
            player_hp = 100;
            player_mp = 100;
        }
        won = true;
        level_num += 1;
        if(level_num > 4)
        {
            level_num = 0;
        }
        fading = true;
    }

    public void Reset_Level()
    {
        won = false;
        fading = true;
    }

    private void Refresh_Canvas()
    {
        canvas = GameObject.Find("Game_Over");
        player = GameObject.Find("PlayerParent");
    }

    private void OnLevelLoaded(Scene current, Scene next)
    {
        Refresh_Canvas();
        if (won && player != null)
        {
            player.GetComponent<PlayerController>().health = player_hp;
            player.GetComponent<PlayerController>().magic = player_mp;
            player.GetComponent<PlayerController>().dealDamage(0, Vector3.zero);
            player.GetComponent<PlayerController>().restore_mp(0);
            if (level_num == 3 || level_num == 4)
            {
                player.GetComponent<PlayerController>().weapon = null;
                player.GetComponent<PlayerController>().weapon = null;
                player.GetComponent<PlayerController>().pick_up(weapon1);
                player.GetComponent<PlayerController>().pick_up(weapon2);
            }
        }
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
                if (child.transform.gameObject.transform.name == "Dead" && !won)
                {
                    Color c = child.transform.gameObject.GetComponent<Text>().color;
                    if (c.a < 1)
                        c.a = c.a + FadeSpeed;
                    child.transform.gameObject.GetComponent<Text>().color = c;
                }
            }
            cur_fade -= 1;
            if (cur_fade <= 0)
            {
                cur_fade = FadeTime;
                fading = false;
                Debug.Log(level_num);
                SceneManager.LoadScene(level_num);
                Refresh_Canvas();
            }
        }
    }
}
