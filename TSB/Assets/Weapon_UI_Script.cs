using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_UI_Script : MonoBehaviour
{
    [SerializeField] private Image left;
    [SerializeField] private Image right;

    [SerializeField] private Sprite sword;
    [SerializeField] private Sprite skull;
    [SerializeField] private Sprite knife;
    [SerializeField] private Sprite crossbow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Change_Left(string weapon)
    {
        if(weapon == "")
        {
            left.color = new Color(left.color.r, left.color.b, left.color.g, 0);
        }
        else
        {
            left.color = new Color(left.color.r, left.color.b, left.color.g, 1);
        }
        switch (weapon)
        {
            case "sword":
                left.sprite = sword;
                break;
            case "skull":
                left.sprite = skull;
                break;
            case "knife":
                left.sprite = knife;
                break;
            case "crossbow":
                left.sprite = crossbow;
                break;
            default:
                left.sprite = null;
                break;
        }
    }

    public void Change_Right(string weapon)
    {
        if (weapon == "")
        {
            right.color = new Color(right.color.r, right.color.b, right.color.g, 0);
        }
        else
        {
            right.color = new Color(right.color.r, right.color.b, right.color.g, 1);
        }
        switch (weapon)
        {
            case "sword":
                right.sprite = sword;
                break;
            case "skull":
                right.sprite = skull;
                break;
            case "knife":
                right.sprite = knife;
                break;
            case "crossbow":
                right.sprite = crossbow;
                break;
            default:
                right.sprite = null;
                break;
        }
    }
}
