using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [Header("Reference Prefabs")]
    [SerializeField] GameObject fencePrefab;
    [SerializeField] GameObject applePrefab;
    [SerializeField] GameObject coinPrefab;

    [Header("Level Settings")]  
    [SerializeField] float appleSpawnChance = .7f;
    [SerializeField] float coinSpawnChance = .5f;
    [SerializeField] float coinSeperationLength = 2f;
    [SerializeField] float[] lanes = { 3f, 0, -3f };

    [Header("References")]

    LevelGenerator levelGenerator;
    ScoreManager scoreManager;

    public void Init(LevelGenerator levelGenerator , ScoreManager scoreManager)
    {
        this.levelGenerator = levelGenerator; //dependency injection
        this.scoreManager = scoreManager;
    }

    //kullanılabilen şeritler sol orta sağ olarak üçe bölündü.
    List<int> availableLanes = new List<int> { 0, 1, 2 };

    void Start()
    {
        SpawnFences();
        SpawnApple();
        SpawnCoins();
    }

   
    int SelectedLane()
    {
        //0 ile şerit adedi arasında şerit seçilecek
        int randomLaneIndex = Random.Range(0, availableLanes.Count);

        //seçilen şerit oluşturduğumuz şerit listesindeki index olacak
        int selectedLane = availableLanes[randomLaneIndex];

        //seçilen şeride bir daha fence oluşamasın diye listeden silinecek
        availableLanes.RemoveAt(randomLaneIndex);

        //seçilen lane değerini döndürecek (örneğin 1, 2 ya da 3)
        return selectedLane;

    }

    void SpawnFences()
    {
        //oluşturalacak fence adedi.
        int fencesToSpawn = Random.Range(0, 3);

        //fence adedine göre for döngüsü kuruldu. fence adedi 3 ise 3 tane fence oluşacak
        for (int i = 0; i < fencesToSpawn; i++)
        {
            //eğer hiç şerit yoksa metot sona erecek
            if (availableLanes.Count <= 0) break;

            int selectedLane = SelectedLane();

            Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
            Instantiate(fencePrefab, spawnPosition, Quaternion.identity, this.transform);
        }
    }

    void SpawnApple()
    {
        if (Random.value > appleSpawnChance || availableLanes.Count <= 0) return;

        int selectedLanes = SelectedLane();
        Vector3 spawnPosition = new Vector3(lanes[selectedLanes], transform.position.y, transform.position.z);
        Apple newApple = Instantiate(applePrefab, spawnPosition, Quaternion.identity, this.transform).GetComponent<Apple>();
        newApple.Init(levelGenerator);
        
    }

    void SpawnCoins()
    {
        if (Random.value > coinSpawnChance || availableLanes.Count <= 0) return;
        int randomLaneIndex = Random.Range(0, availableLanes.Count);
        int selectedLane = availableLanes[randomLaneIndex];

        int maxCoins = 6;
        int coinsToSpawn = Random.Range(0, maxCoins);

        //dizilecek coinler için nereden başlayacağını buluyoruz.
        //transform.position.z ile scriptin bağlı olduğu gameobjectin(chunk) z değerini buluyoruz.
        //coinSeperationLenght coinler arasındaki mesafe
        //bu iki değeri çıkardığımızda coinlerin dizilmeye başlayacğı konumu buluyoruz.
        //projede chunk nesnesinin z değeri 10 yani burada 10 - 2 * 2'den coinler chunk z eksenin 6 değerine sahip olduğu yerden başlayacak.
        float topOfChunkZPos = transform.position.z - (coinSeperationLength * 2f);

        //döngü kaç tane coin sıralanacaksa ona göre çalışacak
        //coinsToSpawn değerimiz aşağıdaki kod ile belli oluyor. Random.Range ile 0 ile maxCoins(6) arasında bir değer alıyor. 
        //int coinsToSpawn = Random.Range(0, maxCoins);

        for (int i = 0; i < coinsToSpawn; i++)
        {
            //spawn olacak coin nesnesinin nerde spawn olacağını buluyoruz. 
            //i=0 iken direkt olarak yukarıda aldığıımız topOfChunkZPos değerinden başlayacak, yani 6 konumundan başlayacak.
            //i=1 olduğunda 6 - 1 * 2'den 8 değeri gelecek ve 4 değerinde diğer coin spawn olacak
            //i=2 olduğunda 6 - 2 * 2'den 2 değeri gelecek ve 2 değerinde diğer coin spawn olacak.
            float spawnPosZ = topOfChunkZPos - (i * coinSeperationLength);
            Vector3 spawnPos = new Vector3(lanes[selectedLane], transform.position.y, spawnPosZ);
            Coin newCoin = Instantiate(coinPrefab, spawnPos, Quaternion.identity, this.transform).GetComponent<Coin>();
            newCoin.Init(scoreManager);
        }
    }

    
}
