using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] public GameObject[] levels;
    [SerializeField] GameObject ContentView;
    [SerializeField] GameObject LevelSample;
    [SerializeField] TextMeshProUGUI LevelSampleText;

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

    private GameObject levelItem;
    private Vector2 levelPosition;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        levels = Resources.LoadAll<GameObject>("levels");
        isWaiting = false;
        loadLevelToUI();
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
                    Invoke(nameof(onWin), 2f);
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
                    Invoke(nameof(onWin), 2f);
                }
                timerTMP.text = Mathf.CeilToInt(currentTime).ToString();
            }
        }

    }

    void loadLevelToUI()
    {
        float[] positionXOptions = { 0f, -300f, 0f, 300f };
        // load sample vào content
        RectTransform contetnRect = ContentView.GetComponent<RectTransform>();
        float contentHeight = levels.Length * 800 - 450 * 2;
        contetnRect.sizeDelta = new Vector2(contetnRect.sizeDelta.x, contentHeight);

        for (int i = 1; i <= levels.Length; i++)
        {
            int patternIndex;
            if (i == 1)
            {
                patternIndex = 0;

            } else {
                patternIndex = i % 4;
            }
            float posX = positionXOptions[patternIndex];
            float posY = -450f * i;

            //load level index vào sample
            LevelSampleText.text = i.ToString();
            levelItem = Instantiate(LevelSample, ContentView.transform);
            levelItem.name = i.ToString();
            levelItem.SetActive(true);
            //load position cho mỗi sample
            levelPosition = new Vector2(posX, posY);
            levelItem.GetComponent<RectTransform>().anchoredPosition = levelPosition;
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
        targetObstacleAmount = activeCount;
        targetObstacleTMP.text = targetObstacleAmount.ToString();

    }

    public void initData()
    {
        isWaiting = false;
        levelIndex = PlayerPrefsManager.GetCurrentLevel();
        gamePlay = levels[levelIndex-1].transform.Find("GamePlay");
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
            // Tìm xem có gameobject nào là obs không, có thì hiển thị lên UI.
            foreach (Transform child in obs_list)
            {
                foundObstacle = child.gameObject;
                break;
            }

            // Hiển thị obs lên UI (làm đỡ)
            if (foundObstacle)
            {
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
                }
            }
        }



        // Hiển thị UI của move level
        if (currentObj.CompareTag("moveLevel"))
        {
            // move level
            Timer.SetActive(false);
            Move.SetActive(true);

            stepMoveText.text = gamePlay.GetChild(0).name;
            GameManager.Instance.step = int.Parse(stepMoveText.text);

        }
        //Hiển thị Ui của timerlevel
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
        //Xóa level hiện tại
        if (currentObj)
        {
            Destroy(currentObj);
        }
        //Nếu hết level thì quay lại level 1 (làm đỡ)
        if (levelIndex >= levels.Length)
        {
            PlayerPrefsManager.SetCurrentLevel(1);
        }
        //Instantiate lại level hiện tại
        currentObj = Instantiate(levels[PlayerPrefsManager.GetCurrentLevel() - 1]);
        //Xóa pool
        FishPooler.Instance.ClearPool();
        //Get fish đầu tiên
        CancelInvoke(nameof(finishGame));
        //restart lại trạng thái
        GameManager.Instance.restartGameAdventure();
        //Khởi tạo lại data
        initData();

        //Handle lỗi invoke
        if (!GameManager.Instance.isCancleDelayDrop)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(PlayerController.waterWidth, PlayerController.waterHeight, 10));
            FishManager.Instance.CreateFish(touchPosition);
        }
        PlayerController.Instance.setLinePosition();
    }

    //Handle next level
    public void onLoadNextLevel()
    {
        //++ tiền
        GameManager.Instance.setCoinText(50);
        // tăng level
        if (PlayerPrefsManager.GetCurrentLevel() < PlayerPrefsManager.GetUnLockedLevel())
        {
            levelIndex = PlayerPrefsManager.GetCurrentLevel();
            PlayerPrefsManager.SetCurrentLevel(levelIndex + 1);
        } else
        {
            levelIndex = PlayerPrefsManager.GetUnLockedLevel();
            PlayerPrefsManager.SetUnLockedLevel(levelIndex + 1);
            PlayerPrefsManager.SetCurrentLevel(PlayerPrefsManager.GetUnLockedLevel());
        }
            

        if (currentObj)
        {
            Destroy(currentObj);
        }
        if (levelIndex >= levels.Length)
        {
            PlayerPrefsManager.SetCurrentLevel(1);
        }
        currentObj = Instantiate(levels[PlayerPrefsManager.GetCurrentLevel() - 1]);
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
                    Invoke(nameof(finishGame), 3f);
                }
            }
            else if (currentObj.CompareTag("timerLevel"))
            {
                if (currentTime == 0)
                {
                    isWaiting = true;
                    Invoke(nameof(finishGame), 3f);
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
            GameManager.Instance.onWinAdventure();
        }
        else
        {
            GameManager.Instance.onLoseAdventure();
        }
    }
    private int GetFishLevel(string tag)
    {
        string[] parts = tag.Split('_');
        return int.Parse(parts[1]) - 1;
    }


}
