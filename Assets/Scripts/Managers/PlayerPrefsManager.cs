using UnityEngine;

public static class PlayerPrefsManager
{
    // Keys
    private const string SoundKey = "Sound";
    private const string MusicKey = "Music";
    private const string BestScoreKey = "BestScore";
    private const string LiveKey = "Live";
    private const string LastExitTimeKey = "LastExitTime";
    private const string CoinKey = "Coin";
    private const string lastCloseTimeKey = "LasteCloseTime";
    private const string currentTimeKey = "CurrentTime";

    private const string unLockedLevelKey = "UnLockedLevel";
    private const string currentLevelKey = "CurrentLevel";


    // SOUND
    public static void SetSound(bool isOn)
    {
        PlayerPrefs.SetInt(SoundKey, isOn ? 1 : 0);
    }

    public static bool GetSound()
    {
        return PlayerPrefs.GetInt(SoundKey, 1) == 1;
    }

    // MUSIC
    public static void SetMusic(bool isOn)
    {
        PlayerPrefs.SetInt(MusicKey, isOn ? 1 : 0);
    }

    public static bool GetMusic()
    {
        return PlayerPrefs.GetInt(MusicKey, 1) == 1;
    }

    // BEST SCORE
    public static void SetBestScore(int score)
    {
        PlayerPrefs.SetInt(BestScoreKey, score);
    }

    public static int GetBestScore()
    {
        return PlayerPrefs.GetInt(BestScoreKey, 0);
    }

    // LIVE
    public static void SetLive(int live)
    {
        PlayerPrefs.SetInt(LiveKey, live);
    }

    public static int GetLive()
    {
        return PlayerPrefs.GetInt(LiveKey, 5);
    }
    public static float GetLastExitTime()
    {
        return PlayerPrefs.GetFloat(LastExitTimeKey, 0f);
    }
    public static void SetLastExitTime(float time)
    {
        PlayerPrefs.SetFloat(LastExitTimeKey, time);
    }
    public static void SetLastCloseTime(string time)
    {
        PlayerPrefs.SetString(lastCloseTimeKey, time);
    }

    public static string GetLastCloseTime()
    {
        return PlayerPrefs.GetString(lastCloseTimeKey, "00:00");
    }

    public static float GetCurrentTime()
    {
        return PlayerPrefs.GetFloat(currentTimeKey, 0f);
    }

    public static void SetCurrentTime(float time)
    {
        PlayerPrefs.SetFloat(currentTimeKey, time);
    }



    // COIN
    public static void SetCoin(int coin)
    {
        PlayerPrefs.SetInt(CoinKey, coin);
    }

    public static int GetCoin()
    {
        return PlayerPrefs.GetInt(CoinKey, 99999);
    }
   
    public static void SetUnLockedLevel(int level)
    {
        PlayerPrefs.SetInt(unLockedLevelKey, level);
    }

    public static int GetUnLockedLevel()
    {
        return PlayerPrefs.GetInt(unLockedLevelKey, 1);
    }
    public static void SetCurrentLevel(int level)
    {
        PlayerPrefs.SetInt(currentLevelKey, level);
    }

    public static int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt(currentLevelKey, 1);
    }
}
