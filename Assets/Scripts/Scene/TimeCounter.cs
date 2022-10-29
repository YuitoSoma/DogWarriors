using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    public GameObject finishPanel;
    public int countdownMinutes = 3;
    float countdownSeconds;
    Text timeText;

    void Start()
    {
        timeText = GetComponent<Text>();
        countdownSeconds = countdownMinutes * 60;
        finishPanel.SetActive(false);
    }

    void Update()
    {
        countdownSeconds -= Time.deltaTime;
        var span = new TimeSpan(0, 0, (int)countdownSeconds);
        timeText.text = "TIME " + span.ToString(@"mm\:ss");

        if (countdownSeconds <= 0)
        {
            // 0•b‚É‚È‚Á‚½‚Æ‚«‚Ìˆ—
            finishPanel.SetActive(true);
            Time.timeScale = 0;     // ŽžŠÔ’âŽ~
            if (ScoreText.bestnumber < EnemyManager.counter)
                ScoreText.bestnumber = EnemyManager.counter;
        }
    }
}