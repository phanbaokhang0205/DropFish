using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
    public int levelIndex;
    [SerializeField] GameObject HomeScreen;
    [SerializeField] GameObject AdventureScreen;
    [SerializeField] GameObject StoreScreen;
    [SerializeField] TextMeshProUGUI[] levels;
    private void Awake()
    {
        Instance = this;
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("GamePlay");
    }

    public void PlayChallenge()
    {
        SceneManager.LoadSceneAsync("Challenge");
    }

    public void goToHome()
    {
        AdventureScreen.SetActive(false);
        StoreScreen.SetActive(false);
        HomeScreen.SetActive(true);
    }

    public void goToChallenge()
    {
        AdventureScreen.SetActive(true);
        StoreScreen.SetActive(false);
        HomeScreen.SetActive(false);
    }

    public void goToShop()
    {
        AdventureScreen.SetActive(false);
        StoreScreen.SetActive(true);
        HomeScreen.SetActive(false);
    }

    public void getLevelIndex(TextMeshProUGUI uiText)
    {
        string textValue = uiText.text;

        if (int.TryParse(textValue, out int number))
        {
            levelIndex = number - 1;
            PlayChallenge();
        }
        else
        {
            Debug.LogWarning("Không chuyển được sang int");
        }
    }
}
