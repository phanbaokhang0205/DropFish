using System.Collections.Generic;
using UnityEngine;

public class FishPooler : MonoBehaviour
{
    public static FishPooler Instance;

    public GameObject[] fishPrefabs;
    private Dictionary<int, Queue<GameObject>> fishPool = new Dictionary<int, Queue<GameObject>>();
    private int poolSize = 5; // Số lượng cá mỗi level ban đầu trong pool

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializePool();
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

        int fishLevel = level ?? Random.Range(0, 4);

        if (fishPool.ContainsKey(fishLevel) && fishPool[fishLevel].Count > 0)
        {
            GameObject fish = fishPool[fishLevel].Dequeue();
            //Debug.Log($"Số lượng cá level {fishLevel} sau khi lấy: {fishPool[fishLevel].Count}");
            fish.transform.position = spawnPosition;
            fish.SetActive(true);
            return fish;
        }
        else
        {
            // Nếu không có cá trong pool, tạo mới
            Debug.Log("Next level: " + fishLevel);
            GameObject newFish = Instantiate(fishPrefabs[fishLevel], spawnPosition, Quaternion.Euler(0, 90, 0));
            newFish.SetActive(true);
            return newFish;
        }
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
