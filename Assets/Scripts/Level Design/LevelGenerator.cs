using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

#region Instantiate Tanımı 
//chunkprefab: neyin kopyalanacağı
//chunkSpawnPos: kopyalanan objenin hangi konumda olacağı
//Quaternion.identity: kopyalanan objenin rotasyonu(identiy 0,0,0 demek)
//chunkparent: Unity'de hangi parent objenin altında olacağı. eğer bu değeri vermezsek scriptin sahip olduğu gameobject içerisinde çoğalır.
#endregion

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]
    ScoreManager scoreManager;
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject[] chunkPrefabs;
    [SerializeField] GameObject checkpointChunkPrefab;

    [SerializeField] Transform chunkParent;

    [Header("Level Settings")]
    [SerializeField] int chunkAmount = 12;
    [SerializeField] int checkpointChunkInterval = 8;

    [SerializeField] float chunkLenght = 10f;
    [SerializeField] float chunkMoveSpeed = 10f;
    [SerializeField] float minMoveSpeed = 2f;
    [SerializeField] float maxMoveSpeed = 2f;
    [SerializeField] float minGravityZ = -22f;
    [SerializeField] float maxGravityZ = -2f;


    List<GameObject> chunks = new List<GameObject>();

    int chunksSpawned = 0;

    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
        SpawnStartingChunks();
    }

    void Update()
    {
        MoveChunks();
    }

    public void ChangeChunkMovespeed(float speedAmount)
    {

        float newChunkMoveSpeed = chunkMoveSpeed + speedAmount;
        newChunkMoveSpeed = Mathf.Clamp(newChunkMoveSpeed, minMoveSpeed, maxMoveSpeed);

        if (newChunkMoveSpeed != chunkMoveSpeed)
        {
            newChunkMoveSpeed = chunkMoveSpeed;

            float newGravityZ = Physics.gravity.z - speedAmount;
            newGravityZ = Mathf.Clamp(newGravityZ, minGravityZ, maxGravityZ);
            Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, newGravityZ);
            cameraController.ChangeCameraFOV(speedAmount);
        }

        if (speedAmount < 0)
        {
            speedAmount = -10;
            scoreManager.ScoreBoardProcess(Mathf.RoundToInt(speedAmount));
        }
    }

    void SpawnStartingChunks()
    {
        for (int i = 0; i < chunkAmount; i++)
        {
            SpawnChunk();
        }
    }

    void SpawnChunk()
    {
        float spawnPositionZ = CalculateSpawnPositionZ();

        Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);

        GameObject chunkToSpawn = ChooseChunktoSpawn();

        GameObject newChunkGO = Instantiate(chunkToSpawn, chunkSpawnPos, Quaternion.identity, chunkParent);

        chunks.Add(newChunkGO);
        Chunk newChunk = newChunkGO.GetComponent<Chunk>();
        newChunk.Init(this, scoreManager); //this == bu class demek.
        chunksSpawned++;
    }


     GameObject ChooseChunktoSpawn() //hangi chunk'ın spawn olacağını belirler.
    {
        GameObject chunkToSpawn;
        if (chunksSpawned % checkpointChunkInterval == 0 && chunksSpawned !=0) //eğer kalan 0 ise 
        {
            chunkToSpawn = checkpointChunkPrefab; //oluşacak chunk checkpoint olacaktır.
        }

        else
        {
            chunkToSpawn = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)]; //kalan 0 değilse random oluşacak chunk seçilecek.
        }

        return chunkToSpawn;
    }

    float CalculateSpawnPositionZ()
    {
        float spawnPositionZ;
        if (chunks.Count == 0)
        {
            spawnPositionZ = transform.position.z;
        }
        else
        {
            spawnPositionZ = chunks[chunks.Count - 1].transform.position.z + chunkLenght;
        }

        return spawnPositionZ;
    }

    void MoveChunks()
    {
        for (int i = 0; i < chunks.Count; i++)
        {
            GameObject chunk = chunks[i];
            chunk.transform.Translate(-transform.forward * chunkMoveSpeed * Time.deltaTime);

            if (chunk.transform.position.z < Camera.main.transform.position.z - chunkLenght)
            {
                chunks.Remove(chunk);
                Destroy(chunk);
                SpawnChunk();
            }

        }
    }
}
