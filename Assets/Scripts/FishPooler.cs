using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
/**
 * Tạo mảng chứa 4 fish đầu tiên
 * Get fish dựa trên rs (chỉ hiển thị - setActive = true)
 * Nếu đã tồn tại fish thì setActive = false fish cũ và get (true) fish mới
 */
public class FishPooler : MonoBehaviour
{
    public static FishPooler Instance;

    public GameObject[] fishPrefabs;
    public Dictionary<int, Queue<GameObject>> fishPool = new Dictionary<int, Queue<GameObject>>();
    public Dictionary<int, Queue<GameObject>> initalFishes = new Dictionary<int, Queue<GameObject>>();
    
    public int rs;

    public int poolSize = 5;
    private GameObject nextFish;
    
    private void Awake()
    {
        Instance = this;
        InitializePool();
        rs = Random.Range(0, 4);
        //Debug.Log("rs in  awake" + rs);
    }

    private void InitializePool()
    {
        // Khởi tạo fishPool
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

        // Khởi tạo initialFishesPool
        for (int i = 0; i < 4; i++)
        {
            initalFishes[i] = new Queue<GameObject>();
            GameObject fish = Instantiate(fishPrefabs[i]);
            //Debug.Log("\n tag: " + fish.tag);
            fish.SetActive(false);
            initalFishes[i].Enqueue(fish);
        }

    }

    //public void GetInitialFish()
    //{
    //    // lấy rs -> active true fish(rs) -> active false fish(prev_rs)
    //    int prev = rs;
    //    if (initalFishes.ContainsKey(prev) && initalFishes[prev].Count > 0)
    //    {
    //        GameObject fish = initalFishes[prev].Dequeue();
    //        Debug.Log("Previous fish: " + fish.tag);
    //    }

    //    rs = Random.Range(0, 4);
    //    int curr = rs;
    //    if (initalFishes.ContainsKey(curr) && initalFishes[curr].Count > 0)
    //    {
    //        GameObject fish = initalFishes[curr].Dequeue();
    //        Debug.Log("Current fish: " + fish.tag);
    //    }    
    //}

    public GameObject GetFish(Vector3 spawnPosition, int? level = null)
    {

        int fishLevel = level ?? rs;
        checkTheFinalFish(fishLevel);
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

        rs = Random.Range(0, 4);
        //Debug.Log("rs in random: " + rs);
        //Hiển thị nextFish
        if (nextFish)
        {
            nextFish.SetActive(false);
            Debug.Log("rs in false: " + rs);
            initalFishes[rs].Enqueue(nextFish);
        }

        // Dù có `nextFish` hay không, đoạn sau vẫn giống nhau
        nextFish = initalFishes[rs].Dequeue();
        nextFish.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(100, 100, 10));
        nextFish.SetActive(true);
        //Debug.Log("rs in true: " + rs);

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
        if (fishLevel == 10)
        {
            GameManager.Instance.onWin();
        }
    }
}
