using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] GameObject[] levels;
    public GameObject Move;
    public TextMeshProUGUI stepMoveText;
    public TextMeshProUGUI targetFishTMP;
    public GameObject Timer;
    public TextMeshProUGUI timerTMP;

    public int targetFishTag;
    public bool isWaiting;

    private GameObject currentObj;
    private int levelIndex;
    private Transform gamePlay;
    private float currentTime;
    List<int> targetList = new List<int>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        isWaiting = false;
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            levelIndex = MainMenu.Instance.levelIndex;
            currentObj = Instantiate(levels[levelIndex]);

            initData();
        }
        
    }

    private void initData()
    {
        isWaiting = false;
        gamePlay = levels[levelIndex].transform.Find("GamePlay");
        Transform listParent = gamePlay.GetChild(1);

        targetList.Clear();
        for (int i = 0; i < listParent.childCount; i++)
        {
            targetFishTag = GetFishLevel(listParent.GetChild(i).tag);
            targetList.Add(targetFishTag);
        }

        if (currentObj.CompareTag("moveLevel"))
        {
            // move level
            Timer.SetActive(false);
            Move.SetActive(true);

            stepMoveText.text = gamePlay.GetChild(0).name;
            GameManager.Instance.step = int.Parse(stepMoveText.text);

        }
        else if (currentObj.CompareTag("timerLevel"))
        {
            // timer level
            Timer.SetActive(true);
            Move.SetActive(false);

            Transform second = gamePlay.GetChild(2);
            currentTime = float.Parse(second.name);
        }
    }

    
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (currentObj.CompareTag("moveLevel"))
            {
                stepMoveText.text = GameManager.Instance.step.ToString();
                if (targetList.Count == 0)
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
                if (targetList.Count == 0)
                {
                    isWaiting = true;
                    Invoke("onWin", 2f);
                }
                timerTMP.text = Mathf.CeilToInt(currentTime).ToString();
            }

        }

    }

    public void restartData()
    {
        FishPooler.Instance.ClearPool();
        FishPooler.Instance.InitializePool();
        if (currentObj)
        {
            Destroy(currentObj);
        }
        currentObj = Instantiate(levels[levelIndex]);
        initData();
        GameManager.Instance.restartGameAdvance();
        CancelInvoke("finishGame");
    }

    public void onLoadNextLevel()
    {
        FishPooler.Instance.ClearPool();
        FishPooler.Instance.InitializePool();
        levelIndex++;
        if (currentObj)
        {
            Destroy(currentObj);
        }

        if (levelIndex >= levels.Length)
        {
            levelIndex = 0;
        }
        currentObj = Instantiate(levels[levelIndex]);
        initData();
        GameManager.Instance.onNextLevel();
        CancelInvoke("onWin");
    }

    public void checkGoalInChallenge(int levelFish)
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            for (int i = targetList.Count - 1; i >= 0; i--)
            {
                if (targetList[i] == levelFish)
                {
                    targetList.Remove(levelFish);
                    break;
                }
            }
            string rs = "";
            for (int i = 0; i < targetList.Count; i++)
            {
                rs += $"fish_{targetList[i] + 1}\n";
            }
            targetFishTMP.text = $"{rs}";

            if (currentObj.CompareTag("moveLevel"))
            {
                if (GameManager.Instance.step == 0)
                {
                    isWaiting = true;
                    Invoke("finishGame", 3f);
                }
            } else if (currentObj.CompareTag("timerLevel"))
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
        GameManager.Instance.onWin();
    }
    void finishGame()
    {
        if (targetList.Count != 0)
        {
            GameManager.Instance.onLose();
        }
        else
        {
            GameManager.Instance.onWin();
        }
    }
    private int GetFishLevel(string tag)
    {
        string[] parts = tag.Split('_');
        return int.Parse(parts[1]) - 1;
    }
    
}
