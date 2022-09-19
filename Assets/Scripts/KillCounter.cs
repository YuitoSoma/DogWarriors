using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour
{
    public Text scoreText;

    void Update()
    {
        int number = EnemyManager.counter;
        scoreText.text = "Score : " + number;
    }
}
