using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// - obstacle bug
/// - claim coin bug
/// - set fish line bug
/// - booster cost bug
/// </summary>
public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    ///win lose pause canvas
    [SerializeField] GameObject WinCanvasAdventure;
    [SerializeField] GameObject LoseCanvasAdventure;
    [SerializeField] GameObject PauseCanvasAdventure; 
    [SerializeField] GameObject WinCanvasNormal;
    [SerializeField] GameObject LoseCanvasNormal;
    [SerializeField] GameObject PauseCanvasNormal;

    /// live UI
    [SerializeField] TextMeshProUGUI liveBarTMP;
    [SerializeField] TextMeshProUGUI liveBarHomeTMP;
    [SerializeField] TextMeshProUGUI liveTimeTMP;
    

    /// best score UI
    [SerializeField] TextMeshProUGUI bestScoreTMP;
    [SerializeField] TextMeshProUGUI currentScoreTMP;
    [SerializeField] TextMeshProUGUI bestScoreLoseCanvasTMP;
    [SerializeField] TextMeshProUGUI currentScoreLoseCanvasTMP;

    ///Coin UI
    [SerializeField] TextMeshProUGUI homeScreenCoinTMP;
    [SerializeField] TextMeshProUGUI normalScreenCoinTMP;
    [SerializeField] TextMeshProUGUI adventureScreenCoinTMP;

    [SerializeField] Button adventurePlayBtn;


    public enum GameState { Playing, Pause, Win, Lose, onChosen, delayBeforeDrop };
    public GameState CurrentState;
    public int score;
    public int step;
    public int bestScore;
    public int currentScore;
    public bool isCancleDelayDrop;
    public int totalCoin;
    //live
    const int maxLiveValue = 5;
    const float countDownLiveTimeValue = 900f; //15:00

    public int live;
    public float liveTime; 
    public float currentLiveTime;
    System.TimeSpan timePassed;
    System.DateTime lastQuitTime;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 120;

        CurrentState = GameState.Playing;
        score = 0;
        live = PlayerPrefsManager.GetLive();
        setLiveText(live);

        bestScore = PlayerPrefsManager.GetBestScore();
        setBestScore();
        
        totalCoin = PlayerPrefsManager.GetCoin();
        homeScreenCoinTMP.text = PlayerPrefsManager.GetCoin().ToString();
        normalScreenCoinTMP.text = PlayerPrefsManager.GetCoin().ToString();
        adventureScreenCoinTMP.text = PlayerPrefsManager.GetCoin().ToString();
        isCancleDelayDrop = false;
        
        updateLifeWhenReopenApp();
        if (PlayerPrefsManager.GetCurrentTime() == -1)
        {
            currentLiveTime = PlayerPrefsManager.GetLastExitTime();
        } else
        {
            currentLiveTime = PlayerPrefsManager.GetCurrentTime();
        }
    }

    private void Update()
    {
        countDownLiveTime();
        PlayerPrefsManager.SetCurrentTime(currentLiveTime);
        if (live==0)
        {
            adventurePlayBtn.interactable = false;
        } else
        {
            adventurePlayBtn.interactable = true;
        }

    }
    private void OnApplicationQuit()
    {
        PlayerPrefsManager.SetLastCloseTime(System.DateTime.UtcNow.ToString());
        PlayerPrefsManager.SetLastExitTime(currentLiveTime);
        PlayerPrefsManager.SetCurrentTime(-1);  
    }


    /// <summary>
    /// Handle live logic
    /// </summary>
    public void countDownLiveTime()
    {

        live = PlayerPrefsManager.GetLive();
        if (live < 5)
        {
            currentLiveTime -= Time.unscaledDeltaTime;

            int minutes = Mathf.FloorToInt(currentLiveTime / 60);
            int seconds = Mathf.FloorToInt(currentLiveTime % 60);
            liveTimeTMP.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (currentLiveTime <= 0)
            {
                PlayerPrefsManager.SetLive(live + 1);
                currentLiveTime = countDownLiveTimeValue;
            }
        }

        setLiveText(live);
        PlayerPrefsManager.SetLastCloseTime(System.DateTime.UtcNow.ToString());
    }

    //IEnumerator Corou_1()
    //{
    //    while(true)
    //    {

    //    }
    //}

    public void updateLifeWhenReopenApp()
    {
        if (live >= maxLiveValue) return;

        // currentLiveTime để lưu lại thời gian count down để hồi lại tim lúc thoát
        currentLiveTime = PlayerPrefsManager.GetLastExitTime();
        // lastQuiTime để lưu lại thời gian lúc thoát -> tính toán thời gian đã trôi qua so với now()
        lastQuitTime = System.DateTime.Parse(PlayerPrefsManager.GetLastCloseTime());
        
        timePassed = System.DateTime.UtcNow - lastQuitTime;

        liveTime = currentLiveTime - (float)timePassed.TotalSeconds;

        if (liveTime > 0)
        {

            currentLiveTime = liveTime;
            PlayerPrefsManager.SetLastExitTime(liveTime);
        } 
        if (liveTime == 0)
        {
            currentLiveTime = liveTime;
            PlayerPrefsManager.SetLastExitTime(liveTime);
            PlayerPrefsManager.SetLive(live + 1);
            setLiveText(live + 1);
        }
        if (liveTime < 0)
        {
            while (live < maxLiveValue && liveTime < 0)
            {
                liveTime += countDownLiveTimeValue;
                live++;
                PlayerPrefsManager.SetLive(live);
            }
            PlayerPrefsManager.SetLastExitTime(liveTime);

        }
        
    }
    private void setLiveText(int liveValue)
    {
        if (liveValue >= maxLiveValue)
        {
            liveBarHomeTMP.text = liveValue.ToString();
            liveBarTMP.text = "full";
            liveTimeTMP.text = "full";
        }
        else
        {
            liveBarTMP.text = liveValue.ToString();
            liveBarHomeTMP.text = liveValue.ToString();
        }
    }

    /// <summary>
    /// Handle Coin logic
    /// </summary>
    public void setCoinText(int coin)
    {
        totalCoin += coin;
        PlayerPrefsManager.SetCoin(totalCoin);
        homeScreenCoinTMP.text = PlayerPrefsManager.GetCoin().ToString();
        normalScreenCoinTMP.text = PlayerPrefsManager.GetCoin().ToString();
        adventureScreenCoinTMP.text = PlayerPrefsManager.GetCoin().ToString();
    }

    public bool isAvailableCoin(int coin)
    {
        int avai_coin = PlayerPrefsManager.GetCoin();
        if (coin > avai_coin)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Handle Score logic
    /// </summary>

    private void setBestScore()
    {
        currentScore = int.Parse(currentScoreTMP.text);
        if (currentScore > bestScore)
        {
            PlayerPrefsManager.SetBestScore(currentScore);
        } else
        {
            PlayerPrefsManager.SetBestScore(bestScore);
        }
        bestScoreTMP.text = PlayerPrefsManager.GetBestScore().ToString();
        bestScoreLoseCanvasTMP.text = PlayerPrefsManager.GetBestScore().ToString();
        currentScoreLoseCanvasTMP.text = currentScore.ToString();
    }

    public void updateScore(int level)
    {
        score += (level * 2 + 2);
    }

    /// <summary>
    /// Handle GameState logic
    /// </summary>
    public void onWinAdventure()
    {
        CurrentState = GameState.Win;
        WinCanvasAdventure.SetActive(true);
    }

    public void onLoseAdventure()
    {
        CurrentState = GameState.Lose;
        LoseCanvasAdventure.SetActive(true);
        PlayerPrefsManager.SetLive(live-1);
        liveBarTMP.text = PlayerPrefsManager.GetLive().ToString();
    }

    public void onPauseAdventure()
    {
        Time.timeScale = 0f;
        CurrentState = GameState.Pause;
        PauseCanvasAdventure.SetActive(true);
    }

    public void onResumeAdventure()
    {
        Time.timeScale = 1f;
        CurrentState = GameState.Playing;
        PauseCanvasAdventure.SetActive(false);
        delayState(0.1f);
    }

    public void onWinNormal()
    {
        CurrentState = GameState.Win;
        WinCanvasNormal.SetActive(true);
        setBestScore();
    }

    public void onLoseNormal()
    {
        CurrentState = GameState.Lose;
        LoseCanvasNormal.SetActive(true);
        setBestScore();
    }

    public void onPauseNormal()
    {
        Time.timeScale = 0f;
        CurrentState = GameState.Pause;
        PauseCanvasNormal.SetActive(true);
    }

    public void onResumeNormal()
    {
        Time.timeScale = 1f;
        CurrentState = GameState.Playing;
        PauseCanvasNormal.SetActive(false);
        delayState(0.1f);
    }

    public void onNextLevelAdventure()
    {
        CurrentState = GameState.Playing;
        WinCanvasAdventure.SetActive(false);
        delayState(0.1f);
    }

    public void restartGameAdventure()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        CurrentState = GameState.Playing;
        LoseCanvasAdventure.SetActive(false);
        PauseCanvasAdventure.SetActive(false);
        delayState(0.1f);
    }

    public void restartGameNormal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    

    public void updateStep()
    {
        step -= 1;
    }

    public void delayState(float time)
    {
        CurrentState = GameState.onChosen;
        Invoke(nameof(deplayStateInvoke), time);
    }
    private void deplayStateInvoke()
    {
        CurrentState = GameState.Playing;
    }

    public void goToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuGame");
    }
}
