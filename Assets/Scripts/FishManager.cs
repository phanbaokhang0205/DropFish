using UnityEngine;


public class FishManager : MonoBehaviour
{
    public static FishManager Instance;

    public GameObject chosenFish;
    public Fish fishScript;
    public GameObject mergeFish;
    public Fish mergeFishScript;


    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {

    }

    public void CreateFish(Vector3 spawnPosition)
    {
        chosenFish = FishPooler.Instance.GetFish(spawnPosition, null);
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
            int level = GetFishLevel(collision1.tag) - 1;
            GameObject evolutionFish = collision1;


            if (collision1.transform.position.y > collision2.transform.position.y)
            {
                Fish coll_1 = collision1.GetComponent<Fish>();
                Fish coll_2 = collision2.GetComponent<Fish>();

                // trước khi return kiểm tra xem có stay collide với obs nào không
                // nếu có thì phá hủy các obs
                // nếu không thì returncoll_1.handleBreakableObs();
                coll_2.handleBreakableObs();
                coll_1.prepareToDrop();
                coll_2.prepareToDrop();
                
                FishPooler.Instance.ReturnFish(collision1, level);
                FishPooler.Instance.ReturnFish(collision2, level);


                mergeFish = FishPooler.Instance.GetFish(evolutionFish.transform.position, level + 1);
                mergeFishScript = mergeFish.GetComponent<Fish>();
                mergeFishScript.dropped();
                GameManager.Instance.updateScore(level + 1);
                AudioManager.Instance.PlayMergeAudio();
            }
        }
    }

    private int GetFishLevel(string tag)
    {
        string[] parts = tag.Split('_');
        return int.Parse(parts[1]);
    }


}
