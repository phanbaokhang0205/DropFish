using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    /// best score UI
    [SerializeField] TextMeshProUGUI bestScoreTMP;
    [SerializeField] TextMeshProUGUI currentScoreTMP;
    [SerializeField] TextMeshProUGUI bestScoreLoseCanvasTMP;
    [SerializeField] TextMeshProUGUI currentScoreLoseCanvasTMP;

    public enum GameState { Playing, Pause, Win, Lose, onChosen, delayBeforeDrop };
    public GameState CurrentState;
    public int score;
    public int step;
    public int live;
    public int bestScore;
    public int currentScore;
    public bool isCancleDelayDrop;
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

        isCancleDelayDrop = false;

    }
    private void setLiveText(int liveValue)
    {
        if (liveValue >= 5)
        {
            liveBarTMP.text = "full";
            liveBarHomeTMP.text = "full";
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
