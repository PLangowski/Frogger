using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Sprite playerUp, playerDown, playerLeft, playerRight;

    public int health = 4;

    private Vector2 originalPosition;
    private HUD hud;

    public float gameTime = 30f;
    public float gameTimeWarning = 5f;
    public float gameTimer;

    private int maxY = 0;
    private int currentY = 1;

    private bool levelStarting = false;
    private bool levelStarted = false;
    private bool gameStarting = false;
    private bool gameStarted = false;

    private bool gameWon = false;

    private int numFrogsInBase = 0;
    private Vector3 startingTimeBandScale;

    public GameObject gameManager;

    int level;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.localPosition;
        hud = GameObject.Find("HUD").GetComponent<HUD>();
        hud.HideWin();
        startingTimeBandScale = hud.timeband.GetComponent<RectTransform>().localScale;
        GameReset();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        CheckCollisions();
        CheckGameTimer();

        if(Input.GetKeyDown(KeyCode.Return))
        {
            GameReset();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    private void CheckGameTimer()
    {
        if (!gameWon)
        {
            gameTimer += Time.deltaTime;
            Vector3 scale = new Vector3(startingTimeBandScale.x - gameTimer, startingTimeBandScale.y, startingTimeBandScale.z);

            hud.timeband.GetComponent<RectTransform>().localScale = scale;

            if (gameTime - gameTimer <= gameTimeWarning)
            {
                hud.timeband.color = Color.red;
            }
            else
            {
                hud.timeband.color = Color.black;
            }

            if (gameTimer >= gameTime)
            {
                PlayerDied();
            }
        }
    }

    void UpdatePosition()
    {
        Vector2 pos = transform.localPosition;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GetComponent<SpriteRenderer>().sprite = playerUp;
            pos += Vector2.up;
            if(currentY>maxY)
            {
                hud.UpdatePlayerScore(10);
                maxY = currentY;
            }
            currentY++;

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GetComponent<SpriteRenderer>().sprite = playerDown;
            if (pos.y > -6)
            {
                pos += Vector2.down;
                currentY--;
                
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GetComponent<SpriteRenderer>().sprite = playerLeft;
            if (pos.x > -8)
            {
                pos += Vector2.left;
                
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GetComponent<SpriteRenderer>().sprite = playerRight;
            if (pos.x < 8)
            {
                pos += Vector2.right;
                
            }
        }
        transform.localPosition = pos;
    }

    void CheckCollisions()
    {
        bool isSafe = true;
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Collidable");

        foreach (GameObject go in gameObjects)
        {
            CollidableObject collidableObject = go.GetComponent<CollidableObject>();

            if (collidableObject.IsColliding(this.gameObject))
            {
                if (collidableObject.isSafe)
                {
                    isSafe = true;
                    Debug.Log("safe");

                    if (collidableObject.isLog)
                    {
                        Vector2 pos = transform.localPosition;

                        if (collidableObject.GetComponent<Log>().moveRight)
                        {
                            pos.x += collidableObject.GetComponent<Log>().moveSpeed * Time.deltaTime;
                            if (transform.localPosition.x >= 9.5)
                            {
                                pos.x = transform.localPosition.x - 18f;
                            }
                        }
                        else
                        {
                            pos.x -= collidableObject.GetComponent<Log>().moveSpeed * Time.deltaTime;

                            if (transform.localPosition.x <= -9.5)
                            {
                                pos.x = transform.localPosition.x + 18f;
                            }
                        }
                        transform.localPosition = pos;
                    }
                    if (collidableObject.isTurtle)
                    {
                        Vector2 pos = transform.localPosition;
                        if (collidableObject.GetComponent<Turtle>().moveRight)
                        {
                            pos.x += collidableObject.GetComponent<Turtle>().moveSpeed * Time.deltaTime;
                            if (transform.localPosition.x >= 9.5f)
                            {
                                pos.x = transform.localPosition.x - 18f;
                            }
                        }
                        else
                        {
                            pos.x -= collidableObject.GetComponent<Turtle>().moveSpeed * Time.deltaTime;
                            if (transform.localPosition.x <= -9.5f)
                            {
                                pos.x = transform.localPosition.x + 18f;
                            }
                        }
                        transform.localPosition = pos;
                    }

                    if (collidableObject.isHomeBay)
                    {
                        if (!collidableObject.isOccupied)
                        {
                            GameObject trophy = (GameObject)Instantiate(Resources.Load("Prefabs/trophy", typeof(GameObject)), collidableObject.transform.localPosition, Quaternion.identity);
                            trophy.tag = "Trophy";
                            hud.UpdatePlayerScore(50);
                            collidableObject.isOccupied = true;
                            numFrogsInBase++;

                            if(numFrogsInBase==5)
                            {
                                if (level < 3)
                                {
                                    NextLevel();
                                }
                                else
                                {
                                    GameWon();
                                }
                            }

                            int timeRemaining = (int)(gameTime - gameTimer);
                            hud.UpdatePlayerScore(timeRemaining * 10);
                            ResetPosition();
                        }
                        else
                        {
                            PlayerDied();
                        }
                    }


                    break;
                }
                else
                {
                    isSafe = false;

                }
            }
        }
        if (!isSafe)
        {

            if (health == 0)
            {
                GameOver();
            }
            else
            {
                PlayerDied();
            }

        }
    }


    void PlayerDied()
    {
        health--;
        ResetPosition();

    }
    void GameOver()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        hud.ShowLose();
    }
    void GameWon()
    {
        hud.ShowWin();
        gameWon = true;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void NextLevel()
    {
        gameManager.GetComponent<GameManager>().UpdateLevel();
        level = gameManager.GetComponent<GameManager>().level;
        RemoveTrophies();
        numFrogsInBase = 0;
        health = 4;
        ResetPosition();
    }

    
    void GameReset()
    {
        hud.HideWin();
        hud.HideLose();
        GetComponent<SpriteRenderer>().enabled = true;
        gameWon = false;
        gameManager.GetComponent<GameManager>().ResetLevel();
        level = gameManager.GetComponent<GameManager>().level;
        hud.ResetPlayerScore();
        RemoveTrophies();
        numFrogsInBase = 0;
        health = 4;
        ResetPosition();
    }

    void RemoveTrophies()
    {
        GameObject[] trophies= GameObject.FindGameObjectsWithTag("Trophy");
        GameObject[] collidables = GameObject.FindGameObjectsWithTag("Collidable");

        foreach(GameObject trophy in trophies)
        {
            Destroy(trophy.gameObject);
        }

        foreach(GameObject collidable in collidables)
        {
            if(collidable.GetComponent<CollidableObject>().isHomeBay)
            {
                collidable.GetComponent<CollidableObject>().isOccupied = false;
            }
        }
    }

    void ResetPosition()
    {
        gameTimer = 0;
        hud.UpdatePlayerLivesHUD(health);
        transform.localPosition = originalPosition;
        GetComponent<SpriteRenderer>().sprite = playerUp;
        currentY = 1;
        maxY = 0;
    }
}
