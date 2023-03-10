using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public Image life1, life2, life3, life4, timeband;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;
    public TextMeshProUGUI levelText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = 0.ToString();
        levelText.text = "Level 1";
        loseText.GetComponent<TextMeshProUGUI>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerLivesHUD(int playerHealth)
    {
        switch(playerHealth)
        {
            case 4:
                life1.enabled = true;
                life2.enabled = true;
                life3.enabled = true;
                life4.enabled = true;
                break;
            case 3:
                life1.enabled = true;
                life2.enabled = true;
                life3.enabled = true;
                life4.enabled = false;
                break;
            case 2:
                life1.enabled = true;
                life2.enabled = true;
                life3.enabled = false;
                life4.enabled = false;
                break;
            case 1:
                life1.enabled = true;
                life2.enabled = false;
                life3.enabled = false;
                life4.enabled = false;
                break;
            case 0:
                life1.enabled = false;
                life2.enabled = false;
                life3.enabled = false;
                life4.enabled = false;
                break;
        }
    }

    public void UpdatePlayerScore (int score)
    {
        int currentScore = int.Parse(scoreText.text);
        currentScore += score;
        scoreText.text = currentScore.ToString();
    }

    public void ResetPlayerScore()
    {
        scoreText.text = "0";
    }

    public void ShowWin()
    {
        winText.GetComponent<TextMeshProUGUI>().enabled = true;
    }

    public void HideWin()
    {
        winText.GetComponent<TextMeshProUGUI>().enabled = false;
    }

    public void ShowLose()
    {
        loseText.GetComponent<TextMeshProUGUI>().enabled = true;
    }

    public void HideLose()
    {
        loseText.GetComponent<TextMeshProUGUI>().enabled = false;
    }

    public void UpdateLevel(int level)
    {
        levelText.text = "Level " + level.ToString();
    }
}
