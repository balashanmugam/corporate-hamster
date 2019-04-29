using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hammerplay.Utils;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] circles;

    [SerializeField]
    float[] wave;

    int currentWave = 0;
    [SerializeField]
    float waveInterval = 45;
    float waveTimer = 0;
    [SerializeField]
    Transform[] points;

    [SerializeField]
    float obstacleTimeInterval, salaryTimeInterval, vacationTimeInterval, platformObstacleInterval;

    float spawnTime = 1, salaryTime = 1, vacationTime = 1, platformTime = 1;
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (GameManager.currentGameState == GameStates.Game)
        {
            if (Time.time - waveTimer > waveInterval)
            {
                waveTimer = Time.time;
                if (currentWave < wave.Length - 1)
                {
                    currentWave++;
                }
            }

            if ((Time.time - spawnTime > wave[currentWave]))
            {
                if (Time.time - salaryTime > salaryTimeInterval)
                {
                    spawnTime = salaryTime = Time.time;
                    GameObject salary = PoolManager.Instantiate("Salary", points[2].position, points[2].rotation).GetGameObject();
                    salary.transform.SetParent(circles[1]);
                }
                else if (Time.time - vacationTime > vacationTimeInterval)
                {
                    spawnTime = vacationTime = Time.time;
                    GameObject salary = PoolManager.Instantiate("Vacation", points[4].position, points[4].rotation).GetGameObject();
                    salary.transform.SetParent(circles[1]);
                }
                else if (Time.time - platformTime > platformObstacleInterval)
                {
                    platformTime = Time.time;
                    spawnTime = Time.time + 1 ;
                    GameObject salary = PoolManager.Instantiate("CircularPlatform", points[3].position, points[3].rotation).GetGameObject();
                    salary.transform.SetParent(circles[0]);
                }
                else if(Time.time - platformTime < platformObstacleInterval -1)
                {
                    spawnTime = Time.time;
                    int index = UnityEngine.Random.Range(0, 2);
					int obsType = UnityEngine.Random.Range(0, 2);
					GameObject obstacle = PoolManager.Instantiate("Obstacle"+index +obsType, points[index].position, points[index].rotation).GetGameObject();
                    obstacle.transform.SetParent(circles[index]);

                }
            }
        }

    }
    void OnEnable()
    {
        GameManager.OnGameStateChange += OngameStateChange;
    }
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        GameManager.OnGameStateChange -= OngameStateChange;
    }
    void OngameStateChange(GameStates gamestate)
    {
        if (gamestate == GameStates.GameStart)
        {
            spawnTime = salaryTime = vacationTime = platformTime = waveTimer = Time.time;
            currentWave = 0;
        }
    }

}
