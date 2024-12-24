using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject[] enemyPrefab;
    public Transform[] spawnPoints;

    private float timer = 120f; // Время в секундах
    private float spawnRate = 2f; // Частота спауна
    private float nextSpawnTime;

    void Update()
    {
        timer -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timer <= 0)
        {
            timer = 0;
            SceneManager.LoadScene(4);
        }

        if (Time.time > nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnEnemy()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int enemyIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab[enemyIndex], spawnPoints[spawnPointIndex].position, Quaternion.identity);
    }
}
