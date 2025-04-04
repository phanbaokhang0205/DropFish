using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class FishPooler : MonoBehaviour
{
    public static FishPooler Instance;

    public GameObject[] fishPrefabs;
    public Dictionary<int, Queue<GameObject>> fishPool = new Dictionary<int, Queue<GameObject>>();
    public int rs;

    public int poolSize = 5;
    public GameObject nextFish;

    private void Awake()
    {
        Instance = this;
        InitializePool();
        rs = Random.Range(0, 4);
    }

    private void InitializePool()
    {
        for (int i = 0; i < 6; i++)
        {
            fishPool[i] = new Queue<GameObject>();

            for (int j = 0; j < poolSize; j++)
            {
                GameObject fish = Instantiate(fishPrefabs[i]);
                fish.SetActive(false);
                fishPool[i].Enqueue(fish);
            }
        }
    }

    public GameObject GetFish(Vector3 spawnPosition, int? level = null)
    {

        int fishLevel = level ?? rs;
        if (fishPool.ContainsKey(fishLevel) && fishPool[fishLevel].Count > 0)
        {
            GameObject fish = fishPool[fishLevel].Dequeue();
            fish.transform.position = spawnPosition;
            fish.SetActive(true);
            randomFish(level);
            return fish;
        }
        else
        {
            GameObject newFish = Instantiate(fishPrefabs[fishLevel], spawnPosition, Quaternion.Euler(0, 90, 0));
            newFish.SetActive(true);
            randomFish(level);
            return newFish;
        }
    }


    public void randomFish(int? level)
    {
        if (level != null) return;

        //if (nextFish) { nextFish.SetActive(false); }

        rs = Random.Range(0, 4);
        //nextFish = fishPool[rs].Dequeue();

        //nextFish.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(100, 600, 10));
        //nextFish.SetActive(true);
        
        Debug.Log("Next fish: " + rs);
    }

    public void ReturnFish(GameObject fish, int level)
    {
        fish.SetActive(false);
        if (!fishPool.ContainsKey(level))
        {
            fishPool[level] = new Queue<GameObject>();
        }
        fishPool[level].Enqueue(fish);
    }
}
