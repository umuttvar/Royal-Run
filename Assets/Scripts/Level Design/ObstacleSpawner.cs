using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] obstacles;
    [SerializeField] Transform obstacleParent;
    [SerializeField] float spawnWidth = 4f;


     float spawnObstacleTimes = 2f;

    void Start()
    {
        StartCoroutine(SpawnObstacleRoutine());
    }

    IEnumerator SpawnObstacleRoutine()
    {
        while (true)
        {
            GameObject obstacle = obstacles[Random.Range(0, obstacles.Length)];
            Vector3 spawnPos = new Vector3(Random.Range(-spawnWidth, spawnWidth), transform.position.y, transform.position.z);
            yield return new WaitForSeconds(spawnObstacleTimes);
            Instantiate(obstacle, spawnPos, Random.rotation, obstacleParent);
        }

    }
    
    

   

}
