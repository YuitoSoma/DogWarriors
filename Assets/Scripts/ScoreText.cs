using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    public Text bestScoreText;
    public static int bestnumber = 0;

    void Update()
    {
        bestScoreText.text = "BEST SCORE : " + bestnumber;
    }
}
