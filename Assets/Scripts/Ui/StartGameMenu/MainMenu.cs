using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
    public int levelIndex;
    [SerializeField] GameObject Home;
    [SerializeField] GameObject Chanllenge;
    [SerializeField] GameObject Shop;
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
        Chanllenge.SetActive(false);
        Shop.SetActive(false);
        Home.SetActive(true);
    }

    public void goToChallenge()
    {
        Chanllenge.SetActive(true);
        Shop.SetActive(false);
        Home.SetActive(false);
    }

    public void goToShop()
    {
        Chanllenge.SetActive(false);
        Shop.SetActive(true);
        Home.SetActive(false);
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
