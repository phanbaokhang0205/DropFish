using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] GameObject[] levels;
    public GameObject Move;
    public TextMeshProUGUI stepMoveText;
    public TextMeshProUGUI targetFishTMP;
    public GameObject Timer;
    public TextMeshProUGUI timerTMP;
    public int targetAmount;
    public int targetFishTag;
    public bool isWaiting;

    private GameObject currentObj;
    private int levelIndex;
    private Transform gamePlay;
    private float currentTime;

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
        GameObject newParent = GameObject.Find("target1");


        targetFishTag = GetFishLevel(listParent.GetChild(0).tag);
        GameObject targetFish = FishPooler.Instance.GetFishInUI(targetFishTag);

        targetFish.transform.localScale = new Vector3(1f, 1f, 1f);    
        targetFish.transform.SetParent(newParent.transform);
        targetFish.transform.localPosition = new Vector3(0f, 0f, 50f);

        targetFishTMP.text = listParent.GetChild(1).name;
        targetAmount = int.Parse(listParent.GetChild(1).name);

        if (currentObj.CompareTag("moveLevel"))
        {
            // move level
            Timer.SetActive(false);
            Move.SetActive(true);

            stepMoveText.text = gamePlay.GetChild(0).name;
            GameManager.Instance.step = int.Parse(stepMoveText.text);
            Debug.Log("child name: " + gamePlay.GetChild(0).name);

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
                if (targetAmount == 0)
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
                if (targetAmount == 0)
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
        GameManager.Instance.restartGame();
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
        GameManager.Instance.restartGame();
    }

    public void checkGoalInChallenge(int levelFish)
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            
            if (targetFishTag == levelFish)
            {
                targetAmount--;
                targetFishTMP.text = targetAmount.ToString();
            }

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
        GameManager.Instance.onWin();
    }
    void finishGame()
    {
        if (targetAmount != 0)
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
