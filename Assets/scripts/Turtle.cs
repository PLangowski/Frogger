using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    public enum TurtleType
    {
        TurtleTypeFloating,
        TurtleTypeDiving
    };

    public TurtleType turtleType= TurtleType.TurtleTypeFloating;

    public Sprite turtleDiveSprite;
    public Sprite turtleFloatSprite;


    public float moveSpeed = 5f;
    public float initialSpeed;

    public bool moveRight = true;
    private readonly float playAreaWidth = 19f;

    public bool shouldDive = false, shouldSurface = true, didDive=false, didSurface=false;

    private float surfaceTime = 5f;
    private float diveTime = 5f;
    private float transitionTime = 5f; 

    private float surfaceTimer, diveTimer,transitionTimer;

    void Update()
    {
        UpdateTurtlePosition();
        UpdateDiveTurtleStatus();
    }

    void UpdateTurtlePosition()
    {
        Vector2 pos = transform.localPosition;

        if (moveRight)
        {
            pos.x += moveSpeed * Time.deltaTime;
            if (pos.x >= ((playAreaWidth / 2) - 1) + (playAreaWidth - 1) - GetComponent<SpriteRenderer>().size.x / 2)
            {
                pos.x = -playAreaWidth / 2 - GetComponent<SpriteRenderer>().size.x / 2;
            }
        }
        else
        {
            pos.x -= moveSpeed * Time.deltaTime;

            if (pos.x <= ((-playAreaWidth / 2) + 1) - (playAreaWidth - 1) + GetComponent<SpriteRenderer>().size.x / 2)
            {
                pos.x = playAreaWidth / 2 + GetComponent<SpriteRenderer>().size.x / 2;
            }
        }
        transform.localPosition = pos;
    }
    void UpdateDiveTurtleStatus()
    {
        if(turtleType==TurtleType.TurtleTypeDiving)
        {
            if(shouldSurface==true)
            {
                transitionTimer += Time.deltaTime;

                if(transitionTimer>=transitionTime)
                {
                    shouldSurface = false;
                    transitionTimer = 0;
                    didSurface = true;
                    GetComponent<SpriteRenderer>().sprite = turtleFloatSprite;
                    
                }
            }

            if(didSurface==true)
            {
                surfaceTimer += Time.deltaTime;

                if (surfaceTimer >= surfaceTime)
                {
                    didSurface = false;
                    surfaceTimer = 0;
                    GetComponent<SpriteRenderer>().sprite = turtleDiveSprite;
                    shouldDive = true;
                }
            }
            if(shouldDive==true)
            {
                transitionTimer += Time.deltaTime;
                if(transitionTimer>=transitionTime)
                {
                    shouldDive = false;
                    didDive = true;
                    transitionTimer = 0;
                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<CollidableObject>().isSafe = false;
                }
            }
            if(didDive==true)
            {
                diveTimer += Time.deltaTime;

                if(diveTimer>=diveTime)
                {
                    didDive = false;
                    shouldSurface = true;
                    diveTimer = 0;
                    GetComponent<SpriteRenderer>().sprite = turtleDiveSprite;
                    GetComponent<CollidableObject>().isSafe = true;
                    GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }
    }
    
}
