using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] public GameObject[] levels;
    public GameObject Move;
    public TextMeshProUGUI stepMoveText;
    public TextMeshProUGUI targetFishTMP;
    public TextMeshProUGUI targetObstacleTMP;
    public GameObject Timer;
    public TextMeshProUGUI timerTMP;
    public int targetFishAmount;
    public int targetObstacleAmount;
    public int targetFishTag;
    public bool isWaiting;

    public GameObject currentObj;
    public int levelIndex;
    private Transform obs_list;
    private Transform gamePlay;

    public List<GameObject> obstacleList;
    private float currentTime;
    private GameObject targetObs;
    private GameObject targetFish;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        levels = Resources.LoadAll<GameObject>("levels");
        isWaiting = false;
    }
    void Update()
    {
        if (MainMenu.Instance.currentMode == 2)
        {
            if (currentObj.CompareTag("moveLevel"))
            {
                stepMoveText.text = GameManager.Instance.step.ToString();
                // Kiểm tra điều kiện thắng
                bool hasFish = targetFish;
                bool hasObstacle = targetObs;

                bool isFishDone = targetFishAmount == 0;
                bool isObstacleDone = targetObstacleAmount == 0;
                
                if ((hasFish && !hasObstacle && isFishDone) ||
                    (!hasFish && hasObstacle && isObstacleDone) ||
                    (hasFish && hasObstacle && isFishDone && isObstacleDone))
                {
                    isWaiting = true;
                    Invoke("onWin", 2f);
                }
                
            }
            else if (currentObj.CompareTag("timerLevel"))
            {
                currentTime -= Time.deltaTime;
                if (currentTime <= 0)
                {
                    currentTime = 0;
                }
                if (targetFishAmount == 0)
                {
                    isWaiting = true;
                    Invoke("onWin", 2f);
                }
                timerTMP.text = Mathf.CeilToInt(currentTime).ToString();
            }
        }

    }

    public void setObstacleAmount()
    {
        
        if (!obs_list) return;
        int activeCount = 0;

        foreach (Transform child in obs_list)
        {
            if (child.gameObject.activeSelf)
            {
                activeCount++;
            }
        }
        Debug.Log("Số lượng obstacle đang active: " + activeCount);
        targetObstacleAmount = activeCount;
        targetObstacleTMP.text = targetObstacleAmount.ToString();

    }

    public void initData()
    {
        Debug.Log("Helloo init");
        isWaiting = false;
        gamePlay = levels[levelIndex].transform.Find("GamePlay");
        //obs_list = levels[levelIndex].transform.Find("obs_list");
        obs_list = currentObj.transform.Find("obs_list");
        if (obs_list)
        {

            targetObstacleTMP.text = obstacleList.Count.ToString();
            targetObstacleAmount = obstacleList.Count;
        }
        Transform listParent = gamePlay.GetChild(1);
        GameObject newParent = GameObject.Find("target1");
        Transform fishParent = newParent.transform.GetChild(0);
        Transform obstacleParent = newParent.transform.GetChild(1);
        if (targetFish)
        {
            targetFish.SetActive(false);
        }
        if (targetObs)
        {
            Destroy(targetObs);
            Debug.Log("Destroy thành công");
        }

        // lấy fish lên UI dựa trên targetFishTag sau đó get ra từ pool
        // dùng hàm checkGoalInChallenge để kiểm tra logic merge của fish để --number -> xử lý logic win/lose
        targetFishTag = GetFishLevel(listParent.GetChild(0).tag);
        targetFish = FishPooler.Instance.GetFishInUI(targetFishTag);
        if (targetFish)
        {
            targetFish.transform.localScale = new Vector3(1f, 1f, 1f);
            targetFish.transform.SetParent(fishParent);
            targetFish.transform.localPosition = new Vector3(0f, 0f, 50f);
            Rigidbody fish_rb = targetFish.GetComponent<Rigidbody>();
            Animator fish_anim = targetFish.GetComponent<Animator>();
            Destroy(fish_rb);
            Destroy(fish_anim);
            targetFishTMP.text = listParent.GetChild(1).name;
            targetFishAmount = int.Parse(listParent.GetChild(1).name);
        }

        // lấy obs lên UI dựa trên ... sau đó ... 
        // dùng ... để kiểm tra logic breakableObs để --number -> xử lý logic win/lose
        //Tìm obstacle trong level để hiển thị lên target
        GameObject foundObstacle = null;
       
        if (obs_list)
        {
            foreach (Transform child in obs_list)
            {
                foundObstacle = child.gameObject;
                Debug.Log("OKEEEE");
                break;
            }

            if (foundObstacle)
            {
                Debug.Log("Parent of BreakableObstacle: " + GameObject.FindWithTag("BreakableObstacle").transform.parent.name);
                Debug.Log("current level: " + currentObj.name);

                targetObs = Instantiate(foundObstacle);
                if (targetObs)
                {
                    targetObs.transform.localScale = new Vector3(1f, 1f, 1f);
                    targetObs.transform.SetParent(obstacleParent);
                    targetObs.transform.localPosition = new Vector3(0f, 0f, 50f);
                    Rigidbody obs_rb = targetObs.GetComponent<Rigidbody>();
                    Destroy(obs_rb);
                    targetObstacleTMP.text = listParent.GetChild(2).name;
                    targetObstacleAmount = int.Parse(listParent.GetChild(2).name);
                    Debug.Log("asdsadsa:" + obstacleList.Count);
                }
            }
        }
        



        if (currentObj.CompareTag("moveLevel"))
        {
            // move level
            Timer.SetActive(false);
            Move.SetActive(true);

            stepMoveText.text = gamePlay.GetChild(0).name;
            GameManager.Instance.step = int.Parse(stepMoveText.text);

            Debug.Log("Move level: \n stepmove: " + stepMoveText.text + "\n instance: " + GameManager.Instance.step);

        }
        else if (currentObj.CompareTag("timerLevel"))
        {
            // timer level
            Timer.SetActive(true);
            Move.SetActive(false);

            currentTime = float.Parse(gamePlay.GetChild(2).name);
        }
    }



    /// <summary>
    /// Khi drop -> sau 1s -> get fish
    /// Khi restart sau khi drop 1s -> phải CreateFish để lấy cá mới vì đã ClearPool
    /// Khi restart ngay khi drop trong 1s -> vừa getFish sau 1s vừa CreateFish.
    /// Giải pháp:
    /// # cancle invoke delaydrop
    /// # PlayerController phát tín hiệu tới LevelManager -> nếu drop sau 1s thì CreateFish
    ///                                                    -> nếu drop trong 1s thì kh CreateFish
    /// </summary>
    public void restartLevel()
    {
        if (currentObj)
        {
            Destroy(currentObj);
        }
        if (levelIndex >= levels.Length)
        {
            levelIndex = 0;
            MainMenu.Instance.levelIndex = levelIndex;
        }
        currentObj = Instantiate(levels[MainMenu.Instance.levelIndex]);
        //Clear fish pool
        FishPooler.Instance.ClearPool();
        //Get fish đầu tiên
        CancelInvoke(nameof(finishGame));
        GameManager.Instance.restartGameAdventure();
        initData();
        if (!GameManager.Instance.isCancleDelayDrop)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(PlayerController.waterWidth, PlayerController.waterHeight, 10));
            FishManager.Instance.CreateFish(touchPosition);
        }
    }

    public void onLoadNextLevel()
    {
        GameManager.Instance.setCoinText(50);
        levelIndex++;
        MainMenu.Instance.levelIndex = levelIndex;
        if (currentObj)
        {
            Destroy(currentObj);
        }
        if (levelIndex >= levels.Length)
        {
            levelIndex = 0;
            MainMenu.Instance.levelIndex = levelIndex;
        }
        currentObj = Instantiate(levels[MainMenu.Instance.levelIndex]);
        CancelInvoke(nameof(onWin));
        //Clear fish pool
        FishPooler.Instance.ClearPool();
        GameManager.Instance.onNextLevelAdventure();
        //Get target - move - timer
        initData();
        //Get fish đầu tiên
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(PlayerController.waterWidth, PlayerController.waterHeight, 10));
        FishManager.Instance.CreateFish(touchPosition);

        PlayerController.Instance.setLinePosition();

    }


    public void checkGoalInChallenge(int levelFish)
    {
        if (MainMenu.Instance.currentMode == 2 && currentObj)
        {
            // handle giảm fish amount
            if (targetFishTag == levelFish)
            {
                targetFishAmount--;
                if (targetFishAmount <= 0)
                {
                    targetFishAmount = 0;
                }
                targetFishTMP.text = targetFishAmount.ToString();
            }

            // handle giảm obstacle amount
            if (currentObj.CompareTag("moveLevel"))
            {
                if (GameManager.Instance.step == 0)
                {
                    isWaiting = true;
                    Invoke("finishGame", 3f);
                }
            }
            else if (currentObj.CompareTag("timerLevel"))
            {
                if (currentTime == 0)
                {
                    isWaiting = true;
                    Invoke("finishGame", 3f);
                }
            }
        }
    }
    void onWin()
    {
        GameManager.Instance.onWinAdventure();
    }
    void finishGame()
    {
        
        if (targetFishAmount == 0 && targetObstacleAmount == 0)
        {
            Debug.Log("AMOUNT:" + targetObstacleAmount + ":::" + targetFishAmount);
            GameManager.Instance.onWinAdventure();
            Debug.Log("Winnn");
            
        }
        else
        {
            GameManager.Instance.onLoseAdventure();
            Debug.Log("AMOUNT:" + targetObstacleAmount + ":::" + targetFishAmount);
        }
    }
    private int GetFishLevel(string tag)
    {
        string[] parts = tag.Split('_');
        return int.Parse(parts[1]) - 1;
    }

}
