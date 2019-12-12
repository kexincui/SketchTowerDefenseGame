using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public int MaxLives;
    //public int InitialGold;

    public int mapIndex;
    public int level;
    public GameObject EvalPanel;
    public GameObject VictoryText;
    public GameObject GameOverText;

    private int lives;
    //private int gold;

    private SketchBoxesManagement SBM;

    private PlayerHealthBar PlayerHealth;
    private int remainingEnemies;

    private Score totalScore;
    private Score simScore;
    private Score speedScore;

    // Start is called before the first frame update
    void Start()
    {
        //gold = InitialGold;
        lives = MaxLives;
        PlayerHealth = GameObject.Find("/Grid/Canvas/Health_Blood").GetComponent<PlayerHealthBar>();
        PlayerHealth.maxLives = MaxLives;
        PlayerHealth.lives = lives;

        remainingEnemies = GetComponent<EnemyGenerator>().Waves.Sum(w => w.Amount);

        SBM = GameObject.Find("GameManagement").GetComponent<SketchBoxesManagement>();

        //EvalPanel = GameObject.Find("Evaluation");


        //VictoryText = GameObject.Find("/Evaluation/Victory");
        //GameOverText = GameObject.Find("/Evaluation/GameOver");
        //Debug.Log(GameObject.Find("/Evaluation/Canvas/TotalScores"));
        totalScore = GameObject.Find("/Evaluation/Canvas/TotalScores/Scores").GetComponent<Score>();
        simScore = GameObject.Find("/Evaluation/Canvas/Similarity/Scores").GetComponent<Score>();
        speedScore = GameObject.Find("/Evaluation/Canvas/Speed/Scores").GetComponent<Score>();
        EvalPanel.SetActive(false);
    }

    public void EnemyEscaped()
    {
        lives--;
        PlayerHealth.UpdateHealth(lives);
        if (lives <= 0)
        {
            End(false);
        }
        remainingEnemies--;
        if (remainingEnemies == 0)
        {
            End(true);
        }
    }

    public void EnemyKilled()
    {
        remainingEnemies--;
        if (remainingEnemies == 0)
        {
            End(true);
        }
    }

    public void End(bool isWin)
    {
        float sim = SBM.similarity / SBM.numGesture;
        if (sim < 0.5)
        {
            simScore.health = 0;
        } else if (sim < 0.7)
        {
            simScore.health = 1;
        } else if (sim < 0.9)
        {
            simScore.health = 2;
        } else
        {
            simScore.health = 3;
        }


        //Debug.Log(s);
        if (SBM.time <= 0)
        {
            speedScore.health = 0;
        }
        else
        {
            float s = SBM.length / SBM.time;
            if (s < 1.0f)
            {
                speedScore.health = 0;
            }
            else if (s < 2.0f)
            {
                speedScore.health = 1;
            }
            else if (s < 3.5f)
            {
                speedScore.health = 2;
            }
            else
            {
                speedScore.health = 3;
            }
        }
        int liveScore = (int)((float)lives / MaxLives * 3);
        totalScore.health = (2*simScore.health + speedScore.health + 4 * liveScore+4)/7;
        EvalPanel.SetActive(true);
        VictoryText.SetActive(isWin);
        GameOverText.SetActive(!isWin);
        Time.timeScale = 0.0f;
        SBM.isInputEnabled = false;
        GameObject.Find("GameManagement").GetComponent<MusicManager>().end = true;
    }
}
