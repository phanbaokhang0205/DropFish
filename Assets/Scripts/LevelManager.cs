using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] GameObject[] levels;
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

    private GameObject currentObj;
    private int levelIndex;
    private Transform gamePlay;
    private float currentTime;
    private GameObject targetObs;
    private GameObject targetFish;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        isWaiting = false;

        if (MainMenu.Instance.currentMode == 2)
        {
            levelIndex = MainMenu.Instance.levelIndex;
            currentObj = Instantiate(levels[levelIndex]);
            initData();
        }

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


    private void initData()
    {
        isWaiting = false;
        gamePlay = levels[levelIndex].transform.Find("GamePlay");
        Transform listParent = gamePlay.GetChild(1);
        GameObject newParent = GameObject.Find("target1");
        Transform fishParent = newParent.transform.GetChild(0);
        Transform obstacleParent = newParent.transform.GetChild(1);

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
        if (GameObject.FindWithTag("BreakableObstacle"))
        {
            targetObs = Instantiate(GameObject.FindWithTag("BreakableObstacle"));
            if (targetObs)
            {
                targetObs.transform.localScale = new Vector3(1f, 1f, 1f);
                targetObs.transform.SetParent(obstacleParent);
                targetObs.transform.localPosition = new Vector3(0f, 0f, 50f);
                Rigidbody obs_rb = targetObs.GetComponent<Rigidbody>();
                Destroy(obs_rb);
                targetObstacleTMP.text = listParent.GetChild(2).name;
                targetObstacleAmount = int.Parse(listParent.GetChild(2).name);
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
        CancelInvoke("finishGame");
        GameManager.Instance.isCancleDelayDrop = true;
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(PlayerController.waterWidth, PlayerController.waterHeight, 10));
        FishManager.Instance.CreateFish(touchPosition);
        initData();
        GameManager.Instance.restartGameAdventure();
    }

    public void onLoadNextLevel()
    {
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
        CancelInvoke("onWin");

        //Clear fish pool
        FishPooler.Instance.ClearPool();
        GameManager.Instance.onNextLevelAdventure();
        //Get target - move - timer
        initData();
        //Get fish đầu tiên
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(PlayerController.waterWidth, PlayerController.waterHeight, 10));
        FishManager.Instance.CreateFish(touchPosition);

        

    }


    public void checkGoalInChallenge(int levelFish)
    {
        if (MainMenu.Instance.currentMode == 2 && currentObj)
        {
            // handle giảm fish amount
            if (targetFishTag == levelFish)
            {
                targetFishAmount--;
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
        if (targetFishAmount != 0)
        {
            GameManager.Instance.onLoseAdventure();
        }
        else
        {
            GameManager.Instance.onWinAdventure();
        }
    }
    private int GetFishLevel(string tag)
    {
        string[] parts = tag.Split('_');
        return int.Parse(parts[1]) - 1;
    }
    
}
