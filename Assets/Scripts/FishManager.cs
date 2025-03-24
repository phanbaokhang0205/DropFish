using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
/**
 * Tạo objectPooler để tối ưu khi tạo và xóa nhiều đối tượng.
 * Cơ chế: 
 *     Trong hàm Start, tạo sẵn 1 ds chứa các obj cần dùng (setActive = false)
 *  -> tạo hàm private GameObject GetFishFromPool() để mở các object cần sử dụng (setActive = true)
 *  -> cái nào không dùng nữa thì tắt nó đi (setActive = false).
 */
public class FishManager : MonoBehaviour
{
    public static FishManager Instance;

    //public GameObject[] fishPrefabs;
    public GameObject chosenFish;
    public Fish fishScript;

    //public GameObject inActiveFish;
    //public Fish inActiveFishScript;


    //Tạo ds fishPool và số lượng
    //private List<GameObject> fishPool = new List<GameObject>();
    //private int poolSize = 10;

    private string oldTag;
    private string newTag;
    private int newTagIndex;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        //Tạo random trc 10 con cá sau đó ẩn đi để sử dụng sau này.
        //for (int i = 0; i < poolSize; i++)
        //{
        //    int randomIndex = Random.Range(0, 4);
        //    GameObject fish = Instantiate(fishPrefabs[randomIndex]);
        //    fish.SetActive(false);
        //    fishPool.Add(fish);
        //}
    }

    private void Update()
    {
        if (fishScript.inWater)
        {
            chosenFish = null;
        }
    }

    public void CreateFish(Vector3 spawnPosition)
    {


        //GameObject newFish = GetFishFromPool(spawnPosition);
        //newFish.transform.position = spawnPosition;
        ////newFish.SetActive(true);

        chosenFish = FishPooler.Instance.GetFish(spawnPosition, -1);
        fishScript = chosenFish.GetComponent<Fish>();
        fishScript.prepareToDrop();
    }

    public void PrepareFish(Vector3 touchPosition)
    {
        chosenFish.transform.position = touchPosition;

    }

    public void DropFish()
    {
        fishScript.dropped();
    }

    public void MoveFish(Vector3 touchPosition)
    {
        chosenFish.transform.position = touchPosition;
        fishScript.prepareToDrop();
    }

    public void MergeFish(GameObject collision1, GameObject collision2)
    {
        if (collision1.tag == collision2.tag)
        {
            int level = GetFishLevel(collision1.tag);

            if (collision1.transform.position.y > collision2.transform.position.y)
            {
                FishPooler.Instance.ReturnFish(collision1, level);
                FishPooler.Instance.ReturnFish(collision2, level);

                // reset lại state
                //collision1.GetComponent<Fish>().inActive();
                //collision2.GetComponent<Fish>().inActive();


                Debug.Log(level);
                GameObject nextFish = FishPooler.Instance.GetFish(collision2.transform.position, level + 1);
                Debug.Log(nextFish);
                collision2.GetComponent<Fish>().dropped();


            }
        }
    }

    void UpdateFish(int nextFishIndex, GameObject oldFish)
    {
        //GameObject newFish = Instantiate(fishPrefabs[nextFishIndex], oldFish.transform.position, oldFish.transform.rotation);
        //newFish.SetActive(true);

        //ResetFish(oldFish.tag, oldFish);
    }

    private int GetFishLevel(string tag)
    {
        string[] parts = tag.Split('_');
        return int.Parse(parts[1]);
    }

    //void ResetFish(string tag, GameObject newFish)
    //{
    //    string oldTag = tag;
    //    string[] parts = oldTag.Split('_');

    //    if (parts.Length == 2)
    //    {
    //        int tagIndex = int.Parse(parts[1]);

    //        if (tagIndex > 4)
    //        {
    //            Debug.Log("Old tag: " + tag);
    //            int randomIndex = Random.Range(1, 5); //[1 -> 4]
    //            RandomFishAgain(newFish, new Vector3(1f, 1f, 1f), parts[0], randomIndex);
    //        }
    //        newFish.SetActive(false);
    //        Debug.Log("New tag: " + newFish.tag);

    //    }
    //    newFish.SetActive(false);
    //}



    void RandomFishAgain(GameObject newFish, Vector3 fishScale, string parts, int randomIndex)
    {
        newFish.transform.localScale = fishScale;
        string newTag = parts + "_" + randomIndex;
        newFish.tag = newTag;
    }

    void ChangeTag(string tag, GameObject oldFish)
    {
        oldTag = tag;
        string[] parts = oldTag.Split('_');

        if (parts.Length == 2)
        {
            newTagIndex = int.Parse(parts[1]);
            newTagIndex++;
            UpdateFish(newTagIndex - 1, oldFish);
            newTag = parts[0] + "_" + newTagIndex;

            oldFish.tag = newTag;
        }
    }

    //private GameObject GetFishFromPool(Vector3 spawnPosition)
    //{
    //    foreach (GameObject fish in fishPool)
    //    {
    //        if (!fish.activeInHierarchy) //Lấy con cá nào chưa được active.
    //        {
    //            return fish;
    //        }
    //    }

    //    //Tạo thêm 1 con mới vào fishPool nếu tất cả đã đc sd hết.
    //    int randomIndex = Random.Range(0, 4); //[0 -> 3]
    //    GameObject newFish = Instantiate(fishPrefabs[randomIndex], spawnPosition, Quaternion.identity);
    //    fishPool.Add(newFish);
    //    return newFish;
    //}

}
