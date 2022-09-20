using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleSceneManager : MonoBehaviour
{
    [SerializeField] Button pauseButton;
    [SerializeField] GameObject pausePanel;
    [SerializeField] Button resumeButton;

    public GameObject enemy;
    public Text scoreText;
    public Text currentScoreText;

    void Start()
    {
        pausePanel.SetActive(false);
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
    }
    void Update()
    {
        int number = EnemyManager.counter;
        scoreText.text = "SCORE : " + number;
        currentScoreText.text = scoreText.text;
    }

    void Pause()
    {
        Time.timeScale = 0;     // éûä‘í‚é~
        pausePanel.SetActive(true);
    }

    void Resume()
    {
        Time.timeScale = 1;     // çƒäJ
        pausePanel.SetActive(false);
    }

    public void EnemyResponce()
    {
        for (int i = 1; i <= 2;i++)
            Instantiate(enemy, new Vector3(Random.Range(-45.0f,45.0f), 0.0f, Random.Range(-45.0f, 45.0f)), Quaternion.identity);
    }

    public void OnStartButton()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
