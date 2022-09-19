using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneManager : MonoBehaviour
{
    public GameObject enemy;
    public GameObject healItem;
    public Text scoreText;
    public void EnemyResponce()
    {
        for (int i = 1; i <= 2;i++)
            Instantiate(enemy, new Vector3(Random.Range(-45.0f,45.0f), 0.0f, Random.Range(-45.0f, 45.0f)), Quaternion.identity);
    }

    public void HealItemResponce()
    {
        Instantiate(healItem, new Vector3(Random.Range(-45.0f, 45.0f), 0.0f, Random.Range(-45.0f, 45.0f)), Quaternion.identity);
    }

    void Update()
    {
        int number = EnemyManager.counter;
        scoreText.text = "Score : " + number;
    }
}
