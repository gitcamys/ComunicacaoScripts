using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    private Vector3 spawnPos = new Vector3(25, 0, 0);
    private float startDelay = 2;
    private float repeatRate = 2;
    [SerializeField] private PlayerController playerControllerScript;

    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
    }

    void SpawnObstacle()
    {
        if (PlayerController.IsGameOver() == false)
        {
            GameObject obstacle = Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
            MoveLeft moveLeftScript = obstacle.GetComponent<MoveLeft>();
            
            if (moveLeftScript != null)
            {
                moveLeftScript.Init(playerControllerScript);
            } 
            else
            {
                Debug.LogError("Prefab sem script MoveLeft");
            }
        }
    }
}
