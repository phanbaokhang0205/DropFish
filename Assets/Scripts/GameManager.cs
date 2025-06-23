using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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



    public enum GameState { Playing, Pause, Win, Lose, onChosen, delayBeforeDrop };
    public GameState CurrentState;
    public int score;
    public int step;
    public int live;
    public int bestScore;
    public int currentScore;
    public bool isCancleDelayDrop;
    public int totalCoin;
    public float currentLiveTime;
    const float totalLiveTime = 5f * 10f; //5 life * 10s
    System.TimeSpan timePassed;
    System.DateTime lastQuitTime;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

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
        currentLiveTime = 5f;

        lastQuitTime = System.DateTime.Parse(PlayerPrefsManager.GetLastCloseTime());
        timePassed = System.DateTime.UtcNow - lastQuitTime;
        Debug.Log("lastQuitTime: " + lastQuitTime);
        Debug.Log("Time passed: " + timePassed.TotalSeconds);
        Debug.Log("Time passed: " + timePassed.TotalSeconds);
        currentLiveTime = totalLiveTime - (float)timePassed.TotalSeconds;

    }

    private void Update()
    {
        countDownLiveTime();
        PlayerPrefsManager.SetLastCloseTime(System.DateTime.UtcNow.ToString());
        
        Debug.Log("DateTime:" + System.DateTime.UtcNow.Second);
    }

    public void countDownLiveTime()
    {
        live = PlayerPrefsManager.GetLive();
        if (live < 5)
        {
            currentLiveTime -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(currentLiveTime / 60);
            int seconds = Mathf.FloorToInt(currentLiveTime % 60);
            liveTimeTMP.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (currentLiveTime <= 0)
            {
                PlayerPrefsManager.SetLive(live + 1);
                currentLiveTime = 5f;
            }
        }

        setLiveText(live);
    }

    public void setCoinText(int coin)
    {
        totalCoin += coin;
        PlayerPrefsManager.SetCoin(totalCoin);
        homeScreenCoinTMP.text = PlayerPrefsManager.GetCoin().ToString();
        normalScreenCoinTMP.text = PlayerPrefsManager.GetCoin().ToString();
        adventureScreenCoinTMP.text = PlayerPrefsManager.GetCoin().ToString();
    }
    private void setLiveText(int liveValue)
    {
        if (liveValue >= 5)
        {
            liveBarHomeTMP.text = liveValue.ToString();
            liveBarTMP.text = "full";
            liveTimeTMP.text = "full";
        } else
        {
            liveBarTMP.text = liveValue.ToString();
            liveBarHomeTMP.text = liveValue.ToString();
        }
    }
    
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
        delayState();
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
        delayState();
    }

    public void onNextLevelAdventure()
    {
        CurrentState = GameState.Playing;
        WinCanvasAdventure.SetActive(false);
        delayState();
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
        delayState();
    }

    public void restartGameNormal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void updateScore(int level)
    {
        score += (level * 2 + 2);
    }

    public void updateStep()
    {
        step -= 1;
    }

    public void delayState()
    {
        CurrentState = GameState.onChosen;
        Invoke("deplayStateInvoke", 0.1f);
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
