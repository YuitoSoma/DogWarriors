using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    public Text bestScoreText;
    public Text currentScoreText;
    public static int bestnumber = 0;

    void Update()
    {
        currentScoreText.text = "Score : " + EnemyManager.counter;

        if (bestnumber < EnemyManager.counter)
            bestnumber = EnemyManager.counter;
        bestScoreText.text = "BestScore : " + bestnumber;
    }
}
