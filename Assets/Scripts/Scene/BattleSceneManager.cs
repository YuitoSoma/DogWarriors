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
        Time.timeScale = 1;     // 再開
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
    }
    void Update()
    {
        int number = EnemyManager.counter;
        scoreText.text = "SCORE : " + number;
        currentScoreText.text = scoreText.text;
    }

    public void Pause()
    {
        Time.timeScale = 0;     // 時間停止
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;     // 再開
        pausePanel.SetActive(false);
    }

    public void EnemyResponce()
    {
        for (int i = 1; i <= 2;i++)
            Instantiate(enemy, new Vector3(Random.Range(-40.0f,40.0f), 0.3f, Random.Range(-40.0f, 40.0f)), Quaternion.identity);
    }

    public void OnSResumeButton()
    {
        SceneManager.LoadScene("BattleScene");
        Time.timeScale = 1;     // 再開
    }

    public void OnResetButton()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
