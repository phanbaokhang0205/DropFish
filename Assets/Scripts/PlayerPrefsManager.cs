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


    // SOUND
    public static void SetSound(bool isOn)
    {
        PlayerPrefs.SetInt(SoundKey, isOn ? 1 : 0);
    }

    public static bool GetSound()
    {
        return PlayerPrefs.GetInt(SoundKey, 1) == 1; // mặc định là bật
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

    // LIVE
    public static void SetLastExitTime(string time)
    {
        PlayerPrefs.SetString(LastExitTimeKey, time);
    }

    public static string GetLastExitTime()
    {
        return PlayerPrefs.GetString(LastExitTimeKey, "0");
    }

    // COIN
    public static void SetCoin(int coin)
    {
        PlayerPrefs.SetInt(CoinKey, coin);
    }

    public static int GetCoin()
    {
        return PlayerPrefs.GetInt(CoinKey, 0);
    }

    // Last Close Time 
    public static void SetLastCloseTime(string time)
    {
        PlayerPrefs.SetString(lastCloseTimeKey, time);
    }

    public static string GetLastCloseTime()
    {
        return PlayerPrefs.GetString(lastCloseTimeKey, "00:00");
    }

}
