﻿using System.Collections.Generic;
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
    [SerializeField] TextMeshProUGUI[] levels;

    [SerializeField] GameObject StartGameUI;
    [SerializeField] GameObject NormalMode;
    [SerializeField] GameObject AdventureMode;

    public int currentMode;

    private bool openSetting;
    private void Awake()
    {
        Instance = this;
        openSetting = false;
    }

    public void PlayNormalMode()
    {
        StartGameUI.SetActive(false);
        AdventureMode.SetActive(false);
        NormalMode.SetActive(true);
        setCurrentMode(NormalMode);
        Debug.Log("mode:" + currentMode);

    }

    public void PlayAdventureMode()
    {
        StartGameUI.SetActive(false);
        AdventureMode.SetActive(true);
        NormalMode.SetActive(false);
        setCurrentMode(AdventureMode);
        Debug.Log("mode:" + currentMode);
    }

    public void setCurrentMode(GameObject crMode)
    {
        if (crMode == NormalMode)
        {
            currentMode = 1;
            Debug.Log("OKe 1");
        }

        if (crMode == AdventureMode)
        {
            currentMode = 2;
            Debug.Log("OKe 2");

        }
        if (crMode == StartGameUI)
        {
            currentMode = 0;
            Debug.Log("OKe 3");
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
        SettingScreen.SetActive(openSetting);
    }

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
