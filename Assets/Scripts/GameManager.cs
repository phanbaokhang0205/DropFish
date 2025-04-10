using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject WinCanvas;
    public GameObject LoseCanvas;
    public GameObject PauseCanvas;

    public enum GameState { Playing, Pause, Win, Lose };
    public GameState CurrentState;
    public int score;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentState = GameState.Playing;
        score = 0;
        Debug.Log("Keep going !");
    }

    public void onWin()
    {
        CurrentState = GameState.Win;
        WinCanvas.SetActive(true);
        Debug.Log("You Win");
    }

    public void onLose()
    {
        CurrentState = GameState.Lose;
        LoseCanvas.SetActive(true);
        Debug.Log("You lose");
    }

    public void onPause()
    {
        CurrentState = GameState.Pause;
        PauseCanvas.SetActive(true);
        Debug.Log("Wait a minute!");
    }

    public void onResume()
    {
        CurrentState = GameState.Playing;
        PauseCanvas.SetActive(false);
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void updateScore(int level)
    {
        score += (level * 2 + 2);
    }
}
