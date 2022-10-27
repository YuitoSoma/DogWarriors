using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleSceneManager : MonoBehaviour
{
    [SerializeField] Button pauseButton;
    [SerializeField] GameObject pausePanel;
    [SerializeField] Button resumeButton;

    public GameObject enemy;
    public GameObject finishPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finishCurrentScoreText;
    
    public Slider slider;
    void Start()
    {
        Time.timeScale = 1;     // 再開
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
    }
    void Update()
    {
        int number = EnemyManager.counter;
        // 現在のスコア表示
        scoreText.text = "SCORE : " + number;
        finishCurrentScoreText.text = scoreText.text;
        if (slider.value == 0.0f)
        {
            finishPanel.SetActive(true);
            Time.timeScale = 0;     // 時間停止
            EnemyManager.counter = 0;
        }

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
        EnemyManager.counter = 0;
    }
}
