using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class FishPooler : MonoBehaviour
{
    public static FishPooler Instance;

    public GameObject[] fishPrefabs;
    public Dictionary<int, Queue<GameObject>> fishPool = new Dictionary<int, Queue<GameObject>>();
    public Dictionary<int, Queue<GameObject>> initalFishes = new Dictionary<int, Queue<GameObject>>();
    
    public int rs;

    public int poolSize = 5;
    public GameObject nextFishImage;

    private GameObject nextFish;
    public Transform FishPool;

    private void Awake()
    {
        Instance = this;
        InitializePool();
        rs = Random.Range(0, 4);
    }

    public void InitializePool()
    {
        // Khởi tạo cá trong pool
        for (int i = 0; i < 6; i++)
        {
            fishPool[i] = new Queue<GameObject>();

            for (int j = 0; j < poolSize; j++)
            {
                GameObject fish = Instantiate(fishPrefabs[i], FishPool);
                fish.SetActive(false);
                fishPool[i].Enqueue(fish);
            }
        }

        // Khởi tạo 4 cá đầu tiên, dùng để hiển thị cá tiếp theo
        for (int i = 0; i < 4; i++)
        {
            initalFishes[i] = new Queue<GameObject>();
            GameObject fish = Instantiate(fishPrefabs[i], FishPool);
            fish.SetActive(false);
            initalFishes[i].Enqueue(fish);
        }

    }

    public GameObject GetFish(Vector3 spawnPosition, int? level = null)
    {
        int fishLevel = level ?? rs;
        checkTheFinalFish(fishLevel);
        LevelManager.Instance.checkGoalInChallenge(fishLevel);
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

        //cất cá trước đó đi để lấy ra cá tiếp theo
        if (nextFish)
        {
            nextFish.SetActive(false);
            initalFishes[rs].Enqueue(nextFish);
        }

        //random cá tiếp theo và hiển thị
        rs = Random.Range(0, 4);

        nextFish = initalFishes[rs].Dequeue();
        Vector3 imagePos = nextFishImage.transform.position;
        nextFish.transform.position = new Vector3(imagePos.x, imagePos.y, 0);

        nextFish.SetActive(true);
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



    public void checkTheFinalFish(int fishLevel)
    {
        if (fishLevel == 8)
        {
            GameManager.Instance.onWin();
        }
    }
    
    public void ClearPool()
    {
        foreach (Transform child in FishPool)
        {
            Destroy(child.gameObject);
        }

        fishPool.Clear();
        initalFishes.Clear();
    }

}
