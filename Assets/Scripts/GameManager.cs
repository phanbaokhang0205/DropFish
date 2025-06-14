using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameObject WinCanvas;
    public GameObject LoseCanvas;
    public GameObject PauseCanvas;

    public enum GameState { Playing, Pause, Win, Lose, onChosen };
    public GameState CurrentState;
    public int score;
    public int step;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentState = GameState.Playing;
        score = 0;
        
    }

    public void onWin()
    {
        CurrentState = GameState.Win;
        WinCanvas.SetActive(true);
    }

    public void onLose()
    {
        CurrentState = GameState.Lose;
        LoseCanvas.SetActive(true);
    }

    public void onPause()
    {
        Time.timeScale = 0f;
        CurrentState = GameState.Pause;
        PauseCanvas.SetActive(true);
    }

    public void onResume()
    {
        Time.timeScale = 1f;
        CurrentState = GameState.Playing;
        PauseCanvas.SetActive(false);
    }

    public void onNextLevel()
    {
        CurrentState = GameState.Playing;
        WinCanvas.SetActive(false);
    }

    public void restartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void restartGameAdvance()
    {
        CurrentState = GameState.Playing;
        LoseCanvas.SetActive(false);
        delayState();
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
