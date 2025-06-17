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

}
