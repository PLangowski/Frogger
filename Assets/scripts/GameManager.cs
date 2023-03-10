using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int level;
    public GameObject player;
    GameObject[] collidables;
    private HUD hud;

    // Start is called before the first frame update
    void Start()
    {
        hud = GameObject.Find("HUD").GetComponent<HUD>();
        collidables = GameObject.FindGameObjectsWithTag("Collidable");
        level = 1;
    }

   public void UpdateLevel()
    {
        level++;
        SetLevel(level);
        hud.UpdateLevel(level);
    }

   public void ResetLevel()
    {
        level = 1;
        SetLevel(level);
        hud.UpdateLevel(level);
    }


    void SetLevel(int level)
    {
        if (level > 1)
        {
            foreach (GameObject go in collidables)
            {
                CollidableObject co = go.GetComponent<CollidableObject>();

                if (co.isLog)
                {
                    co.GetComponent<Log>().moveSpeed = 0.5f*level+ co.GetComponent<Log>().initialSpeed;
                }
                else if (co.isTurtle)
                {
                    co.GetComponent<Turtle>().moveSpeed = 0.5f*level+ co.GetComponent<Turtle>().initialSpeed;
                }
                else if (co.isCar)
                {
                    co.GetComponent<Car>().moveSpeed = 0.5f*level+ co.GetComponent<Car>().initialSpeed;
                }
            }
        }

        else
        {
            foreach (GameObject go in collidables)
            {
                CollidableObject co = go.GetComponent<CollidableObject>();

                if (co.isLog)
                {
                    co.GetComponent<Log>().moveSpeed = co.GetComponent<Log>().initialSpeed;
                }
                else if (co.isTurtle)
                {
                    co.GetComponent<Turtle>().moveSpeed = co.GetComponent<Turtle>().initialSpeed;
                }
                else if (co.isCar)
                {
                    co.GetComponent<Car>().moveSpeed = co.GetComponent<Car>().initialSpeed;
                }
            }
        }
    }
}
