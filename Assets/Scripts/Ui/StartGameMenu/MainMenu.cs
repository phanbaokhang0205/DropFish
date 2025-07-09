using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
    public int levelIndex;
    [SerializeField] GameObject HomeScreen;
    [SerializeField] GameObject AdventureScreen;
    [SerializeField] GameObject StoreScreen;
    [SerializeField] GameObject SettingScreen;
    [SerializeField] GameObject panel_SettingScreen;
    [SerializeField] GameObject frame_SettingScreen;
    [SerializeField] TextMeshProUGUI[] levels;

    [SerializeField] GameObject StartGameUI;
    [SerializeField] GameObject NormalMode;
    [SerializeField] GameObject AdventureMode;
    RectTransform panel_rt;
    RectTransform frame_rt;
    Image panel_image;
    public int currentMode;

    private bool openSetting;
    private void Awake()
    {
        Instance = this;
        openSetting = false;
    }

    private void Start()
    {
        panel_rt = panel_SettingScreen.GetComponent<RectTransform>();
        frame_rt = frame_SettingScreen.GetComponent<RectTransform>();
        panel_image = panel_SettingScreen.GetComponent<Image>();
    }

    public void PlayNormalMode()
    {
        StartGameUI.SetActive(false);
        AdventureMode.SetActive(false);
        NormalMode.SetActive(true);
        setCurrentMode(NormalMode);
    }

    public void PlayAdventureMode()
    {
        StartGameUI.SetActive(false);
        AdventureMode.SetActive(true);
        NormalMode.SetActive(false);
        setCurrentMode(AdventureMode);
    }

    public void PlayLastestLevel()
    {
        StartGameUI.SetActive(false);
        AdventureMode.SetActive(true);
        NormalMode.SetActive(false);
        currentMode = 2;
        LevelManager.Instance.currentObj = Instantiate(LevelManager.Instance.levels[PlayerPrefsManager.GetUnLockedLevel() - 1]);
        LevelManager.Instance.initData();
    }
    public void GotoMainMenu()
    {
        Time.timeScale = 1f;
        if (GameManager.Instance.CurrentState == GameManager.GameState.Win)
        {
            GameManager.Instance.setCoinText(50);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //StartGameUI.SetActive(true);
        //AdventureMode.SetActive(false);
        //NormalMode.SetActive(false);
        //setCurrentMode(StartGameUI);
    }
    public void setCurrentMode(GameObject crMode)
    {
        if (crMode == NormalMode)
        {
            currentMode = 1;
        }

        if (crMode == AdventureMode)
        {
            currentMode = 2;
            //LevelManager.Instance.levelIndex = levelIndex;
            //LevelManager.Instance.levelIndex = PlayerPrefsManager.GetCurrentLevel();
            LevelManager.Instance.currentObj = Instantiate(LevelManager.Instance.levels[PlayerPrefsManager.GetCurrentLevel() - 1]);
            LevelManager.Instance.initData();
            Debug.Log(LevelManager.Instance.levelIndex);

        }
        if (crMode == StartGameUI)
        {
            currentMode = 0;
        }

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

    public void handleSetting()
    {
        openSetting = !openSetting;
        handleSettingAnim(openSetting, panel_rt, frame_rt, panel_image);
    }

    public Sequence handleSettingAnim(bool isOpen, RectTransform panel, RectTransform frame, Image image)
    {
        Sequence settingSeq = DOTween.Sequence();
        if (isOpen)
        {
            settingSeq
            .Append(panel.DOAnchorPos(new Vector2(0f, panel.anchoredPosition.y), 0f))
            .Join(image.DOFade(0.7f, 0.3f))
            .Join(frame.DOAnchorPos(new Vector2(0f, frame.anchoredPosition.y), 0.4f));
        }
        else
        {
            settingSeq
            .Append(image.DOFade(0f, 0.3f))
            .AppendInterval(0.1f)
            .Append(frame.DOAnchorPos(new Vector2(1000f, frame.anchoredPosition.y), 0.4f))
            .Join(panel.DOAnchorPos(new Vector2(1000f, panel.anchoredPosition.y), 0.2f));
        }

        return settingSeq;
    }
    //public void getLevelIndex(TextMeshProUGUI uiText)
    //{
    //    string textValue = uiText.text;

    //    if (int.TryParse(textValue, out int number))
    //    {
    //        levelIndex = number - 1;
    //        PlayAdventureMode();
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Không chuyển được sang int");
    //    }
    //}

    public void getLevelIndex(TextMeshProUGUI uiText)
    {
        string textValue = uiText.text;

        if (int.TryParse(textValue, out int number))
        {
            levelIndex = number - 1;
            PlayAdventureMode();
        }
        else
        {
            Debug.LogWarning("Không chuyển được sang int");
        }
    }
}
